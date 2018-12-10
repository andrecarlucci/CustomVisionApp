using System.IO;
using System.Windows.Media.Imaging;

namespace CustomVisionApp.Models {
    public static class ImageHelper {

        public static BitmapImage ToImage(byte[] imageInBytes) {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = new MemoryStream(imageInBytes);
            image.EndInit();
            return image;
        }

        public static BitmapImage ToImage(Stream stream) {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        public static byte[] ToImage(BitmapImage bitmapImage) {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (MemoryStream ms = new MemoryStream()) {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }
    }
}
