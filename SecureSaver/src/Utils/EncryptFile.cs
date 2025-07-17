using System.Security.Cryptography;

namespace SecureSaver.src.Utils
{
    public class EncryptFile
    {
        public static byte[] EncryptData(byte[] data, string password, int iterations)
        {
            using Aes aesAlg = Aes.Create();

            byte[] salt = new byte[16];
            RandomNumberGenerator.Fill(salt);

            using var keyDerivation = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);

            aesAlg.Key = keyDerivation.GetBytes(32);

            aesAlg.IV = new byte[16];
            RandomNumberGenerator.Fill(aesAlg.IV);

            using var ms = new MemoryStream();
            ms.Write(salt, 0, salt.Length);
            ms.Write(aesAlg.IV, 0, aesAlg.IV.Length);

            using (var cs = new CryptoStream(ms, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
            }

            return ms.ToArray();
        }
    }
}