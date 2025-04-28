using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TicketTracer.Data.Entities.Abstract;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace TicketTracer.Data.Entities;

[Table("users")]
[Index(nameof(Email), IsUnique = true)]
public class UserEntity : BaseEntity
{
    [Column("email")]
    [MaxLength(EntityConstraints.User.EmailMaxLength)]
    public required string Email { get; set; }

    [Column("password_hash")]
    [MaxLength(64)]
    public required string PasswordHash { get; set; }

    [Column("password_salt")]
    [MaxLength(32)]
    public required string PasswordSalt { get; set; }

    [Column("name")]
    [MaxLength(EntityConstraints.User.NameMaxLength)]
    public required string Name { get; set; }

    [Column("surname")]
    [MaxLength(EntityConstraints.User.SurnameMaxLength)]
    public required string Surname { get; set; }
}