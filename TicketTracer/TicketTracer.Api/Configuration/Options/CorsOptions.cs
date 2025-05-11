using System.ComponentModel.DataAnnotations;

namespace TicketTracer.Api.Configuration.Options;

internal class CorsOptions
{
    public const string SectionName = "Cors";

    [Required]
    public string[] AllowedOrigins { get; init; } = null!;
}