using System.Collections.Generic;
using Desktop_client.Models;
using Desktop_client.Services.Interfaces;

namespace Desktop_client.Services.Implementations
{
    public class UserManager : IUserManager
    {
        public User user { get; set; }
        public IEnumerable<Password> passwords { get; set; }
    }
}
