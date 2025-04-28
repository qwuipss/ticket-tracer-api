using TicketTracer.Api.Services;

namespace TicketTracer.Api.Configuration;

internal static class ServicesConfigurator
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IProjectsService, ProjectsService>();
        services.AddScoped<IBoardsService, BoardsService>();
        services.AddScoped<ITicketsService, TicketsService>();
        services.AddScoped<IAccountsService, AccountsService>();
    }
}