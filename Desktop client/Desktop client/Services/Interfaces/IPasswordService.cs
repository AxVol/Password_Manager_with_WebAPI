using Desktop_client.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Desktop_client.Services.Interfaces
{
    public interface IPasswordService
    {
        public Task<ObservableCollection<Password>> GetAll(string token);
        public Task Delete(PasswordSendModel model, string token);
        public Task Update(PasswordSendModel model, string token);
        public Task Create(PasswordSendModel model, string token);
        public Task<string> GenerateStrongPassword();
    }
}
