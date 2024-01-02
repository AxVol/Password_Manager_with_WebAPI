using System.Text;
using WebApi.Service.AlgoRU.KuznechikCypher;
using WebApi.Service.AlgoRU.StribogHash;

namespace WebApi.Service.AlgoRU
{
    public class Usage
    {
        private readonly Stribog stribog;
        private readonly Kuznechik kuznechik;

        public Usage()
        {
            stribog = new Stribog(LengthHash.Length512);
            byte[] key = File.ReadAllBytes("Keys/keys.txt");

            kuznechik = new Kuznechik();
            kuznechik.SetKey(key);
        }

        public string DecryptPassword(string cryptedPassword)
        {
            byte[] bytes = Convert.FromBase64String(cryptedPassword);
            string hex = BitConverter.ToString(bytes);
            hex = hex.Replace("-", "").ToUpper();

            string[] inputText = BlockSplitter.BlockSplitterContainer128(hex);
            string[] outputBlocks = new string[inputText.Length];

            for (int i = 0; i < inputText.Length; i++)
            {
                outputBlocks[i] = ByteToHex.ByteToHexConverter(
                    kuznechik.Decrypt(HexToByte.HexToByteConvert(inputText[i])));
            }

            string password = Encoding.ASCII.GetString(GetConvertBytes(outputBlocks));

            return password;
        }

        public string EncryptPassword(string password)
        {
            byte[] hexByte = Encoding.Default.GetBytes(password);
            string hex = BitConverter.ToString(hexByte);
            hex = hex.Replace("-", "").ToUpper();

            string[] inputText = BlockSplitter.BlockSplitterContainer128(hex);
            string[] outputBlocks = new string[inputText.Length];

            for (int i = 0; i < inputText.Length; i++)
            {
                outputBlocks[i] = ByteToHex.ByteToHexConverter(
                    kuznechik.Encrypt(HexToByte.HexToByteConvert(inputText[i])));
            }

            string result = Convert.ToBase64String(GetConvertBytes(outputBlocks));

            return result;
        }

        public string GetPasswordHash(string password, string salt)
        {
            byte[] passwordBytes = Encoding.Default.GetBytes(password);
            byte[] saltBytes = Encoding.Default.GetBytes(salt);
            byte[] saltPassword = new byte[passwordBytes.Length + saltBytes.Length];

            Array.Copy(passwordBytes, 0, saltPassword, 0, passwordBytes.Length);
            Array.Copy(saltBytes, 0, saltPassword, 0, saltBytes.Length);

            byte[] hashBytes = stribog.GetHash(saltPassword);
            StringBuilder hash = new StringBuilder(128);

            foreach (byte b in hashBytes)
                hash.Append(b.ToString("X2"));

            return hash.ToString();
        }

        private byte[] GetConvertBytes(string[] blocks)
        {
            string result = BlockBuilder.BlockBuilderContainerFrom128(blocks);
            byte[] data = new byte[result.Length / 2];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Convert.ToByte(result.Substring(i * 2, 2), 16);
            }

            return data;
        }
    }
}
