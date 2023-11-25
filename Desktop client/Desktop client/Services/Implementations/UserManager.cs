using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Desktop_client.Models;
using Desktop_client.Services.Interfaces;

namespace Desktop_client.Services.Implementations
{
    public class UserManager : IUserManager
    {
        public User User { get; set; }
        public ObservableCollection<Password> Passwords { get; set; }

        public async Task AddPassword(Password password)
        {
            Password pass = new Password()
            {
                Id = password.Id,
                Login = password.Login,
                PassWord = password.PassWord,
                Service = password.Service,
            };
            await Task.Run(() => Passwords.Add(pass));
        }

        public async Task RemovePassword(Password password)
        {
           await Task.Run(() => Passwords.Remove(password));
        }

        public async Task UpdatePassword(Password password)
        {
            await Task.Run(() =>
            {
                foreach (Password pass in Passwords)
                {
                    if (pass.Id == password.Id)
                    {
                        pass.PassWord = password.PassWord;
                        pass.Login = password.Login;
                        pass.Service = password.Service;
                        return;
                    }
                }
            });
        }
    }
}
