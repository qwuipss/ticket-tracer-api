using Microsoft.EntityFrameworkCore;
using TicketTracer.Api.Models.Common;
using TicketTracer.Api.Repositories.Abstract;
using TicketTracer.Data;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Repositories;

internal class AttributeValuesRepository(TicketTracerDbContext dbContext) : BaseRepository<AttributeValueEntity>(dbContext), IAttributesValuesRepository
{
    public async Task<List<AttributeValueEntity>> GetAsync(Guid ticketId, CancellationToken cancellationToken)
    {
        return await DbContext.AttributeValues.Where(t => t.TicketId == ticketId && !t.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<AttributeType> GetAttributeTypeAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await DbContext.AttributeValues.Where(a => a.Id == id && !a.IsDeleted).SingleAsync(cancellationToken);
        var attributeEntity = await DbContext.Attributes.Where(a => a.Id == entity.AttributeId && !a.IsDeleted).SingleAsync(cancellationToken);
        return attributeEntity.Type;
    }

    public async Task PutAsync(AttributeValueModel model, CancellationToken cancellationToken)
    {
        var entity = await DbContext.AttributeValues.SingleAsync(a => a.Id == model.Id && !a.IsDeleted, cancellationToken);
        entity.Value = model.Value;
        await SaveChangesAsync(cancellationToken);
    }
}