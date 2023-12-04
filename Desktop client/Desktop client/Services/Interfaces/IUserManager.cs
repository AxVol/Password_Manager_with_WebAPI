using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Desktop_client.Models;

namespace Desktop_client.Services.Interfaces
{
    public interface IUserManager
    {
        public string Token { get; set; }
        public ObservableCollection<Password> Passwords { get; set; }

        public void AddPassword(Password password);
        public void RemovePassword(Password id);
        public void UpdatePassword(Password password);
    }
}
