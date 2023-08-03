using System.Collections.Generic;
using System.Collections.ObjectModel;
using Desktop_client.Models;

namespace Desktop_client.Services.Interfaces
{
    public interface IUserManager
    {
        public User user { get; set; }
        public ObservableCollection<Password> passwords { get; set; }
    }
}
