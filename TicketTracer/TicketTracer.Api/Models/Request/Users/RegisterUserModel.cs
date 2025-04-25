using System.ComponentModel.DataAnnotations;
using TicketTracer.Api.Models.Binders;
using TicketTracer.Data.Models;

namespace TicketTracer.Api.Models.Request.Users;

internal class RegisterUserModel
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(DboConstraints.EmailMaxLength)]
    [EmailAddress] 
    [LowerCase]
    public string Email { get; init; } = null!;

    [Required(AllowEmptyStrings = false)]
    [MinLength(RequestModelsConstraints.PasswordMinLength)]
    [MaxLength(RequestModelsConstraints.PasswordMaxLength)]
    public string Password { get; init; } = null!;

    [Required(AllowEmptyStrings = false)]
    [MaxLength(DboConstraints.NameMaxLength)]
    public string Name { get; init; } = null!;

    [Required(AllowEmptyStrings = false)]
    [MaxLength(DboConstraints.SurnameMaxLength)]
    public string Surname { get; init; } = null!;
}