using Microsoft.AspNetCore.Authentication.Cookies;

namespace TicketTracer.Api.Configuration;

internal static class AuthConfigurator
{
    public static void AddAuth(this IServiceCollection services)
    {
        services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
                {
                    options.Cookie.Name = "ticket_tracer_auth";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.MaxAge = TimeSpan.FromDays(7);
                    options.ExpireTimeSpan = TimeSpan.FromDays(7);
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return Task.CompletedTask;
                        },
                        OnRedirectToAccessDenied = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            return Task.CompletedTask;
                        },
                    };
                }
            );
    }
}