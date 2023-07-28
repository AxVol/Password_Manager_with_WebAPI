using System.Security.Cryptography;
using System.Text;
using WebApi.Service.Interfaces;

namespace WebApi.Service.Implementations
{
    public class Cryptography : ICryptography
    {
        public string CypherPassword(string password)
        {
            throw new NotImplementedException();
        }

        public string GetPasswordHash(string password)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));

            return Convert.ToBase64String(hash);
        }
    }
}
