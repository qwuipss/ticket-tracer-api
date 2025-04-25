using System.ComponentModel.DataAnnotations;

namespace TicketTracer.Api.Configuration.Options;

internal class SentryOptions
{
    public const string SectionName = "Sentry";

    [Required(AllowEmptyStrings = false)]
    public string BotToken { get; init; } = null!;

    [Required]
    public long ReportChatId { get; init; }

    [Required]
    public bool IsDisabled { get; init; }
}