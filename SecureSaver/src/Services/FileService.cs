using System.Security.Cryptography;

namespace SecureSaver.src.Services
{
    public static class FileService
    {
        public static void SecureDelete(string path)
        {
            if (!File.Exists(path)) return;

            long size = new FileInfo(path).Length;
            byte[] randomData = new byte[size];

            RandomNumberGenerator.Fill(randomData);

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Write))
            {
                fs.Write(randomData, 0, randomData.Length);
            }

            File.Delete(path);
        }
    }
}