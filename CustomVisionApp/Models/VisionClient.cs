using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomVisionApp.Models {
    public class VisionClient {

        private ComputerVisionClient _client;

        public VisionClient(string token, string url) {
            _client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(token));
            _client.Endpoint = url;
        }

        public async Task<string> Analyze(byte[] image) {
            var imageStream = new MemoryStream(image);
            var description = await _client.DescribeImageInStreamAsync(imageStream, 1, "pt");

            var captions = String.Join(" - ", description.Captions.Select(c => c.Text));
            var tags = String.Join(" - ", description.Tags);

            return $"{captions}\r\nTags: {tags}";            
        }
    }
}
