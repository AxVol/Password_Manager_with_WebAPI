using System.Collections.Generic;

namespace Desktop_client.Models
{
    public interface IUserManager
    {
        public User user { get; set; }
        public IEnumerable<Password> passwords { get; set; }
    }
}
