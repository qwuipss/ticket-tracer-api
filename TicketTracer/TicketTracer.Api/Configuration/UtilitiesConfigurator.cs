using TicketTracer.Api.Utilities;

namespace TicketTracer.Api.Configuration;

internal static class UtilitiesConfigurator
{
    public static void AddUtilities(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordsManager, PasswordsManager>();
    }
}