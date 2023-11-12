using System;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Desktop_client.Services
{
    class ImageConverter
    {
        public static BitmapSource ConvertImage(string path)
        {
            Bitmap bitmap = (Bitmap)Bitmap.FromFile($"{path}", true);
            BitmapSource bitmapSource = BitmapToBitmapSource(bitmap);

            return bitmapSource;
        }

        private static BitmapSource BitmapToBitmapSource(Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                          source.GetHbitmap(),
                          IntPtr.Zero,
                          Int32Rect.Empty,
                          BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
