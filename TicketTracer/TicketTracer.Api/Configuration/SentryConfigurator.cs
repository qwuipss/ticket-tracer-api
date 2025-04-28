using TicketTracer.Api.Configuration.Options;
using TicketTracer.Api.Utilities;

namespace TicketTracer.Api.Configuration;

internal static class SentryConfigurator
{
    public static void AddSentry(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptionsWithValidateOnStart<SentryOptions>()
            .Bind(configuration.GetSection(SentryOptions.SectionName))
            .ValidateDataAnnotations();

        services.AddSingleton<ISentry, Sentry>();
    }
}