using Desktop_client.Core;
using Desktop_client.Services.Interfaces;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Desktop_client.Models;
using System.Threading.Tasks;
using Desktop_client.Views;

namespace Desktop_client.ViewModels
{
    public class RegisterViewModel : ObservableObject
    {
        private readonly IConnectionService connectionService;
        private readonly IPageService pageService;

        public Commands Register { get; set; }

        public string ErrorMessage { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }

        public RegisterViewModel(IConnectionService connection, IPageService page) 
        {
            connectionService = connection;
            pageService = page;

            Register = new Commands(Registration);
        }

        public void Registration(object data)
        {
            char[] specSymbols = new char[]
            {
                '?', '!', '@', '#', '$', '%', '&' 
            };
            EmailAddressAttribute emailValidate = new EmailAddressAttribute();

            if ((Login != string.Empty)
                && (Email != string.Empty)
                && (Password != string.Empty)
                && (RepeatPassword != string.Empty))
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

                            string response = connectionService.Register(user).Result;

                            if (response == "Успешно")
                                pageService.ChangePage(new PasswordsPage());

                            ErrorMessage = response;
                        }

                        ErrorMessage = "Некоректная почта";
                        return;
                    }

                    ErrorMessage = "Пароль не достаточно сложный";
                    return;
                }

                ErrorMessage = "Пароли не совпадают";
                return;
            }

            ErrorMessage = "Заполните все поля";
        }
    }
}
