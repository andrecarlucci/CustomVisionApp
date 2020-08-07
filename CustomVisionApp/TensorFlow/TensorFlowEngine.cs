using System;
using System.Diagnostics;
using System.IO;
using TensorFlow;

namespace CustomVisionApp.TensorFlow {
    public class TensorFlowEngine : IDisposable {

        public const string TensorFlowModelFilePath = "Assets/{0}/model.pb";
        public const string TensorFlowLabelsFilePath = "Assets/{0}/labels.txt";

        private TFGraph _graph;
        private string[] _labels;

        public TensorFlowEngine(byte[] model, string[] labels) {
            Create(model, labels);
        }

        public TensorFlowEngine(string projectId) {
            var model = File.ReadAllBytes(String.Format(TensorFlowModelFilePath, projectId));
            var labels = File.ReadAllLines(String.Format(TensorFlowLabelsFilePath, projectId));
            Create(model, labels);
        }

        private void Create(byte[] model, string[] labels) {
            _graph = new TFGraph();
            _labels = labels;
            _graph.Import(model);
        }

        public void Dispose() {
            _graph.Dispose();
        }

        public TensorResult Run(byte[] image) {
            var result = new Result();
            var sw = new Stopwatch();

            var res = new TensorResult();
            using (var session = new TFSession(_graph)) {
                sw.Start();
                var tensor = ImageUtil.CreateTensorFromImage(image);
                res.TimeToCreateImage = sw.Elapsed;

                var runner = session.GetRunner();
                runner.AddInput(_graph["Placeholder"][0], tensor).Fetch(_graph["loss"][0]);

                sw.Restart();
                var output = runner.Run();
                res.TimeToRecognize = sw.Elapsed;

                var allResults = output[0];
                var probabilities = ((float[][])allResults.GetValue(jagged: true))[0];
                for (var i = 0; i < probabilities.Length; i++) {
                    if (probabilities[i] > result.Idx) {
                        result.Label = _labels[i];
                        result.Idx = probabilities[i];
                    }
                }
            }
            res.Result = result;
            return res;
        }
    }

    public class TensorResult
    {
        public Result Result { get; set; }
        public TimeSpan TimeToCreateImage { get; set; }
        public TimeSpan TimeToRecognize { get; set; }
    }
}
