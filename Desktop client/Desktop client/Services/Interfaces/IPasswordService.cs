using Desktop_client.Enums;
using Desktop_client.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Desktop_client.Services.Interfaces
{
    public interface IPasswordService
    {
        public Task<ObservableCollection<Password>> GetAll(string token);
        public Task<string> Delete(PasswordSendModel model, string token);
        public Task<string> Update(PasswordSendModel model, string token);
        public Task<string> Create(PasswordSendModel model, string token);
        public Task<string> GenerateStrongPassword();
        public Task<PasswordQuality> GetPasswordQuality(string password);
    }
}
