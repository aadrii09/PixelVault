using System.Globalization;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ApiVault.Utilidades
{
    public class PasswordHashed
    {
        public static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] salt = new byte[128/ 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256, //este es el hash mas popular
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));

                return $"{Convert.ToBase64String(salt)}.{hashed}";
            }
        }

        public static bool VerifyPassword(string password, string storeHash)
        {
            var parts = storeHash.Split('.');
            if (parts.Length != 2)
                return false;

            var salt = Convert.FromBase64String(parts[0]);
            var hash = parts[1];

            var attemptedHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return attemptedHash == hash;
        }
    }
}
