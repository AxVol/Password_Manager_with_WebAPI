using System.Collections.Generic;
using Desktop_client.Models;

namespace Desktop_client.Services.Interfaces
{
    public interface IUserManager
    {
        public User user { get; set; }
        public IEnumerable<Password> passwords { get; set; }
    }
}
