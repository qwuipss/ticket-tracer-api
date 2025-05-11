using Microsoft.EntityFrameworkCore;
using TicketTracer.Api.Repositories.Abstract;
using TicketTracer.Data;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Repositories;

internal class BoardsRepository(TicketTracerDbContext dbContext)
    : BaseRepository<BoardEntity>(dbContext), IBoardsRepository
{
    public async Task<bool> IsExistAsync(string title, CancellationToken cancellationToken)
    {
        return await DbContext.Boards.AnyAsync(p => p.Title == title, cancellationToken);
    }

    public async Task<List<BoardEntity>> GetAllByProjectIdAsync(Guid projectId, int offset, int limit, CancellationToken cancellationToken)
    {
        return await DbContext
                     .Boards.Where(b => b.ProjectId == projectId && !b.IsDeleted)
                     .OrderBy(b => b.CreatedAt)
                     .Skip(offset)
                     .Take(limit)
                     .ToListAsync(cancellationToken);
    }

    public new async Task<Guid> AddAsync(BoardEntity entity, CancellationToken cancellationToken)
    {
        await using var transaction = await DbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var entry = await DbContext.Boards.AddAsync(entity, cancellationToken);
            await SaveChangesAsync(cancellationToken);

            var attributeEntities = CreateAttributeEntities(entry.Entity.Id);

            DbContext.Attributes.AddRange(attributeEntities);
            await SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return entry.Entity.Id;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<List<AttributeEntity>> GetAttributesAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext.Attributes.Where(a => a.BoardId == id && !a.IsDeleted).ToListAsync(cancellationToken);
    }

    private static IEnumerable<AttributeEntity> CreateAttributeEntities(Guid boardId)
    {
        yield return new AttributeEntity
        {
            Name = "Assignee",
            Type = AttributeType.User,
            BoardId = boardId,
        };

        yield return new AttributeEntity
        {
            Name = "Stage",
            Type = AttributeType.TicketStage,
            BoardId = boardId,
        };
    }
}