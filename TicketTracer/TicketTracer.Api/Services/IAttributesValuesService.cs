using OneOf;
using TicketTracer.Api.Models.Common;
using TicketTracer.Api.Models.Response.AttributesValues;

namespace TicketTracer.Api.Services;

internal interface IAttributesValuesService
{
    Task<List<AttributeValueModel>?> GetAttributesValuesAsync(Guid id, CancellationToken cancellationToken);

    Task<OneOf<AttributeValuePatchedModel, AttributeValueNotFoundModel, InvalidAttributeValueModel>> PutAsync(AttributeValueModel model, CancellationToken cancellationToken);
}