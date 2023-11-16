using System.Security.Cryptography;
using System.Text;
using WebApi.Service.Interfaces;
using WebApi.Domain.Extensions;
using WebApi.Domain.Entity;

namespace WebApi.Service.Implementations
{
    public class Cryptography : ICryptography
    {
        private readonly Aes aes;

        public Cryptography()
        {
            aes = Aes.Create();
            aes.Key = File.ReadAllBytes("Keys/keys.txt");
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
        }

        public string DecryptPassword(string cryptedPassword, long userId)
        {
            byte[] passByte = Convert.FromBase64String(cryptedPassword);
            ICryptoTransform decryptor = GetDecryptor(userId);
            byte[] decryptedData = decryptor.TransformFinalBlock(passByte, 0, passByte.Length);

            return Encoding.Default.GetString(decryptedData);
        }

        public string EncryptPassword(string password, long userId)
        {
            byte[] cryptPassword = Encoding.Default.GetBytes(password);
            ICryptoTransform encryptor = GetEncryptor(userId);
            byte[] encryptedPass = encryptor.TransformFinalBlock(cryptPassword, 0, cryptPassword.Length);

            return Convert.ToBase64String(encryptedPass, 0, encryptedPass.Length);
        }

        public string GetPasswordHash(string password)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));

            return Convert.ToBase64String(hash);
        }

        private void SetIV(long vector)
        {
            byte[] iv = vector.ToBigEndianBytes();
            iv = new byte[8].Concat(iv).ToArray();

            aes.IV = iv;
        }

        private ICryptoTransform GetEncryptor(long vector)
        {
            SetIV(vector);

            return aes.CreateEncryptor();
        }

        private ICryptoTransform GetDecryptor(long vector)
        {
            SetIV(vector);

            return aes.CreateDecryptor();
        }
    }
}
