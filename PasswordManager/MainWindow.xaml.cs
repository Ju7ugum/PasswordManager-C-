using System;
using System.Windows;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager
{
    public partial class MainWindow : Window
    {
        private const string DatabaseFile = "passwords.db";

        public MainWindow()
        {
            InitializeComponent();
        }

        // Метод для генерации ключа:
        private void GenerateKeyButton_Click(object sender, RoutedEventArgs e)
        {
            byte[] key = GenerateKey();
            string keyHex = BitConverter.ToString(key).Replace("-", "");
            GeneratedKeyTextBox.Text = keyHex; // Отображаем ключ в TextBox
        }

        // Метод генерации ключа:
        private byte[] GenerateKey()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[32]; // 256-битный ключ
                rng.GetBytes(key);
                return key;
            }
        }

        // Вспомогательный метод для генерации пароля:
        private void GeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            int length = int.TryParse(PasswordLength.Text, out int result) ? result : 12;
            bool includeUppercase = IncludeUppercase.IsChecked ?? false;
            bool includeLowercase = IncludeLowercase.IsChecked ?? false;
            bool includeNumbers = IncludeNumbers.IsChecked ?? false;
            bool includeSymbols = IncludeSymbols.IsChecked ?? false;

            string password = GeneratePassword(length, includeUppercase, includeLowercase, includeNumbers, includeSymbols);
            GeneratedPasswordTextBox.Text = password; // Отображаем пароль в TextBox
        }

        private string GeneratePassword(int length, bool includeUppercase, bool includeLowercase, bool includeNumbers, bool includeSymbols)
        {
            const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercase = "abcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";
            const string symbols = "!@#$%^&()";

            StringBuilder characterPool = new StringBuilder();

            if (includeUppercase) characterPool.Append(uppercase);
            if (includeLowercase) characterPool.Append(lowercase);
            if (includeNumbers) characterPool.Append(numbers);
            if (includeSymbols) characterPool.Append(symbols);

            if (characterPool.Length == 0)
                return "Выберите хотя бы один тип символов.";

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                StringBuilder password = new StringBuilder();

                for (int i = 0; i < length; i++)
                {
                    int index = randomBytes[i] % characterPool.Length;
                    password.Append(characterPool[index]);
                }

                return password.ToString();
            }
        }

        private void SaveRecord_Click(object sender, RoutedEventArgs e)
        {
            string title = RecordTitle.Text;
            string username = RecordUsername.Text;
            string password = GeneratedPasswordTextBox.Text;
            string key = GeneratedKeyTextBox.Text;

            MessageBox.Show($"Запись сохранена:\nНазвание: {title}\nПользователь: {username}\nПароль: {password}\nКлюч: {key}",
                            "Запись сохранена", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
