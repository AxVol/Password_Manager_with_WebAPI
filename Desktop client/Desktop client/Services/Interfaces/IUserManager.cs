using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Desktop_client.Models;

namespace Desktop_client.Services.Interfaces
{
    public interface IUserManager
    {
        public User User { get; set; }
        public ObservableCollection<Password> Passwords { get; set; }

        public Task AddPassword(Password password);
        public Task RemovePassword(Password id);
        public Task UpdatePassword(Password password);
    }
}
