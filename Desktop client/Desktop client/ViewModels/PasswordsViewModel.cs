using Desktop_client.Core;
using Desktop_client.Services.Interfaces;
using Desktop_client.Views;
using System;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Collections.ObjectModel;
using Desktop_client.Models;
using System.Linq;

namespace Desktop_client.ViewModels
{
    public class PasswordsViewModel : ObservableObject
    {
        private readonly IPageService pageService;
        private readonly IPasswordService passwordService;
        private readonly IUserManager userManager;

        public ObservableCollection<Password> Passwords { get; private set; }
        public bool IsEnabled { get; set; } = true;

        public Commands LogoutCommand { get; set; }
        public Commands AddPasswordCommand { get; set; }
        public Commands UpdateTokenCommand { get; set; }
        public Commands UpdatePasswordCommand { get; set; }
        public Commands DeletePasswordCommand { get; set; }
        public Commands SwitchPasswordVisibilityCommand { get; set; }

        public PasswordsViewModel(IPageService page, IUserManager manager, IPasswordService password) 
        {
            pageService = page;
            userManager = manager;
            passwordService = password;

            if (userManager.user != null)
            {
                Passwords = userManager.passwords;
            }

            LogoutCommand = new Commands(Logout);
            AddPasswordCommand = new Commands(AddPassword);
            UpdateTokenCommand = new Commands(UpdateToken);
            UpdatePasswordCommand = new Commands(UpdatePassword);
            DeletePasswordCommand = new Commands(DeletePassword);
            SwitchPasswordVisibilityCommand = new Commands(SwitchPasswordVisibility);
        }

        private void SwitchPasswordVisibility(object data)
        {
            int id = Convert.ToInt32(data);
            Password password = Passwords.First(p => p.Id == id);

            if (password.HiddenPasswordStatus == System.Windows.Visibility.Hidden)
            {
                password.HiddenPasswordStatus = System.Windows.Visibility.Visible;
                password.PasswordStatus = System.Windows.Visibility.Hidden;
            }
            else
            {
                password.HiddenPasswordStatus = System.Windows.Visibility.Hidden;
                password.PasswordStatus = System.Windows.Visibility.Visible;
            }
        }

        private void UpdatePassword(object data)
        {
            int id = Convert.ToInt32(data);
            Password password = Passwords.First(p => p.Id == id);

            pageService.PasswordPageStatus = "Update";
            pageService.password = password;
            pageService.ChangePage(new AddPassword());
        }

        private async void DeletePassword(object data)
        {
            IsEnabled = false;
            int id = Convert.ToInt32(data);
            Password password = Passwords.First(p => p.Id == id);

            PasswordSendModel sendModel = new PasswordSendModel()
            {
                Id = password.Id,
                SecretToken = userManager.user.SecretToken,
                Login = password.Login,
                Password = password.PassWord,
                Service = password.Service,
            };

            await passwordService.Delete(sendModel);
            userManager.passwords = await passwordService.GetAll(userManager.user.SecretToken);

            pageService.ChangePage(new PasswordsPage());
        }

        private async void UpdateToken(object data)
        {
            string token = await passwordService.UpdateUserToken(userManager.user.SecretToken);

            userManager.user.SecretToken = token;
        }

        private void AddPassword(object data)
        {
            pageService.PasswordPageStatus = "Add";
            pageService.ChangePage(new AddPassword());
        }

        private void Logout(object data)
        {
            userManager.user = null;
            userManager.passwords = null;

            pageService.ChangePage(new LoginPage());
        }
    }
}
