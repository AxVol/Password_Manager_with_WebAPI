using Desktop_client.Core;
using Desktop_client.Services.Interfaces;
using Desktop_client.Views;
using System.Windows;
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
        private IUserManager userManager;

        public ObservableCollection<Password> Passwords { get; private set; }
        public bool IsEnabled { get; set; } = true;
        public string Token { get; set; }
        public BitmapSource CopyImage 
        { 
            get
            {
                BitmapSource image = ConvertImage("image\\copy.png");

                return image;
            }
        }

        public Commands LogOut { get; set; }
        public Commands AddPassword { get; set; }
        public Commands Copy { get; set; }
        public Commands UpdateToken { get; set; }
        public Commands Update { get; set; }
        public Commands Delete { get; set; }

        public PasswordsViewModel(IPageService page, IUserManager manager, IPasswordService password) 
        {
            pageService = page;
            userManager = manager;
            passwordService = password;

            if (userManager.user != null)
            {
                Passwords = userManager.passwords;
                Token = userManager.user.SecretToken;
            }

            LogOut = new Commands(Logout);
            AddPassword = new Commands(Addpassword);
            Copy = new Commands(CopyToken);
            UpdateToken = new Commands(TokenUpdate);
            Update = new Commands(UpdatePassword);
            Delete = new Commands(DeletePassword);
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

        private async void TokenUpdate(object data)
        {
            string token = await passwordService.UpdateUserToken(userManager.user.SecretToken);

            userManager.user.SecretToken = token;
            Token = token;
        }

        private void CopyToken(object data)
        {
            Clipboard.Clear();
            Clipboard.SetText(Token);
        }

        private void Addpassword(object data)
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

        private BitmapSource ConvertImage(string path)
        {
            Bitmap bitmap = (Bitmap)Bitmap.FromFile($"{path}", true);
            BitmapSource bitmapSource = BitmapToBitmapSource(bitmap);

            return bitmapSource;
        }

        private BitmapSource BitmapToBitmapSource(Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                          source.GetHbitmap(),
                          IntPtr.Zero,
                          Int32Rect.Empty,
                          BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
