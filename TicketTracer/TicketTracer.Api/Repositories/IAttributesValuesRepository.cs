using TicketTracer.Api.Models.Common;
using TicketTracer.Api.Repositories.Abstract;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Repositories;

internal interface IAttributesValuesRepository : IBaseRepository<AttributeValueEntity>
{
    Task<List<AttributeValueEntity>> GetAsync(Guid id, CancellationToken cancellationToken);

    Task PutAsync(AttributeValueModel model, CancellationToken cancellationToken);

    Task<AttributeType> GetAttributeTypeAsync(Guid id, CancellationToken cancellationToken);
}