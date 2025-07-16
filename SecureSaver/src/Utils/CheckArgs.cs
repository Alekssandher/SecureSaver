using System.CommandLine;
using System.Text;
using SecureSaver.src.Models;

namespace SecureSaver.src.Utils
{
    public static class CheckArgs
    {
        public static int Check(ParseResult parseResult)
        {

            try
            {
                var inputFile = parseResult.GetValue<FileInfo>("--input")
                    ?? throw new ArgumentException("Input is required.");

                var operation = parseResult.GetValue<string>("--operation")
                    ?? throw new ArgumentException("Operation is required: dec(rypt)/enc(rypt)");

                var outputFile = parseResult.GetValue<FileInfo>("--output");
                var saltCount = parseResult.GetValue<int>("--salts");

                if (saltCount == 0) saltCount = 500_000;

                if (saltCount < 100_000)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Warning: {saltCount} is a very low salt iteration count. This may weaken security.");
                    Console.ResetColor();

                    Console.Write("Do you want to continue? (y/N): ");
                    var input = Console.ReadLine()?.Trim().ToLower();

                    if (input != "y" && input != "yes")
                    {
                        Console.WriteLine("Operation canceled.");
                        return 0;
                    }
                }

                var verbose = parseResult.GetValue<bool>("--verbose");

                var model = new Base
                {
                    InputPath = inputFile.FullName,
                    OutputPath = outputFile?.FullName,
                    SaltCount = saltCount,
                    Verbose = verbose,
                    Operation = operation.ToLower()
                };


                if (model.Operation.StartsWith("enc"))
                {
                    byte[] result = Encrypt.EncryptData(
                        model.InputPath,
                        ReadPassword(),
                        model.SaltCount

                        );
                        
                    return 0;
                }
                else if (model.Operation.StartsWith("dec"))
                {
                    return 0;
                }
                else
                {
                    return 1;
                }

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine("Error: " + ex.Message);
                Console.ResetColor();
                return 1;
            }
        }
        public static string ReadPassword(string prompt = "Enter password: ")
        {
            Console.Write(prompt);
            var password = new StringBuilder();
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(intercept: true); // intercepta para não exibir

                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Length--;
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    password.Append(key.KeyChar);
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine(); // quebra linha após ENTER
            return password.ToString();
        }
    }
}