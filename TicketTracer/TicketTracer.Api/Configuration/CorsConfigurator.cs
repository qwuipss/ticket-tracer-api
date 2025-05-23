using TicketTracer.Api.Configuration.Options;

namespace TicketTracer.Api.Configuration;

internal static class CorsConfigurator
{
    public static void AddCors(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptionsWithValidateOnStart<CorsOptions>()
            .Bind(configuration.GetSection(CorsOptions.SectionName))
            .ValidateDataAnnotations();

        var corsOptions = configuration.GetSection(CorsOptions.SectionName).Get<CorsOptions>();

        services.AddCors(options =>
            {
                options
                    .AddDefaultPolicy(policy =>
                        {
                            policy
                                .WithOrigins(corsOptions!.AllowedOrigins)
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                        }
                    );
            }
        );
    }
}