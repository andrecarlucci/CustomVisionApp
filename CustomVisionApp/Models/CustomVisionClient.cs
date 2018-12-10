using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomVisionApp.Models {
    public class CustomVisionClient {
        private CustomVisionPredictionClient _client;
        public static string Url = "https://southcentralus.api.cognitive.microsoft.com";

        public CustomVisionClient(string token) {
            _client = new CustomVisionPredictionClient();
            _client.ApiKey = token;
            _client.Endpoint = Url;
        }

        public async Task<string> Analyze(byte[] image, string projectId) {
            var imageStream = new MemoryStream(image);
            var prediction = await _client.PredictImageAsync(new Guid(projectId), imageStream);
            return prediction.Predictions.FirstOrDefault().TagName ?? "None";
        }
    }
}