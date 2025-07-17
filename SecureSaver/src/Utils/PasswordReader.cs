using System.Text;

namespace SecureSaver.src.Utils
{
    public static class PasswordReader
    {
        public static string ReadPassword(string prompt = "Enter password: ")
        {
            Console.Write(prompt);
            var password = new StringBuilder();
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    password.Length--;
                else if (!char.IsControl(key.KeyChar))
                    password.Append(key.KeyChar);
            }
            while (key.Key != ConsoleKey.Enter);

            Console.Clear();
            return password.ToString();
        }
    }
}