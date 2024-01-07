using Desktop_client.Services.Interfaces;
using Desktop_client.Views;
using System;
using System.Collections.ObjectModel;
using Desktop_client.Models;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
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
        private ObservableCollection<Password>? passwords;
        [ObservableProperty]
        private bool isEnabled = true;
        [ObservableProperty]
        private bool popupIsOpen = false;
        [ObservableProperty]
        private bool errorPopupIsOpen = false;
        [ObservableProperty]
        private string? errorMessage;

        public PasswordsViewModel(IPageService page, IUserManager manager, IPasswordService password) 
        {
            pageService = page;
            userManager = manager;
            passwordService = password;

            if (userManager.Token != null)
            {
                Passwords = userManager.Passwords;
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
        private void ShowErrorPopup()
        {
            ErrorPopupIsOpen = true;

            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(20)
            };

            timer.Tick += delegate (object? sender, EventArgs e)
            {
                ((DispatcherTimer)timer).Stop();

                if (PopupIsOpen)
                    ErrorPopupIsOpen = false;
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
            pageService.Password = password;
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
                Login = password.Login,
                Password = password.PassWord,
                Service = password.Service,
            };

            string status = await passwordService.Delete(sendModel, userManager.Token);
            
            if (status == "")
            {
                userManager.RemovePassword(password);
                pageService.ChangePage(new PasswordsPage());

                return;
            }
            else
            {
                ErrorMessage = status;
                ShowErrorPopup();
            }
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
            userManager.Token = null;
            userManager.Passwords = null;

            pageService.ChangePage(new LoginPage());
        }
    }
}
