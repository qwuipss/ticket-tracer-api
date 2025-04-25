namespace TicketTracer.Api.Configuration;

internal static class CorsConfigurator
{
    public static void AddCors(this IServiceCollection services)
    {
        // services.AddCors(
        //     options =>
        //     {
        //         options.AddPolicy(
        //             "FrontendClient",
        //             policy =>
        //             {
        //                 policy
        //                     .AllowAnyOrigin()
        //                     .AllowAnyHeader()
        //                     .AllowAnyMethod();
        //             }
        //         );
        //     }
        // );
    }
}