﻿using Desktop_client.Core;
using Desktop_client.Services;
using Desktop_client.Services.Interfaces;
using Desktop_client.Views;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Desktop_client.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        private readonly IPageService pageService;
        private readonly IConnectionService connectionService;

        public Commands LoginCommand { get; set; }
        public Commands RegisterCommand { get; set; }
        public Commands ShowPasswordCommand { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public bool EnableButton { get; set; } = true;
        public BitmapSource HiddenImage 
        {
            get
            {
                BitmapSource image = ImageConverter.ConvertImage("image\\hiden.png");

                return image;
            }
        }

        public LoginViewModel(IPageService page, IConnectionService connection)
        {
            pageService = page;
            connectionService = connection;

            LoginCommand = new Commands(LoginUser);
            RegisterCommand = new Commands(Register);
            ShowPasswordCommand = new Commands(ShowPassword);
        }

        private async void ShowPassword(object data)
        {
            PasswordBox passwordBox = data as PasswordBox;

            if (passwordBox.Visibility == System.Windows.Visibility.Visible)
            {
                Password = passwordBox.Password;
                passwordBox.Visibility = System.Windows.Visibility.Hidden;

                return;
            }

            passwordBox.Password = Password;
            passwordBox.Visibility = System.Windows.Visibility.Visible;
        }

        private async void LoginUser(object data)
        {
            PasswordBox passwordBox = data as PasswordBox;

            if (passwordBox.Visibility == System.Windows.Visibility.Visible)
                Password = passwordBox.Password;

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
