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

        Option<FileInfo?> outputPath = new("--output", "-o")
        {
            Required = false,
            Description = "Path to export the file after the operation."
        };

        Option<int?> saltCount = new("--salts", "-sal") 
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

        RootCommand rootCommand = new("Your CLI Cryptographer/Decryptographer With AES-256");


        rootCommand.Options.Add(inputPath);
        rootCommand.Options.Add(outputPath);
        rootCommand.Options.Add(saltCount);
        rootCommand.Options.Add(verbose);
        rootCommand.Options.Add(operation);

        return CheckArgs.Check(rootCommand.Parse(args));        
        
    }
    
}