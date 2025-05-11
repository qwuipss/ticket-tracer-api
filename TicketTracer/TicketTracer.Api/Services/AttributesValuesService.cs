using AutoMapper;
using OneOf;
using TicketTracer.Api.Models.Common;
using TicketTracer.Api.Models.Response.AttributesValues;
using TicketTracer.Api.Repositories;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Services;

internal class AttributesValuesService(
    ILogger<AttributesValuesService> logger,
    IAttributesValuesRepository attributesValuesRepository,
    IUsersRepository usersRepository,
    IMapper mapper
) : IAttributesValuesService
{
    private readonly IAttributesValuesRepository _attributesValuesRepository = attributesValuesRepository;
    private readonly ILogger<AttributesValuesService> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IUsersRepository _usersRepository = usersRepository;

    public async Task<List<AttributeValueModel>?> GetAttributesValuesAsync(Guid id, CancellationToken cancellationToken)
    {
        var entities = await _attributesValuesRepository.GetAsync(id, cancellationToken);

        _logger.LogInformation("Retrieved {count} attributes values", entities.Count);

        if (entities.Count is 0)
        {
            return null;
        }

        var models = _mapper.Map<List<AttributeValueEntity>, List<AttributeValueModel>>(entities);

        return models;
    }

    public async Task<OneOf<AttributeValuePatchedModel, AttributeValueNotFoundModel, InvalidAttributeValueModel>> PutAsync(
        AttributeValueModel model,
        CancellationToken cancellationToken
    )
    {
        var isAttributeValueExist = await _attributesValuesRepository.IsExistAsync(model.Id, cancellationToken);
        if (!isAttributeValueExist)
        {
            _logger.LogWarning("Attribute value with specified id not found");
            return new AttributeValueNotFoundModel();
        }

        var attributeType = await GetAttributeTypeAsync(model.Id, cancellationToken);

        switch (attributeType)
        {
            case AttributeType.User when await ValidateAttributeValueWithTypeUserAsync(model.Value, cancellationToken):
                _logger.LogWarning(
                    "Validate value for attribute with type ({type}) failed. User with id ({id}) doesn't exist",
                    nameof(AttributeType.User),
                    model.Value
                );
                return new InvalidAttributeValueModel { Message = "User with such id doesn't exist", };
            case AttributeType.TicketStage when !ValidateAttributeValueWithTypeTicketStage(model.Value):
                _logger.LogWarning(
                    "Validate value for attribute with type ({type}) failed. Ticket stage with name ({id}) doesn't exist",
                    nameof(AttributeType.TicketStage),
                    model.Value
                );
                return new InvalidAttributeValueModel { Message = "Ticket stage with such name doesn't exist", };
            default:
                await _attributesValuesRepository.PutAsync(model, cancellationToken);
                return new AttributeValuePatchedModel();
        }
    }

    private static bool ValidateAttributeValueWithTypeTicketStage(string value)
    {
        return Enum.TryParse<AttributeType>(value, out _);
    }

    private async Task<AttributeType> GetAttributeTypeAsync(Guid attributeValueId, CancellationToken cancellationToken)
    {
        return await _attributesValuesRepository.GetAttributeTypeAsync(attributeValueId, cancellationToken);
    }

    private async Task<bool> ValidateAttributeValueWithTypeUserAsync(string value, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(value, out var userId))
        {
            return false;
        }

        return await _usersRepository.IsExistAsync(userId, cancellationToken);
    }
}