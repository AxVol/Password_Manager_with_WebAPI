using System.Collections.Generic;

namespace Desktop_client.Models
{
    public class UserManager : IUserManager
    {
        public User user { get; set; }
        public IEnumerable<Password> passwords { get; set; }
    }
}
