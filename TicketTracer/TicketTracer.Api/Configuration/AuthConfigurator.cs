using Microsoft.AspNetCore.Authentication.Cookies;

namespace TicketTracer.Api.Configuration;

internal static class AuthConfigurator
{
    public static void AddAuth(this IServiceCollection services)
    {
        services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(
                options =>
                {
                    options.Cookie.Name = "ticket_tracer_auth";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.SlidingExpiration = true;
                }
            );
    }
}