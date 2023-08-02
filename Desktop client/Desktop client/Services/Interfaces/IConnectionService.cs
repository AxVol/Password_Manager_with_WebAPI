using Desktop_client.Models;
using System.Threading.Tasks;

namespace Desktop_client.Services.Interfaces
{
    public interface IConnectionService
    {
        public Task<string> Login(string login, string password);
        public Task Register(User user);
    }
}
