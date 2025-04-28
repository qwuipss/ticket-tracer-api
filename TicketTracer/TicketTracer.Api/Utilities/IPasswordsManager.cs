namespace TicketTracer.Api.Utilities;

internal interface IPasswordsManager
{
    string Hash(string password, string salt);

    bool Verify(string rawPassword, string passwordHash, string passwordSalt);

    string GenerateSalt();
}