using TicketTracer.Api.Repositories;

namespace TicketTracer.Api.Configuration;

internal static class RepositoriesConfigurator
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProjectsRepository, ProjectsRepository>();
        services.AddScoped<IBoardsRepository, BoardsRepository>();
        services.AddScoped<ITicketsRepository, TicketsRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
    }
}