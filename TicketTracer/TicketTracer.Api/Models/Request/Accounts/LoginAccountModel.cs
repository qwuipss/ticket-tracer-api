using System.ComponentModel.DataAnnotations;
using TicketTracer.Api.Models.Binders;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Models.Request.Accounts;

internal class LoginAccountModel
{
    [Required(AllowEmptyStrings = false)]
    [LowerCase]
    [MaxLength(EntityConstraints.User.EmailMaxLength)]
    public string Email { get; init; } = null!;

    [Required(AllowEmptyStrings = false)]
    [MinLength(RequestModelsConstraints.User.PasswordMinLength)]
    [MaxLength(RequestModelsConstraints.User.PasswordMaxLength)]
    public string Password { get; init; } = null!;
}