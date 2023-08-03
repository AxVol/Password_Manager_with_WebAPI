using Desktop_client.Core;
using Desktop_client.Services.Interfaces;
using Desktop_client.Views;
using System.Windows;
using System;
using System.Windows.Media.Imaging;
using System.Drawing;
using Desktop_client.Services.Implementations;

namespace Desktop_client.ViewModels
{
    public class PasswordsViewModel : ObservableObject
    {
        private readonly IPageService pageService;
        private readonly IPasswordService passwordService;
        private IUserManager userManager;

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

        public PasswordsViewModel(IPageService page, IUserManager manager, IPasswordService password) 
        {
            pageService = page;
            userManager = manager;
            passwordService = password;

            if(userManager.user != null)
                Token = userManager.user.SecretToken;

            LogOut = new Commands(Logout);
            AddPassword = new Commands(Addpassword);
            Copy = new Commands(CopyToken);
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
