using Microsoft.EntityFrameworkCore;
using TicketTracer.Api.Repositories.Abstract;
using TicketTracer.Data;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Repositories;

internal class TicketsRepository(TicketTracerDbContext dbContext) : BaseRepository<TicketEntity>(dbContext), ITicketsRepository
{
    public async Task<int> GetLastTicketNumberAsync(Guid boardId, CancellationToken cancellationToken)
    {
        try
        {
            return await DbContext.Tickets.Where(t => t.BoardId == boardId).MaxAsync(ticket => ticket.Number, cancellationToken);
        }
        catch (InvalidOperationException)
        {
            return 0;
        }
    }

    public async Task<List<TicketEntity>> GetAllByBoardIdAsync(Guid boardId, int offset, int limit, CancellationToken cancellationToken)
    {
        return await DbContext
                     .Tickets.Where(t => t.BoardId == boardId && !t.IsDeleted)
                     .OrderByDescending(ticket => ticket.CreatedAt)
                     .Skip(offset)
                     .Take(limit)
                     .ToListAsync(cancellationToken);
    }

    public new async Task<TicketEntity> AddAsync(TicketEntity ticket, CancellationToken cancellationToken)
    {
        var attributeEntities = await DbContext
                                      .Attributes.Where(a => a.BoardId == ticket.BoardId && !a.IsDeleted)
                                      .ToListAsync(cancellationToken);

        await using var transaction = await DbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var ticketEntry = await DbContext.Tickets.AddAsync(ticket, cancellationToken);

            await SaveChangesAsync(cancellationToken);

            var attributeValueEntities = CreateAttributeValueEntities(ticketEntry.Entity.Id, attributeEntities);

            await DbContext.AttributeValues.AddRangeAsync(attributeValueEntities, cancellationToken);
            await SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            return ticket;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static IEnumerable<AttributeValueEntity> CreateAttributeValueEntities(Guid ticketId, List<AttributeEntity> attributeEntities)
    {
        var groupedAttributes = attributeEntities
                                .GroupBy(a => a.Type)
                                .ToDictionary(g => g.Key, g => g.AsEnumerable());

        var assigneeAttributeValueEntity = CreateAssigneeAttributeValueEntity(ticketId, groupedAttributes[AttributeType.User].Single());
        var ticketStageAttributeValueEntity = CreateTicketStageAttributeValueEntity(ticketId, groupedAttributes[AttributeType.TicketStage].Single());

        return [assigneeAttributeValueEntity, ticketStageAttributeValueEntity,];
    }

    private static AttributeValueEntity CreateAssigneeAttributeValueEntity(Guid ticketId, AttributeEntity attribute)
    {
        return new AttributeValueEntity
        {
            Value = string.Empty,
            TicketId = ticketId,
            AttributeId = attribute.Id,
        };
    }

    private static AttributeValueEntity CreateTicketStageAttributeValueEntity(Guid ticketId, AttributeEntity attribute)
    {
        return new AttributeValueEntity
        {
            Value = nameof(TicketStage.ToDo),
            TicketId = ticketId,
            AttributeId = attribute.Id,
        };
    }
}