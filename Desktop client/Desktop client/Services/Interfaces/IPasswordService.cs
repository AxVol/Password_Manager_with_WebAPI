using Desktop_client.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Desktop_client.Services.Interfaces
{
    public interface IPasswordService
    {
        public Task<ObservableCollection<Password>> GetAll(string token);
        public Task Delete(PasswordSendModel model);
        public Task Update(PasswordSendModel model);
        public Task Create(PasswordSendModel model);
        public Task<string> UpdateUserToken(string token);
        public Task<string> GenerateStrongPassword();
    }
}
