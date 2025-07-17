using System.CommandLine;
using System.Security.Cryptography;
using SecureSaver.src.Services;
using SecureSaver.src.Utils;

class Program
{
    static int Main(string[] args)
    {
        var root = new RootCommand("Your CLI Cryptographer/Decryptographer With AES-256")
        {
            new Option<FileInfo>("--input", "-i"){ Required = true, Description = "File to Encrypt" },
            new Option<FileInfo>("--output", "-o"){ Required = true, Description = "Path to export the file after the operation."},
            new Option<string>("--operation", "-op"){ Required = true, Description = "Decrypt/Encrypt operation."},
            new Option<int?>("--iterations", "-itr"){ Required = false, Description = "Number of iterations (leav blank for 500_000)."},
            new Option<bool?>("--verbose", "-v"){ Required = false, Description = "Show more information on the terminal"},
            new Option<bool>("--overwriteOriginalFile", "-oof"){ Required = false, Description = "Overwrite the original file to safely erase the original data from device."}
        };

        try
        {
            var model = ArgValidator.Validate(root.Parse(args));

            if (model.Operation.StartsWith("enc"))
            {
                EncryptionService.Encrypt(model);
            }
            else if (model.Operation.StartsWith("dec"))
            {
                EncryptionService.Decrypt(model);
            }
            else
            {
                Console.Error.WriteLine("Invalid operation. Use 'enc' or 'dec'.");
                Environment.Exit(1);
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


        return 0;
    }
    private static void HandleEx(Exception ex, int code)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine("Error: " + ex.Message);
        Console.ResetColor();
        Environment.Exit(code);
    }
    
}