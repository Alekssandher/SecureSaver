using System.CommandLine;
using System.Security.Cryptography;
using System.Text;
using SecureSaver.src.Models;

namespace SecureSaver.src.Utils
{
    public static class CheckArgs
    {
        public static void Check(ParseResult parseResult)
        {

            try
            {
                var inputFile = parseResult.GetValue<FileInfo>("--input")
                    ?? throw new ArgumentException("InputFile is required.");

                var operation = parseResult.GetValue<string>("--operation")
                    ?? throw new ArgumentException("Operation is required: dec(rypt)/enc(rypt)");

                var outputFile = parseResult.GetValue<FileInfo>("--output")
                    ?? throw new ArgumentException("OutputPath is required.");

                var iterations = parseResult.GetValue<int>("--iterations");

                if (iterations == 0) iterations = 500_000;

                if (operation.StartsWith("enc") && outputFile == null)
                {
                    throw new ArgumentException("For this kind of operation (Encrypt) you need to pass an output path");
                }

                CheckPaths(inputFile, outputFile);

                if (iterations < 100_000)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Warning: {iterations} is a very low iteration count. This may weaken security.");
                    Console.ResetColor();

                    Console.Write("Do you want to continue? (y/N): ");
                    var input = Console.ReadLine()?.Trim().ToLower();

                    if (input != "y" && input != "yes")
                    {
                        Console.WriteLine("Operation canceled.");
                        Environment.Exit(3);
                    }
                }

                var verbose = parseResult.GetValue<bool>("--verbose");

                var overwrite = parseResult.GetValue<bool>("--overwriteOriginalFile");

                var model = new Base
                {
                    InputPath = inputFile.FullName,
                    OutputPath = outputFile.FullName,
                    Iterations = iterations,
                    Verbose = verbose,
                    Operation = operation.ToLower(),
                    Overwrite = overwrite
                };


                if (model.Operation.StartsWith("enc"))
                {
                    var data = File.ReadAllText(model.InputPath);
                    byte[] result = Encrypt.EncryptData(
                        data,
                        ReadPassword(),
                        model.Iterations

                    );

                    if (model.Overwrite)
                    {
                        SafeDeleteFile(model.InputPath);

                        Console.WriteLine("File Overwritten Successfully.\n");
                    }

                    File.WriteAllBytes(model.OutputPath, result);

                    Console.WriteLine("File Encrypted and Saved Successfully.");

                    Environment.Exit(0);
                }
                else if (model.Operation.StartsWith("dec"))
                {

                    byte[] encryptedFile = File.ReadAllBytes(model.InputPath);

                    Console.WriteLine($"InputPath: {model.InputPath}");
                    Console.WriteLine($"OutputPath: {model.OutputPath}");
                    Console.WriteLine($"Encrypted bytes count: {encryptedFile.Length}");

                    string result = Decrypt.DecryptData(
                        encryptedFile,
                        ReadPassword(),
                        iterations
                    );

                    File.WriteAllText(model.OutputPath, result);

                    Console.WriteLine("File Decrypted and Saved Successfully.");

                    Environment.Exit(0);
                }


                Environment.Exit(0);
            }
            catch (DirectoryNotFoundException ex)
            {
                HandleEx(ex, 73);
            }
            catch (FileNotFoundException ex)
            {
                HandleEx(ex, 66);
            }
            catch (UnauthorizedAccessException ex)
            {
                HandleEx(ex, 77);
            }
            catch (CryptographicException ex)
            {
                HandleEx(ex, 1);
            }
            catch (Exception ex)
            {
                HandleEx(ex, 1);
            }
        }
        private static void HandleEx(Exception ex, int code)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("Error: " + ex.Message);
            Console.ResetColor();
            Environment.Exit(code);
        }
        public static void CheckPaths(FileInfo _inputFile, FileInfo _outputFile)
        {
            if (!_inputFile.Exists)
            {
                throw new FileNotFoundException($"InputFile not found at: {_inputFile}");
            }

            var outputDirectory = _outputFile.Directory
                ?? throw new DirectoryNotFoundException($"Output path is invalid: {_outputFile.DirectoryName}");

            if (!outputDirectory.Exists)
            {
                outputDirectory.Create();
            }
            return;
        }
        public static string ReadPassword(string prompt = "Enter password: ")
        {
            Console.Write(prompt);
            var password = new StringBuilder();
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Length--;
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    password.Append(key.KeyChar);
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.Clear();

            return password.ToString();
        }
        private static void SafeDeleteFile(string filePath)
        {

            long fileSize = new FileInfo(filePath).Length;

            byte[] randomData = new byte[fileSize];
            Random random = new();
            random.NextBytes(randomData);

            using (FileStream fs = new(filePath, FileMode.Open, FileAccess.Write))
            {
                fs.Write(randomData, 0, randomData.Length);
            }

            File.Delete(filePath);

            return;
	    }

    }
    
}