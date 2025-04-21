using System.ComponentModel.DataAnnotations;

namespace TicketTracer.Api.Configuration.Options;

public class SentryOptions
{
    public const string SectionName = "Sentry";

    [Required]
    public string BotToken { get; init; } = null!;

    [Required]
    public long ReportChatId { get; init; }

    [Required]
    public bool IsDisabled { get; init; }
}