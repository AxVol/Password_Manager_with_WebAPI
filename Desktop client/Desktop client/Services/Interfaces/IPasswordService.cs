using Desktop_client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desktop_client.Services.Interfaces
{
    public interface IPasswordService
    {
        public Task<IEnumerable<Password>> GetAll(PasswordSendModel model);
        public Task Delete(PasswordSendModel model);
        public Task<Password> Update(PasswordSendModel model);
        public Task<Password> Create(PasswordSendModel model);
        public Task<string> UpdateUserToken(string token);
        public Task<string> GenerateStrongPassword();
    }
}
