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
        public string ErrorMessage { get; set; }
        public bool EnableButton { get; set; } = true;

        public LoginViewModel(IPageService page, IConnectionService connection)
        {
            pageService = page;
            connectionService = connection;

            LoginCommand = new Commands(LoginUser);
            RegisterCommand = new Commands(Register);
        }

        private async void LoginUser(object data)
        {
            if ((Login == null || Password == null)
                || (Login == string.Empty || Password == string.Empty))
            {
                ErrorMessage = "Не все поля заполены";
            }
            else
            {
                EnableButton = false;
                string response = await connectionService.Login(Login, Password);

                if (response == "Успешно")
                {
                    pageService.ChangePage(new PasswordsPage());

                    return;
                }

                EnableButton = true;
                ErrorMessage = response;
            }
        }
        private void Register(object data)
        {
            pageService.ChangePage(new RegisterPage());
        }
    }
}
