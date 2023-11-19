using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Desktop_client.Models;
using Desktop_client.Services.Interfaces;

namespace Desktop_client.Services.Implementations
{
    public class UserManager : IUserManager
    {
        public User user { get; set; }
        public ObservableCollection<Password> passwords { get; set; }

        public async Task AddPassword(Password password)
        {
            Password pass = new Password()
            {
                Id = password.Id,
                Login = password.Login,
                PassWord = password.PassWord,
                Service = password.Service,
            };
            passwords.Add(pass);
        }

        public async Task RemovePassword(Password password)
        {
            passwords.Remove(password);
        }

        public async Task UpdatePassword(Password password)
        {
            foreach (Password pass in passwords)
            {
                if (pass.Id == password.Id)
                {
                    pass.PassWord = password.PassWord;
                    pass.Login = password.Login;
                    pass.Service = password.Service;
                    return;
                }
            }
        }
    }
}
