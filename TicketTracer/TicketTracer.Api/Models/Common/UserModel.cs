using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TicketTracer.Api.Models.Common.Abstract;
using TicketTracer.Data.Entities;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace TicketTracer.Api.Models.Common;

internal class UserModel : WithIdModel
{
    [MaybeNull]
    [MinLength(1)]
    [MaxLength(EntityConstraints.User.EmailMaxLength)]
    public string Email { get; set; }

    [MaybeNull]
    [MinLength(1)]
    [MaxLength(EntityConstraints.User.NameMaxLength)]
    public string Name { get; set; }

    [MaybeNull]
    [MinLength(1)]
    [MaxLength(EntityConstraints.User.SurnameMaxLength)]
    public string Surname { get; set; }
}