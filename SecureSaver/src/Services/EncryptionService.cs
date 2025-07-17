using SecureSaver.src.Models;
using SecureSaver.src.Utils;

namespace SecureSaver.src.Services
{
    public static class EncryptionService
    {
        public static void Encrypt(Base model)
        {
            byte[] data = File.ReadAllBytes(model.InputPath);
            byte[] encrypted = EncryptFile.EncryptData(data, PasswordReader.ReadPasswordTwice(), model.Iterations);

            if (model.Overwrite)
                FileService.SecureDelete(model.InputPath);

            File.WriteAllBytes(model.OutputPath, encrypted);
            Console.WriteLine("File Encrypted and Saved.");
        }

        public static void Decrypt(Base model)
        {
            byte[] encryptedData = File.ReadAllBytes(model.InputPath);
            byte[] decrypted = DecryptFile.DecryptData(encryptedData, PasswordReader.ReadPassword(), model.Iterations);
            File.WriteAllBytes(model.OutputPath, decrypted);
            Console.WriteLine("File Decrypted and Saved.");
        }
    }

}