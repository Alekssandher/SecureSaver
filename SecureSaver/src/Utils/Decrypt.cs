using System.Security.Cryptography;
using System.Text;

namespace SecureSaver.src.Utils
{
    public class Decrypt
    {
        public static string DecryptData(byte[] encryptedData, string password, int iterations)
        {
            using Aes aesAlg = Aes.Create();

            using var ms = new MemoryStream(encryptedData);
            
            byte[] salt = new byte[16];
            ms.Read(salt, 0, salt.Length);

            byte[] iv = new byte[16];
            ms.Read(iv, 0, iv.Length);

            using var keyDerivation = new Rfc2898DeriveBytes(
                password: password,
                salt: salt,
                iterations: iterations,
                hashAlgorithm: HashAlgorithmName.SHA256
            );

            aesAlg.Key = keyDerivation.GetBytes(32);
            aesAlg.IV = iv;

            using var cs = new CryptoStream(ms, aesAlg.CreateDecryptor(), CryptoStreamMode.Read);
            using var sr = new StreamReader(cs, Encoding.UTF8);
            return sr.ReadToEnd();
        }
    }
}