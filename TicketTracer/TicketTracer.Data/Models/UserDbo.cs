using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TicketTracer.Data.Models.Abstract;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace TicketTracer.Data.Models;

[Table("users")]
[Index(nameof(Email), IsUnique = true)]
public class UserDbo : BaseDbo
{
    [MaxLength(DboConstraints.EmailMaxLength)]
    public string Email { get; set; } = null!;

    [MaxLength(64)]
    public string PasswordHash { get; set; } = null!;

    [MaxLength(32)]
    public string PasswordSalt { get; set; } = null!;

    [MaxLength(DboConstraints.NameMaxLength)]
    public string Name { get; set; } = null!;

    [MaxLength(DboConstraints.SurnameMaxLength)]
    public string Surname { get; set; } = null!;
}