using Desktop_client.Core;
using Desktop_client.Services.Interfaces;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Desktop_client.Models;
using Desktop_client.Views;

namespace Desktop_client.ViewModels
{
    public class RegisterViewModel : ObservableObject
    {
        private readonly IConnectionService connectionService;
        private readonly IPageService pageService;

        public Commands Register { get; set; }
        public Commands BackToLogin { get; set; }

        public string ErrorMessage { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public bool EnableButton { get; set; } = true;

        public RegisterViewModel(IConnectionService connection, IPageService page) 
        {
            connectionService = connection;
            pageService = page;

            Register = new Commands(Registration);
            BackToLogin = new Commands(BackLogin);
        }

        private void BackLogin(object data)
        {
            pageService.ChangePage(new LoginPage());
        }

        private async void Registration(object data)
        {
            char[] specSymbols = new char[]
            {
                '?', '!', '@', '#', '$', '%', '&' 
            };
            EmailAddressAttribute emailValidate = new EmailAddressAttribute();

            if (Login == null || Email == null || Password == null || RepeatPassword == null)
            {
                ErrorMessage = "Заполните все поля";

                return;
            }

            EnableButton = false;

            if ((Login != string.Empty)
                || (Email != string.Empty)
                || (Password != string.Empty)
                || (RepeatPassword != string.Empty))
            {
                if (Password == RepeatPassword)
                {
                    if ((Password.IndexOfAny(specSymbols) != -1)
                        && (Password.Length > 8)
                        && (Password.Any(c => char.IsDigit(c)))
                        && (Password.Any(c => char.IsUpper(c))))
                    {
                        if (emailValidate.IsValid(Email))
                        {
                            RegistrationModel user = new RegistrationModel()
                            {
                                Login = Login,
                                Email = Email,
                                Password = Password,
                            };

                            string response = await connectionService.Register(user);

                            if (response == "Успешно")
                                pageService.ChangePage(new PasswordsPage());

                            ErrorMessage = response;
                            EnableButton = true;
                            return;
                        }

                        ErrorMessage = "Некоректная почта";
                        EnableButton = true;
                        return;
                    }

                    ErrorMessage = "Пароль не достаточно сложный";
                    EnableButton = true;
                    return;
                }

                ErrorMessage = "Пароли не совпадают";
                EnableButton = true;
                return;
            }

            ErrorMessage = "Заполните все поля";
            EnableButton = true;
        }
    }
}
