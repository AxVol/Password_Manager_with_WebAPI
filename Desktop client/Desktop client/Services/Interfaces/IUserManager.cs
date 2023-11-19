using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Desktop_client.Models;

namespace Desktop_client.Services.Interfaces
{
    public interface IUserManager
    {
        public User user { get; set; }
        public ObservableCollection<Password> passwords { get; set; }

        public Task AddPassword(Password password);
        public Task RemovePassword(Password id);
        public Task UpdatePassword(Password password);
    }
}
