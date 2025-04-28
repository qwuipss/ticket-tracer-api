using System.Security.Claims;

namespace TicketTracer.Api.Extensions;

internal static class ClaimsPrincipalExtensions
{
    public static Guid GetIdOrThrow(this ClaimsPrincipal principal)
    {
        if (Guid.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), out var id))
        {
            return id;
        }

        throw new ArgumentNullException(nameof(principal), "Required claim 'NameIdentifier' was not found in principal");
    }

    public static string? GetEmail(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.Email);
    }
}