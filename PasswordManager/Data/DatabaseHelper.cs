using System;
using System.Data.SQLite;

namespace PasswordManager.Data
{
    public static class DatabaseHelper
    {
        private static string connectionString = "Data Source=PasswordManager.db;Version=3;";

        public static void InitializeDatabase()
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Passwords (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Title TEXT NOT NULL,
                            Username TEXT NOT NULL,
                            EncryptedPassword TEXT NOT NULL
                        );";
                    using (var command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Логирование: 
                Console.WriteLine($"Ошибка при инициализации базы данных: {ex.Message}");
            }
        }

        public static void SavePassword(string title, string username, string encryptedPassword)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string insertQuery = @"
                        INSERT INTO Passwords (Title, Username, EncryptedPassword) 
                        VALUES (@Title, @Username, @EncryptedPassword);";
                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Title", title);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@EncryptedPassword", encryptedPassword);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Логирование:
                Console.WriteLine($"Ошибка при сохранении пароля: {ex.Message}");
            }
        }
    }
}
