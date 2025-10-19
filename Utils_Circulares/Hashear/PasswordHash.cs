using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utils_Circulares.Hashear
{
    public static class PasswordHash
    {
        // Parámetros recomendados (ajústalos a tu política)
        private const int Iteraciones = 150_000;
        private const int SaltSize = 16;
        private const int KeySize = 32;
        /// <summary>
        /// Devuelve un hash portable en formato:
        /// PBKDF2$HMACSHA256$<iteraciones>$<salt_base64>$<hash_base64>
        /// </summary>
        public static string Hash(string plainPassword)
        {
            if (string.IsNullOrEmpty(plainPassword))
                throw new ArgumentException("La contraseña no puede ser nula o vacía.", nameof(plainPassword));

            // Generar salt aleatoria (128 bits)
            byte[] salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);

            // Derivar clave con PBKDF2-HMACSHA256
            byte[] key = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(plainPassword),
                salt,
                Iteraciones,
                HashAlgorithmName.SHA256,
                KeySize 
            );

            return $"PBKDF2$HMACSHA256${Iteraciones}${Convert.ToBase64String(salt)}${Convert.ToBase64String(key)}";
        }

        /// <summary>
        /// Verifica una contraseña en texto plano contra un hash generado por Hash().
        /// </summary>
        public static bool Verify(string plainPassword, string storedHash)
        {
            if (string.IsNullOrEmpty(plainPassword) || string.IsNullOrWhiteSpace(storedHash))
                return false;

            var parts = storedHash.Split('$', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 5 || parts[0] != "PBKDF2" || parts[1] != "HMACSHA256")
                return false;

            if (!int.TryParse(parts[2], out int iters) || iters <= 0)
                return false;

            byte[] salt, key;
            try
            {
                salt = Convert.FromBase64String(parts[3]);
                key = Convert.FromBase64String(parts[4]);
            }
            catch
            {
                return false;
            }

            byte[] computed = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(plainPassword),
                salt,
                iters,
                HashAlgorithmName.SHA256,
                key.Length
            );

            return CryptographicOperations.FixedTimeEquals(computed, key);
        }


    }
}
