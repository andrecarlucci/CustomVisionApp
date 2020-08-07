using CustomVisionApp.TensorFlow;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CustomVisionApp.Models {
    public class LocalCustomVisionClient {

        private Dictionary<string, TensorFlowEngine> _engines = new Dictionary<string, TensorFlowEngine>();

        public Task<TensorResult> Analyze(byte[] image, string projectId) {
            EnsureEngine(projectId);
            try
            {
                var result = _engines[projectId].Run(image);
                return Task.FromResult(result);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error running the model: " + ex);
                return Task.FromResult(new TensorResult { Result = new Result { Label = "Error running the model" } });
            }
        }

        private void EnsureEngine(string projectId) {
            if(!_engines.ContainsKey(projectId)) {
                _engines.Add(projectId, new TensorFlowEngine(projectId));
            }
        }
    }
}