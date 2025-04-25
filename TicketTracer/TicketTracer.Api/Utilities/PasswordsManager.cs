using System.Security.Cryptography;
using System.Text;

namespace TicketTracer.Api.Utilities;

internal class PasswordsManager : IPasswordsManager
{
    public string Hash(string password, string salt)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password + salt));
        var hashString = Convert.ToBase64String(hashBytes);

        return hashString;
    }

    public bool Verify(string rawPassword, string passwordHash, string passwordSalt)
    {
        return Hash(rawPassword, passwordSalt) == passwordHash;
    }

    public string GenerateSalt()
    {
        var salt = new byte[16];
        RandomNumberGenerator.Create().GetBytes(salt);
        return Convert.ToBase64String(salt);
    }
}