
using System.CommandLine;
using SecureSaver.src.Models;

namespace SecureSaver.src.Utils
{
    public static class ArgValidator
    {
        public static Base Validate(ParseResult result)
        {
            var input = result.GetValue<FileInfo>("--input")
                ?? throw new ArgumentException("Input is required.");

            var output = result.GetValue<FileInfo>("--output")
                ?? throw new ArgumentException("Output is required.");

            var operation = result.GetValue<string>("--operation")?.ToLower()
                ?? throw new ArgumentException("Operation is required.");

            int iterations = result.GetValue<int>("--iterations");
            
            if (iterations == 0) iterations = 500_000;

            if (iterations < 100_000)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Warning: {iterations} is very low.");
                Console.ResetColor();
                Console.Write("Continue? (y/N): ");
                var response = Console.ReadLine()?.Trim().ToLower();
                if (response != "y" && response != "yes") Environment.Exit(3);
            }

            if (!input.Exists)
                throw new FileNotFoundException($"Input file not found: {input}");

            var dir = output.Directory ?? throw new DirectoryNotFoundException($"Invalid output path: {output}");
            if (!dir.Exists) dir.Create();

            return new Base
            {
                InputPath = input.FullName,
                OutputPath = output.FullName,
                Operation = operation,
                Iterations = iterations,
                Verbose = result.GetValue<bool>("--verbose"),
                Overwrite = result.GetValue<bool>("--overwriteOriginalFile")
            };
        }
    }
}