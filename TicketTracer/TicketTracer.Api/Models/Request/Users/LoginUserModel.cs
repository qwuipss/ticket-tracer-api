using System.ComponentModel.DataAnnotations;
using TicketTracer.Api.Models.Binders;
using TicketTracer.Data.Models;

namespace TicketTracer.Api.Models.Request.Users;

internal class LoginUserModel
{
    [Required(AllowEmptyStrings = false)]
    [LowerCase]
    [MaxLength(DboConstraints.EmailMaxLength)]
    public string Email { get; init; } = null!;
    
    [Required(AllowEmptyStrings = false)]
    [MinLength(RequestModelsConstraints.PasswordMinLength)]
    [MaxLength(RequestModelsConstraints.PasswordMaxLength)]
    public string Password { get; init; } = null!;
}