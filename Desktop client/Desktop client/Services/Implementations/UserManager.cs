using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Desktop_client.Models;
using Desktop_client.Services.Interfaces;

namespace Desktop_client.Services.Implementations
{
    public class UserManager : IUserManager
    {
        public string Token { get; set; }
        public ObservableCollection<Password> Passwords { get; set; }

        public void AddPassword(Password password)
        {
            Password pass = new Password()
            {
                Id = password.Id,
                Login = password.Login,
                PassWord = password.PassWord,
                Service = password.Service,
            };
            Passwords.Add(pass);
        }

        public void RemovePassword(Password password)
        {
            Passwords.Remove(password);
        }

        public void UpdatePassword(Password password)
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
        }
    }
}
