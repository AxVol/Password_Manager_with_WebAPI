using Desktop_client.Core;
using Desktop_client.Services.Interfaces;
using Desktop_client.Views;
using System.Threading.Tasks;

namespace Desktop_client.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        private readonly IPageService pageService;
        private readonly IConnectionService connectionService;

        public Commands LoginCommand { get; set; }
        public Commands RegisterCommand { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public LoginViewModel(IPageService page, IConnectionService connection)
        {
            pageService = page;
            connectionService = connection;

            LoginCommand = new Commands(LoginUser);
            RegisterCommand = new Commands(Register);
        }

        private void LoginUser(object data)
        {
            Task<string> response = connectionService.Login(Login, Password);

            if (response.Result == "Успешно") 
            {
                pageService.ChangePage(new PasswordsPage());
            }
        }
        private void Register(object data)
        {
            pageService.ChangePage(new RegisterPage());
        }
    }
}
