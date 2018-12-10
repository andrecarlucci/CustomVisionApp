using CustomVisionApp.TensorFlow;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomVisionApp.Models {
    public class LocalCustomVisionClient {

        private Dictionary<string, TensorFlowEngine> _engines = new Dictionary<string, TensorFlowEngine>();

        public Task<string> Analyze(byte[] image, string projectId) {
            EnsureEngine(projectId);
            var result = _engines[projectId].Run(image);
            return Task.FromResult(result.Label);
        }

        private void EnsureEngine(string projectId) {
            if(!_engines.ContainsKey(projectId)) {
                _engines.Add(projectId, new TensorFlowEngine(projectId));
            }
        }
    }
}