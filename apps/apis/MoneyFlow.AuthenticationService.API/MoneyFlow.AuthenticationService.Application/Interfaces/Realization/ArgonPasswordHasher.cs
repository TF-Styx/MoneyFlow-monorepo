using Konscious.Security.Cryptography;
using MoneyFlow.AuthenticationService.Application.Enums;
using MoneyFlow.AuthenticationService.Application.Interfaces.Abstraction;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace MoneyFlow.AuthenticationService.Application.Interfaces.Realization
{
    public class ArgonPasswordHasher : IPasswordHasher
    {
        private const int ArgonDegreeOfParallelism = 2;
        private const int ArgonIterations = 3;
        private const int ArgonMemorySizeKb = 65536;

        private const int HashSize = 32;
        private const int SaltSize = 16;

        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) 
                throw new ArgumentNullException(password);

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            var argon2 = new Argon2id(passwordBytes)
            {
                Salt = salt,
                DegreeOfParallelism = ArgonDegreeOfParallelism,
                Iterations = ArgonIterations,
                MemorySize = ArgonMemorySizeKb,
            };

            byte[] hash = argon2.GetBytes(HashSize);

            string hashBase64 = Convert.ToBase64String(hash);
            string saltBase64 = Convert.ToBase64String(salt);

            return $"{Hashers.Argon2id}:{ArgonDegreeOfParallelism}:{ArgonIterations}:{ArgonMemorySizeKb}:{saltBase64}:{hashBase64}";
        }


        public bool VerifyPassword(string password, string storedHashString)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(password);
            if (string.IsNullOrEmpty(storedHashString))
                throw new ArgumentNullException(storedHashString);

            string[] parts = storedHashString.Split(':');
            if (parts.Length != 6 || parts[0] != Hashers.Argon2id.ToString())
            {
                Debug.WriteLine($"Неверный формат для алгоритма {Hashers.Argon2id}");
                return false;
            }

            byte[] salt;
            byte[] storedHash;
            int degreeOfParallelism;
            int iterations;
            int memorySizeKb;

            try
            {
                if (!int.TryParse(parts[1], out degreeOfParallelism) ||
                    !int.TryParse(parts[2], out iterations) ||
                    !int.TryParse(parts[3], out memorySizeKb))
                {
                    Debug.WriteLine($"Не верные параметры для {Hashers.Argon2id}");
                    return false;
                }

                salt = Convert.FromBase64String(parts[4]);
                storedHash = Convert.FromBase64String(parts[5]);
            }
            catch (FormatException ex)
            {
                Debug.WriteLine($"Ошибка парсинга сохраненного хеша! {ex.Message}!");
                return false;
            }

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            var argon2 = new Argon2id(passwordBytes)
            {
                Salt = salt,
                DegreeOfParallelism = degreeOfParallelism,
                Iterations = iterations,
                MemorySize = memorySizeKb,
            };

            byte[] testHash = argon2.GetBytes(storedHash.Length);

            return CryptographicOperations.FixedTimeEquals(testHash, storedHash);
        }
    }
}
