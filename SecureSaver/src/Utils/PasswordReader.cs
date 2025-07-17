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

        public static string ReadPasswordTwice(string prompt1 = "Enter password: ", string prompt2 = "Confirm password: ")
        {
            while (true)
            {
                Console.Write(prompt1);
                var password1 = ReadPassword("");

                Console.Write(prompt2);
                var password2 = ReadPassword("");

                if (password1 == password2)
                    return password1;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Passwords do not match. Please try again.\n");
                Console.ResetColor();
            }
        }
    }
}