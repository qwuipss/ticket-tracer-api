using TicketTracer.Api.Configuration.Options;
using TicketTracer.Api.Services;

namespace TicketTracer.Api.Configuration;

public static class SentryConfigurator
{
    public static void AddSentry(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ISentryService, SentryService>();
        services
            .AddOptions<SentryOptions>()
            .Bind(configuration.GetSection(SentryOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}