using System.Security.Cryptography;
using System.Text;
using WebApi.Service.Interfaces;

namespace WebApi.Service.Implementations
{
    public class Cryptography : ICryptography
    {
        private readonly ICryptoTransform encryptor;
        private readonly ICryptoTransform decryptor;

        public Cryptography()
        {
            Aes aes = Aes.Create();

            aes.Key = File.ReadAllBytes("Keys/keys.txt");
            aes.IV = File.ReadAllBytes("Keys/IV.txt");

            encryptor = aes.CreateEncryptor();
            decryptor = aes.CreateDecryptor();
        }

        public string DecryptPassword(string cryptedPassword)
        {
            byte[] passByte = Convert.FromBase64String(cryptedPassword);
            byte[] decryptedData = decryptor.TransformFinalBlock(passByte, 0, passByte.Length);

            return Encoding.Default.GetString(decryptedData);
        }

        public string EncryptPassword(string password)
        {
            byte[] cryptPassword = Encoding.Default.GetBytes(password);
            byte[] encryptedPass = encryptor.TransformFinalBlock(cryptPassword, 0, cryptPassword.Length);

            return Convert.ToBase64String(encryptedPass, 0, encryptedPass.Length);
        }

        public string GetPasswordHash(string password)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));

            return Convert.ToBase64String(hash);
        }
    }
}
