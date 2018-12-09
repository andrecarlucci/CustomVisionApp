using OpenCvSharp;
using System;
using System.Threading.Tasks;

namespace CustomVisionApp.Models {
    public class MyCamera : IDisposable {
        private bool _running;
        private object _sync = new object();
        private VideoCapture _capture;
        public Action<Mat> NewImage;

        private DateTime _lastFrame;
        public double FramesPerSecond { get; set; }

        public async Task StartCamera(int delay = 0) {
            _running = true;
            _capture = new VideoCapture(0);
            _lastFrame = DateTime.Now;
            while (_running) {
                lock (_sync) {
                    if (_capture?.IsDisposed ?? true) {
                        return;
                    }
                    using (var mat = new Mat()) {
                        _capture.Read(mat);
                        NewImage?.Invoke(mat.Clone());
                    }
                    FramesPerSecond = 1 / (DateTime.Now - _lastFrame).TotalSeconds;
                    _lastFrame = DateTime.Now;
                }
                if (_running && delay > 0) {
                    await Task.Delay(delay);
                }
            }
        }

        public void Dispose() {
            lock (_sync) {
                _running = false;
                _capture?.Dispose();
                _capture = null;
            }
        }
    }
}
