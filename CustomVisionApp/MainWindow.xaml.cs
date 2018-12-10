using CustomVisionApp.Models;
using CustomVisionApp.StreetFighter;
using Microsoft.Win32;
using OpenCvSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CustomVisionApp {
    public partial class MainWindow : System.Windows.Window {

        private const string _projectIdBolacha = "70bc8939-75f0-417c-ba86-06fdf03b46df";
        private const string _projectIdHadouken = "a71c971b-c791-479b-b8ad-54e431197bad";

        private MyCamera _camera;
        private VisionClient _visionClient;
        private CustomVisionClient _customVisionClient;
        private LocalCustomVisionClient _localCustomVisionClient;
        private BitmapImage _currentImage;
        private string _currentProjectId;
        
        public MainWindow() {
            InitializeComponent();
            var url = "https://brazilsouth.api.cognitive.microsoft.com";
            var token = Environment.GetEnvironmentVariable("AZURE-COGNITIVESERVICES-TOKEN", EnvironmentVariableTarget.User);
            var predictionToken = Environment.GetEnvironmentVariable("AZURE-CUSTOMVISION-PREDICTION-TOKEN", EnvironmentVariableTarget.User);
            
            _visionClient = new VisionClient(token, url);
            _customVisionClient = new CustomVisionClient(predictionToken);
            _localCustomVisionClient = new LocalCustomVisionClient();
            _currentProjectId = _projectIdBolacha;
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

        private void BLoad_Click(object sender, RoutedEventArgs e) {
            var op = new OpenFileDialog {
                Title = "Select a picture",
                Filter = @"All supported graphics|*.jpg;*.jpeg;*.png|
                         JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|
                         Portable Network Graphic (*.png)|*.png"
            };
            if (op.ShowDialog() == true) {
                _currentImage = new BitmapImage(new Uri(op.FileName));
                TheImage.Source = _currentImage;
            }
        }

        private async void BDescribe_Click(object sender, RoutedEventArgs e) {
            if(_currentImage == null) {
                return;
            }
            var image = ImageHelper.ToImage(_currentImage);

            string description = "Analyze...";
            Description.Text = description;
            if (ServiceGeneric.IsChecked ?? false) {
                description = await _visionClient.Analyze(image);
            }
            else if(ServiceCustom.IsChecked ?? false) {
                description = await _customVisionClient.Analyze(image, _currentProjectId);
            }
            else if(ServiceLocal.IsChecked ?? false) {
                description = await _localCustomVisionClient.Analyze(image, _currentProjectId);
            }
            Description.Text = description;
        }

        private void NewImage(Mat frame) {
            var bytes = frame.ToBytes(".png");
            ChangeUI(() => {
                _currentImage = ImageHelper.ToImage(bytes);
                TheImage.Source = _currentImage;
                TheWindow.Title = "Azure Vision: " + _camera.FramesPerSecond.ToString();                
            });

            var result = _localCustomVisionClient.Analyze(bytes, _currentProjectId).Result;

            //SpecialAtacks.Execute(result);
        }

        private void ChangeUI(Action action) {
            Dispatcher.BeginInvoke(action);
        }

        private void BSave_Click(object sender, RoutedEventArgs e) {
            var bytes = ImageHelper.ToImage(_currentImage);
            File.WriteAllBytes("image.jpg", bytes);
        }

        private void Bolacha_Checked(object sender, RoutedEventArgs e) {
            _currentProjectId = _projectIdBolacha;
        }

        private void Hadouken_Checked(object sender, RoutedEventArgs e) {
            _currentProjectId = _projectIdHadouken;
        }
    }
}
