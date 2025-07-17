using System.CommandLine;
using SecureSaver.src.Utils;

class Program
{
    static int Main(string[] args)
    {
        Option<FileInfo> inputPath = new("--input", "-i")
        {
            Required = true,
            Description = "File to Encrypt"
        };

        Option<FileInfo> outputPath = new("--output", "-o")
        {
            Required = true,
            Description = "Path to export the file after the operation."
        };

        Option<int?> iterations = new("--iterations", "-itr")
        {
            Required = false,
            Description = "Number of Salts."
        };


        Option<bool?> verbose = new("--verbose", "-v")
        {
            Required = false,
            Description = "Show more information on the terminal"
        };

        Option<string> operation = new("--operation", "-op")
        {
            Required = true,
            Description = "Decrypt/Encrypt operation."
        };

        Option<bool> overwriteOriginalFile = new("--overwriteOriginalFile", "-oof")
        {
            Required = false,
            Description = "Overwrite the original file to safely erase the original data from device."
        };

        RootCommand rootCommand = new("Your CLI Cryptographer/Decryptographer With AES-256");


        rootCommand.Options.Add(inputPath);
        rootCommand.Options.Add(outputPath);
        rootCommand.Options.Add(iterations);
        rootCommand.Options.Add(verbose);
        rootCommand.Options.Add(operation);
        rootCommand.Options.Add(overwriteOriginalFile);

        CheckArgs.Check(rootCommand.Parse(args));

        return 0;
    }
    
}