using Desktop_client.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using Desktop_client.Models;
using Desktop_client.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Desktop_client.Enums;

namespace Desktop_client.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly IConnectionService connectionService;
        private readonly IPageService pageService;
        private readonly IPasswordService passwordService;

        [ObservableProperty]
        private string? errorMessage;
        [ObservableProperty]
        private string? login;
        [ObservableProperty]
        private string? email;
        [ObservableProperty]
        private string? repeatPassword;
        [ObservableProperty]
        private bool enableButton = true;
        [ObservableProperty]
        private string? passwordStatus;

        private string? password;
        public string? Password
        {
            get
            {
                if (password is not null)
                {
                    PasswordQuality quality = passwordService.GetPasswordQuality(password).Result;

                    switch (quality)
                    {
                        case PasswordQuality.Hard:
                            PasswordStatus = "Сложный";
                            break;
                        case PasswordQuality.Medium:
                            PasswordStatus = "Средний";
                            break;
                        case PasswordQuality.Easy:
                            PasswordStatus = "Легкий";
                            break;
                    }
                }

                return password;
            }
            set
            {
                password = value;
            }
        }

        public RegisterViewModel(IConnectionService connection, IPageService page, IPasswordService pass) 
        {
            connectionService = connection;
            pageService = page;
            passwordService = pass;
        }

        [RelayCommand]
        private void BackToLogin(object data)
        {
            pageService.ChangePage(new LoginPage());
        }

        [RelayCommand]
        private async Task Register(object data)
        {
            EnableButton = false;

            if (await CanRegister())
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

        private async Task<bool> CanRegister()
        {
            if (Login == null || Email == null || Password == null || RepeatPassword == null)
            {
                ErrorMessage = "Заполните все поля";

                return false;
            }

            EmailAddressAttribute emailValidate = new EmailAddressAttribute();

            if ((Login != string.Empty)
                || (Email != string.Empty)
                || (Password != string.Empty)
                || (RepeatPassword != string.Empty))
            {
                if (Password == RepeatPassword)
                {
                    if (await passwordService.GetPasswordQuality(Password) == PasswordQuality.Hard)
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
