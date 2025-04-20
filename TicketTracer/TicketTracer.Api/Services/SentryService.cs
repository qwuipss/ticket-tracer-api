using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TicketTracer.Api.Services;

public class SentryService : ISentryService
{
    private readonly TelegramBotClient _botClient;
    private readonly long _chatId;
    private readonly ILogger<SentryService> _logger;

    public SentryService(IConfiguration config, ILogger<SentryService> logger)
    {
        var token = config.GetValue<string>("TelegramBotToken");
        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentException("Telegram bot token is missing");
        }

        _logger = logger;
        _chatId = config.GetValue<long>("TelegramReportChatId");
        _botClient = new TelegramBotClient(
            new TelegramBotClientOptions(token)
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
        _logger.LogInformation("Trying to report error information into Telegram chat");
        
        var message = $"{Activity.Current!.TraceId}\n\n{exc.Message}";
        await _botClient.SendMessage(chatId: _chatId, text: message);
        
        _logger.LogInformation("Error information successfully reported");
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message!.Text == "/ping")
        {
            _logger.LogInformation("Ping request message received");
            await botClient.SendMessage(chatId: _chatId, text: "pong", cancellationToken: cancellationToken);
        }
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken _)
    {
        _logger.LogError(exception, "An error occured in Telegram bot client:");
        return Task.CompletedTask;
    }
}