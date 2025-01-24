using System;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Crypto
{
    public static class AdvancedHashing
    {
        // Метод хеширования:
        public static string ComputeHash(string input, string salt, string pepper, int iterations = 1000)
        {
            // Проверка на null или пустоту для salt и pepper:
            if (string.IsNullOrEmpty(salt)) throw new ArgumentException("Salt cannot be null or empty");
            if (string.IsNullOrEmpty(pepper)) throw new ArgumentException("Pepper cannot be null or empty");

            // Добавляем salt в начало и конец, затем pepper:
            string saltedInput = salt + input + pepper;

            try
            {
                // Вычисляем SHA-256:
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedInput));

                    // Усилинение хэш с помощью количества итераций:
                    for (int i = 0; i < iterations; i++)
                    {
                        hashBytes = sha256.ComputeHash(hashBytes);
                    }

                    return Convert.ToBase64String(hashBytes);
                }
            }
            catch (Exception ex)
            {
                // Логирование:
                Console.WriteLine($"Ошибка при хешировании: {ex.Message}");
                throw;
            }
        }

        // Метод для генерации случайного pepper:
        public static string GeneratePepper()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] buffer = new byte[32]; // 256 бит
                rng.GetBytes(buffer);
                return Convert.ToBase64String(buffer);
            }
        }
    }
}
