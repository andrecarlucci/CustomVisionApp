using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomVisionApp.Models {
    public class CustomVisionClient {
        private readonly string _projectId;
        private CustomVisionPredictionClient _client;

        public CustomVisionClient(string token, string url, string projectId) {
            _client = new CustomVisionPredictionClient();
            _client.ApiKey = token;
            _client.Endpoint = url;
            _projectId = projectId;
        }

        public async Task<string> Analyze(byte[] image) {
            var imageStream = new MemoryStream(image);
            var prediction = await _client.PredictImageAsync(new Guid(_projectId), imageStream);
            return prediction.Predictions.FirstOrDefault().TagName ?? "None";
        }
    }
}