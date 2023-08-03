using Desktop_client.Core;
using Desktop_client.Models;
using Desktop_client.Services.Interfaces;
using Desktop_client.Views;
using System.Linq;

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

        public Commands SendPassword { get; set; }
        public Commands GeneratePassword { get; set; }
        public Commands Back { get; set; }

        public AddPasswordViewModel(IPasswordService password, IPageService page, IUserManager manager) 
        {
            userManager = manager;
            passwordService = password;
            pageService = page;

            SendPassword = new Commands(Send);
            GeneratePassword = new Commands(PasswordGenerator);
            Back = new Commands(BackToPasswords);

            if (pageService.PasswordPageStatus == "Update")
            {
                Service = pageService.password.Service;
                Login = pageService.password.Login;
                Password = pageService.password.PassWord;
            }
        }

        private async void PasswordGenerator(object data)
        {
            IsEnabled = false;
            string pass = await passwordService.GenerateStrongPassword();

            Password = pass;
            IsEnabled = true;
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
