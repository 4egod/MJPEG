using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SimplePlayer
{
    public static class ImageSourceConverter
    {
        public static ImageSource ToImageSource(this byte[] rawImage)
        {
            BitmapImage img = new BitmapImage();
            MemoryStream ms = new MemoryStream(rawImage);
            img.BeginInit();
            img.StreamSource = ms;
            img.EndInit();

            ImageSource res = img as ImageSource;

            return res;
        }
    }
}
