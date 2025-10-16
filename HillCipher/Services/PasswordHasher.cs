using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Security.Cryptography;

namespace HillCipher.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        const int SaltLength = 16;
        const int HashIterations = 100000;
        const int HashLength = 32;

        public string HashPassword(string password)
        {
            byte[] salt = GenerateSalt();
            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: HashIterations,
                numBytesRequested: HashLength
             );

            byte[] hashBytes = new byte[SaltLength + HashLength];
            Array.Copy(salt, 0, hashBytes, 0, SaltLength);
            Array.Copy(hash, 0, hashBytes, SaltLength, HashLength);

            return Convert.ToBase64String(hashBytes);
        }
        
        private byte[] GenerateSalt() => RandomNumberGenerator.GetBytes(SaltLength);

        public bool VerifyPassword(string password, )
    }
}
