using Desktop_client.Core;
using Desktop_client.Models;
using Desktop_client.Services;
using Desktop_client.Services.Interfaces;
using Desktop_client.Views;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Desktop_client.ViewModels
{
    public class AddPasswordViewModel : ObservableObject
    {
        private readonly IPasswordService passwordService;
        private readonly IUserManager userManager;
        private readonly IPageService pageService;

        public bool IsEnabled { get; set; } = true;
        public string Service { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public BitmapSource HiddenImage
        {
            get
            {
                BitmapSource image = ImageConverter.ConvertImage("image\\hiden.png");

                return image;
            }
        }

        public Commands SendPassword { get; set; }
        public Commands GeneratePassword { get; set; }
        public Commands Back { get; set; }
        public Commands ShowPasswordCommand { get; set; }

        public delegate void PasswordGeneratedHandler(string password);
        public event PasswordGeneratedHandler PasswordGeneratedEvent;

        public AddPasswordViewModel(IPasswordService password, IPageService page, IUserManager manager) 
        {
            userManager = manager;
            passwordService = password;
            pageService = page;

            SendPassword = new Commands(Send);
            GeneratePassword = new Commands(PasswordGenerator);
            Back = new Commands(BackToPasswords);
            ShowPasswordCommand = new Commands(ShowPassword);

            if (pageService.PasswordPageStatus == "Update")
            {
                Service = pageService.password.Service;
                Login = pageService.password.Login;
                Password = pageService.password.PassWord;
            }
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

        private async void PasswordGenerator(object data)
        {
            IsEnabled = false;
            string pass = await passwordService.GenerateStrongPassword();

            Password = pass;
            IsEnabled = true;
            PasswordGeneratedEvent?.Invoke(pass);
        }

        private void BackToPasswords(object data)
        {
            pageService.ChangePage(new PasswordsPage());
        }

        private async void Send(object data)
        {
            IsEnabled = false;
            int id = default;

            if (pageService.password != null)
                id = pageService.password.Id;

            PasswordSendModel password = new PasswordSendModel()
            {
                Id = id,
                SecretToken = userManager.user.SecretToken,
                Service = Service,
                Login = Login,
                Password = Password
            };

            switch (pageService.PasswordPageStatus)
            {
                case "Add":
                    await passwordService.Create(password);
                    break;
                case "Update":
                    await passwordService.Update(password);
                    break;
            }

            userManager.passwords = await passwordService.GetAll(userManager.user.SecretToken);
            pageService.ChangePage(new PasswordsPage());
        }
    }
}
