using System.ComponentModel.DataAnnotations;
using TicketTracer.Api.Models.Common.Abstract;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace TicketTracer.Api.Models.Common;

internal class AttributeValueModel : WithIdModel
{
    [Required]
    public string Value { get; set; } = null!;
}