using System.Diagnostics;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TicketTracer.Api.Configuration.Options;

namespace TicketTracer.Api.Utilities;

internal class Sentry : ISentry
{
    private readonly TelegramBotClient _botClient;
    private readonly ILogger<Sentry> _logger;
    private readonly IOptions<SentryOptions> _options;

    public Sentry(IOptions<SentryOptions> options, ILogger<Sentry> logger)
    {
        _options = options;
        _logger = logger;
        _botClient = new TelegramBotClient(
            new TelegramBotClientOptions(_options.Value.BotToken)
            {
                RetryCount = 3,
            }
        );

        _botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions { DropPendingUpdates = true, AllowedUpdates = [UpdateType.Message,], }
        );
    }

    public async Task ReportAsync(Exception exc)
    {
        if (_options.Value.IsDisabled)
        {
            return;
        }

        _logger.LogInformation("Trying to report error information into Telegram chat");

        var message = $"{Activity.Current!.TraceId}\n\n{exc.Message}";
        await _botClient.SendMessage(_options.Value.ReportChatId, message);

        _logger.LogInformation("Error information successfully reported");
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (_options.Value.IsDisabled)
        {
            return;
        }

        if (update.Message!.Text == "/ping")
        {
            _logger.LogInformation("Ping request message received");
            await botClient.SendMessage(_options.Value.ReportChatId, "pong", cancellationToken: cancellationToken);
        }
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken _)
    {
        if (_options.Value.IsDisabled)
        {
            return Task.CompletedTask;
        }

        _logger.LogError(exception, "An error occured in Telegram bot client:");
        return Task.CompletedTask;
    }
}