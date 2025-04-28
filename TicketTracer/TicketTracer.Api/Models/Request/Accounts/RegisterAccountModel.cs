using System.ComponentModel.DataAnnotations;
using TicketTracer.Api.Models.Binders;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Models.Request.Accounts;

internal class RegisterAccountModel
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(EntityConstraints.User.EmailMaxLength)]
    [EmailAddress]
    [LowerCase]
    public string Email { get; init; } = null!;

    [Required(AllowEmptyStrings = false)]
    [MinLength(RequestModelsConstraints.User.PasswordMinLength)]
    [MaxLength(RequestModelsConstraints.User.PasswordMaxLength)]
    public string Password { get; init; } = null!;

    [Required(AllowEmptyStrings = false)]
    [MaxLength(EntityConstraints.User.NameMaxLength)]
    public string Name { get; init; } = null!;

    [Required(AllowEmptyStrings = false)]
    [MaxLength(EntityConstraints.User.SurnameMaxLength)]
    public string Surname { get; init; } = null!;
}