using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RuleEngine.Domain.RequestResponse
{
    public class AfterPaymentExecutionResponse
    {
        [JsonProperty("executionStatusCode")]
        public HttpStatusCode StatusCode { get; set; }

        [JsonProperty("executionStatusMessage")]
        public HttpStatusCode StatusMessage { get; set; }

        [JsonProperty("logs")]
        public List<string> Logs { get; set; }
    }
}
