using Desktop_client.Services.Interfaces;
using Desktop_client.Views;
using System;
using System.Collections.ObjectModel;
using Desktop_client.Models;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace Desktop_client.ViewModels
{
    public partial class PasswordsViewModel : ObservableObject
    {
        private readonly IPageService pageService;
        private readonly IPasswordService passwordService;
        private readonly IUserManager userManager;

        [ObservableProperty]
        private ObservableCollection<Password> passwords;
        [ObservableProperty]
        private bool isEnabled = true;
        [ObservableProperty]
        private bool popupIsOpen = false;

        public PasswordsViewModel(IPageService page, IUserManager manager, IPasswordService password) 
        {
            pageService = page;
            userManager = manager;
            passwordService = password;

            if (userManager.user != null)
            {
                Passwords = userManager.passwords;
            }
        }

        [RelayCommand]
        private void CopyInBuffer(object data)
        {
            string copy = (string)data;

            Clipboard.Clear();
            Clipboard.SetText(copy);

            ShowPopup();
        }

        [RelayCommand]
        private void ShowPopup()
        {
            PopupIsOpen = true;

            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(5)
            };

            timer.Tick += delegate (object? sender, EventArgs e)
            {
                ((DispatcherTimer)timer).Stop();

                if (PopupIsOpen)
                    PopupIsOpen = false;
            };

            timer.Start();
        }

        [RelayCommand]
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

        [RelayCommand]
        private void UpdatePassword(object data)
        {
            int id = Convert.ToInt32(data);
            Password password = Passwords.First(p => p.Id == id);

            pageService.PasswordPageStatus = "Update";
            pageService.password = password;
            pageService.ChangePage(new AddPassword());
        }

        [RelayCommand]
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
            await userManager.RemovePassword(password);

            pageService.ChangePage(new PasswordsPage());
        }

        [RelayCommand]
        private async Task UpdateToken(object data)
        {
            string token = await passwordService.UpdateUserToken(userManager.user.SecretToken);

            userManager.user.SecretToken = token;
        }

        [RelayCommand]
        private void AddPassword(object data)
        {
            pageService.PasswordPageStatus = "Add";
            pageService.ChangePage(new AddPassword());
        }

        [RelayCommand]
        private void Logout(object data)
        {
            userManager.user = null;
            userManager.passwords = null;

            pageService.ChangePage(new LoginPage());
        }
    }
}
