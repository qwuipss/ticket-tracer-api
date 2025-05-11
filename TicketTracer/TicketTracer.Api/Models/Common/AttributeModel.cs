using System.Diagnostics.CodeAnalysis;
using TicketTracer.Api.Models.Common.Abstract;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Models.Common;

internal class AttributeModel : WithIdModel
{
    [MaybeNull]
    public string Name { get; set; }

    public AttributeType Type { get; set; }
}