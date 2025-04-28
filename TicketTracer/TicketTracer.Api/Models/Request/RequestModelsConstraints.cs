namespace TicketTracer.Api.Models.Request;

internal static class RequestModelsConstraints
{
    public const int ItemsPerPageLimit = 50;

    public static class User
    {
        public const int PasswordMinLength = 8;

        public const int PasswordMaxLength = 32;
    }
}