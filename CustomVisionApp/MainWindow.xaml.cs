using CustomVisionApp.Models;
using Microsoft.Win32;
using OpenCvSharp;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CustomVisionApp {
    public partial class MainWindow : System.Windows.Window {

        private MyCamera _camera;

        public MainWindow() {
            InitializeComponent();
        }

        private void BStart_Click(object sender, RoutedEventArgs e) {
            if(bStart.Content.Equals("Start")) {
                _camera = new MyCamera();
                _camera.NewImage = NewImage;
                bStart.Content = "Stop";
                bLoad.IsEnabled = false;
                Task.Factory.StartNew(() => _camera.StartCamera(0));
            }
            else {
                _camera.Dispose();
                bStart.Content = "Start";
                bLoad.IsEnabled = true;
            }
        }

        private void NewImage(Mat frame) {
            var stream = frame.ToMemoryStream(".png");
            var bitmap = new Bitmap(stream);
            ChangeUI(() => {
                var source = ConvertToBitmapSource(bitmap);
                TheImage.Source = source;
                TheWindow.Title = "Azure Vision: " + _camera.FramesPerSecond.ToString();
            });
        }

        private void ChangeUI(Action action) {
            Dispatcher.BeginInvoke(action);
        }

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public ImageSource ConvertToBitmapSource(Bitmap bmp) {
            var handle = bmp.GetHbitmap();
            try {
                return Imaging.CreateBitmapSourceFromHBitmap(
                    handle,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        private void BDescribe_Click(object sender, RoutedEventArgs e) {
            
        }

        private void BLoad_Click(object sender, RoutedEventArgs e) {
            var op = new OpenFileDialog {
                Title = "Select a picture",
                Filter = @"All supported graphics|*.jpg;*.jpeg;*.png|
                         JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|
                         Portable Network Graphic (*.png)|*.png"
            };
            if (op.ShowDialog() == true) {
                TheImage.Source = new BitmapImage(new Uri(op.FileName));
            }
        }
    }
}
