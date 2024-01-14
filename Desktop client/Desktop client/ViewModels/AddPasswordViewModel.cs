using Desktop_client.Models;
using Desktop_client.Services;
using Desktop_client.Services.Interfaces;
using Desktop_client.Views;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Desktop_client.Enums;

namespace Desktop_client.ViewModels
{
    public partial class AddPasswordViewModel : ObservableObject
    {
        private readonly IPasswordService passwordService;
        private readonly IUserManager userManager;
        private readonly IPageService pageService;

        [ObservableProperty]
        private string? service;
        [ObservableProperty]
        private string? login;
        [ObservableProperty]
        private bool isEnabled = true;
        [ObservableProperty]
        private string title = "Добавление пароля";
        [ObservableProperty]
        private string buttonText = "Добавить пароль";
        [ObservableProperty]
        private string? errorMessage;
        [ObservableProperty]
        private string? passwordStatus;

        private string? password;
        public string? PasswordTemp;
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

        public BitmapSource HiddenImage
        {
            get
            {
                BitmapSource image = ImageConverter.ConvertImage("image\\hiden.png");

                return image;
            }
        }

        public delegate void PasswordGeneratedHandler(string password);
        public event PasswordGeneratedHandler? PasswordGeneratedEvent;

        public AddPasswordViewModel(IPasswordService password, IPageService page, IUserManager manager) 
        {
            userManager = manager;
            passwordService = password;
            pageService = page;

            if (pageService.PasswordPageStatus == "Update")
            {
                Service = pageService.Password.Service;
                Login = pageService.Password.Login;
                PasswordTemp = pageService.Password.PassWord;
                ButtonText = "Изменить пароль";
                Title = "Изменение пароля";
            }
        }

        [RelayCommand]
        private void ShowPassword(object data)
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

        [RelayCommand]
        private async Task GeneratePassword(object data)
        {
            IsEnabled = false;
            string pass = await passwordService.GenerateStrongPassword();

            Password = pass;
            IsEnabled = true;
            PasswordGeneratedEvent?.Invoke(pass);
        }

        [RelayCommand]
        private void Back(object data)
        {
            pageService.ChangePage(new PasswordsPage());
        }

        [RelayCommand]
        private async Task SendPassword(object data)
        {
            string status;
            IsEnabled = false;
            Password passWord = new Password();
            errorMessage = null;

            if (await passwordService.GetPasswordQuality(Password) != PasswordQuality.Hard)
            {
                ErrorMessage = "Пароль не достаточно сложный";
                IsEnabled = true;
                return;
            }

            if (pageService.Password != null)
                passWord.Id = pageService.Password.Id;

            passWord.PassWord = Password;
            passWord.Service = Service;
            passWord.Login = Login;

            if (userManager.Passwords is not null)
            {
                foreach (Password pass in userManager.Passwords)
                {
                    if (pass.PassWord == passWord.PassWord)
                    {
                        ErrorMessage = "Ошибка: Такой пароль уже есть в списке ваших паролей.\n В целях безопасности поставте другую комбинацию";
                        IsEnabled = true;

                        return;
                    }
                }
            }

            PasswordSendModel password = new PasswordSendModel()
            {
                Id = passWord.Id,
                Service = Service,
                Login = Login,
                Password = Password
            };

            switch (pageService.PasswordPageStatus)
            {
                case "Add":
                    status = await passwordService.Create(password, userManager.Token);

                    if (status != "")
                    {
                        errorMessage = status;
                        break;
                    }

                    userManager.AddPassword(passWord);
                    break;
                case "Update":
                    status = await passwordService.Update(password, userManager.Token);

                    if (status != "")
                    {
                        errorMessage = status;
                        break;
                    }

                    userManager.UpdatePassword(passWord);
                    break;
            }

            if (errorMessage == null)
                pageService.ChangePage(new PasswordsPage());
        }
    }
}
