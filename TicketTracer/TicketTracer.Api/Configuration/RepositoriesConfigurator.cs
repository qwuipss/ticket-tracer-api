using TicketTracer.Api.Repositories;

namespace TicketTracer.Api.Configuration;

internal static class RepositoriesConfigurator
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUsersRepository, UsersRepository>();
    }
}