using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Test.Helpers
{
    public class ResponseReaderHelper
    {
        private readonly string _filePath;

        public ResponseReaderHelper(string filePath)
        {
            _filePath = filePath;
        }

        public string GetMockResponseFromJsonFile()
        {
            if (string.IsNullOrEmpty(_filePath))
                throw new ArgumentNullException($"Response json file path is required");

            string content = File.ReadAllText(_filePath); ;
            return content;
        }
    }
}
