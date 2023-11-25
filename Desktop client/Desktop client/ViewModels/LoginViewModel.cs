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
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IPageService pageService;
        private readonly IConnectionService connectionService;

        [ObservableProperty]
        private string? login;
        [ObservableProperty]
        private string? password;
        [ObservableProperty]
        private string? errorMessage;
        [ObservableProperty]
        private bool enableButton = true;

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

            if (!connection.HasEthernet())
            {
                ErrorMessage = "Отсутствует интернет";
                EnableButton = false;
            }
        }

        [RelayCommand]
        private void Register(object data)
        {
            pageService.ChangePage(new RegisterPage());
        }

        [RelayCommand]
        private async Task ShowPassword(object data)
        {
            await Task.Run(() => 
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
            });
        }

        [RelayCommand]
        private async Task LoginUser(object data)
        {
            EnableButton = false;

            PasswordBox passwordBox = data as PasswordBox;

            if (passwordBox.Visibility == System.Windows.Visibility.Visible)
                Password = passwordBox.Password;

            if (CanLogin())
            {
                string response = await connectionService.Login(Login, Password);

                if (response == "Успешно")
                {
                    pageService.ChangePage(new PasswordsPage());

                    return;
                }
                ErrorMessage = response;
            }

            EnableButton = true;
            ErrorMessage ??= "Непредвиденная ошибка";
        }

        private bool CanLogin()
        {
            if ((Login == null || Password == null)
               || (Login == string.Empty || Password == string.Empty))
            {
                ErrorMessage = "Не все поля заполены";

                return false;
            }

            return true;
        }
    }
}
