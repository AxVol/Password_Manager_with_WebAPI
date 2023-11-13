using Desktop_client.Core;
using Desktop_client.Services;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Desktop_client.Models
{
    public class Password : ObservableObject
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string PassWord { get; set; }
        public string Service { get; set; }
        public Visibility PasswordStatus { get; set; } = Visibility.Hidden;
        public Visibility HiddenPasswordStatus { get; set; } = Visibility.Visible;
        public string HiddenPassword 
        {
            get
            {
                string password = string.Empty;

                for (int i = 0; i < PassWord.Length; i++)
                {
                    password += "•";
                }

                return password;
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
    }
}
