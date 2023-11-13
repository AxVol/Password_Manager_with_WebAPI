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

        public Commands RegisterCommand { get; set; }
        public Commands BackToLoginCommand { get; set; }

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

            RegisterCommand = new Commands(Registration);
            BackToLoginCommand = new Commands(BackToLogin);
        }

        private void BackToLogin(object data)
        {
            pageService.ChangePage(new LoginPage());
        }

        private async void Registration(object data)
        {
            EnableButton = false;

            if (CanRegister())
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

            EnableButton = true;
            ErrorMessage ??= "Не предвиденная ошибка";

            return;
        }

        private bool CanRegister()
        {
            if (Login == null || Email == null || Password == null || RepeatPassword == null)
            {
                ErrorMessage = "Заполните все поля";

                return false;
            }

            char[] specSymbols = new char[]
            {
                '?', '!', '@', '#', '$', '%', '&'
            };

            EmailAddressAttribute emailValidate = new EmailAddressAttribute();

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
                            return true;
                        }

                        ErrorMessage = "Некоректная почта";

                        return false;
                    }

                    ErrorMessage = "Пароль не достаточно сложный";

                    return false;
                }

                ErrorMessage = "Пароли не совпадают";

                return false;
            }

            return false;
        }
    }
}
