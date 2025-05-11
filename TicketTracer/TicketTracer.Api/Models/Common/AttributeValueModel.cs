using System.ComponentModel.DataAnnotations;
using TicketTracer.Api.Models.Common.Abstract;

namespace TicketTracer.Api.Models.Common;

internal class AttributeValueModel : WithIdModel
{
    [Required]
    public string Value { get; set; } = null!;

    public Guid AttributeId { get; set; }
}