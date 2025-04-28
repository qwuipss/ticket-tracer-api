// ReSharper disable InconsistentNaming

using FluentAssertions;
using TicketTracer.Api.Utilities;

namespace TicketTracer.Tests.Api.Services;

[TestFixture]
internal class PasswordHasher_Tests
{
    [SetUp]
    public void SetUp()
    {
        hasher = new PasswordsManager();
    }

    private PasswordsManager hasher = null!;

    [TestCase("password1", "salt1")]
    [TestCase("password2", "salt2")]
    [TestCase("password3", "salt3")]
    public void Should_hash_and_verify_same_passwords(string password, string passwordSalt)
    {
        var passwordHash = hasher.Hash(password, passwordSalt);

        hasher.Verify(password, passwordHash, passwordSalt).Should().BeTrue();
        hasher.Verify("random_password", passwordHash, passwordSalt).Should().BeFalse();
    }
}