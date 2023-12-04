using Desktop_client.Models;
using Desktop_client.Services;
using Desktop_client.Services.Interfaces;
using Desktop_client.Views;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

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
        private string? password;
        [ObservableProperty]
        private bool isEnabled = true;
        [ObservableProperty]
        private string title = "Добавление пароля";
        [ObservableProperty]
        private string buttonText = "Добавить пароль";

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
                Password = pageService.Password.PassWord;
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
            IsEnabled = false;
            Password passWord = new Password();

            if (pageService.Password != null)
                passWord.Id = pageService.Password.Id;

            passWord.PassWord = Password;
            passWord.Service = Service;
            passWord.Login = Login;
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
                    await passwordService.Create(password, userManager.Token);
                    userManager.AddPassword(passWord);

                    break;
                case "Update":
                    await passwordService.Update(password, userManager.Token);
                    userManager.UpdatePassword(passWord);
                    
                    break;
            }

            pageService.ChangePage(new PasswordsPage());
        }
    }
}
