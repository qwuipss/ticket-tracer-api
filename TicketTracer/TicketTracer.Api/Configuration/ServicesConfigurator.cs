using TicketTracer.Api.Services;

namespace TicketTracer.Api.Configuration;

internal static class ServicesConfigurator
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
    }
}