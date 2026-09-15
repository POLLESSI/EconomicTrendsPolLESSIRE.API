using EconomicTrendsPolLESSIRE.Domain.Entities;
using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Security
{
    public sealed class Argon2PasswordHasher : IPasswordHasher<Users>
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;

        private const int Iterations = 3;
        private const int MemorySizeKb = 64 * 1024; // 64 MB
        private const int Parallelism = 2;

        public string HashPassword(
            Users user,
            string password)
        {
            ArgumentNullException.ThrowIfNull(user);

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException(
                    "Password cannot be empty.",
                    nameof(password));

            byte[] salt =
                RandomNumberGenerator.GetBytes(SaltSize);

            byte[] hash =
                DeriveHash(password, salt);

            return string.Join(
                '$',
                "argon2id",
                "v1",
                Convert.ToBase64String(salt),
                Convert.ToBase64String(hash));
        }

        public PasswordVerificationResult VerifyHashedPassword(
            Users user,
            string hashedPassword,
            string providedPassword)
        {
            ArgumentNullException.ThrowIfNull(user);

            if (string.IsNullOrWhiteSpace(hashedPassword) ||
                string.IsNullOrWhiteSpace(providedPassword))
            {
                return PasswordVerificationResult.Failed;
            }

            try
            {
                string[] parts =
                    hashedPassword.Split('$');

                if (parts.Length != 4)
                    return PasswordVerificationResult.Failed;

                if (!string.Equals(
                        parts[0],
                        "argon2id",
                        StringComparison.Ordinal))
                {
                    return PasswordVerificationResult.Failed;
                }

                if (!string.Equals(
                        parts[1],
                        "v1",
                        StringComparison.Ordinal))
                {
                    return PasswordVerificationResult.Failed;
                }

                byte[] salt =
                    Convert.FromBase64String(parts[2]);

                byte[] expectedHash =
                    Convert.FromBase64String(parts[3]);

                byte[] actualHash =
                    DeriveHash(
                        providedPassword,
                        salt,
                        expectedHash.Length);

                bool valid =
                    CryptographicOperations.FixedTimeEquals(
                        expectedHash,
                        actualHash);

                return valid
                    ? PasswordVerificationResult.Success
                    : PasswordVerificationResult.Failed;
            }
            catch (
                FormatException)
            {
                return PasswordVerificationResult.Failed;
            }
        }

        private static byte[] DeriveHash(
            string password,
            byte[] salt,
            int hashSize = HashSize)
        {
            var argon2 =
                new Argon2id(
                    Encoding.UTF8.GetBytes(password))
                {
                    Salt = salt,
                    Iterations = Iterations,
                    MemorySize = MemorySizeKb,
                    DegreeOfParallelism = Parallelism
                };

            return argon2.GetBytes(hashSize);
        }
    }
}