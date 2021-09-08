using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RuleEngine.Domain.RequestResponse
{
    public class AfterPaymentExecutionRequest
    {
        [JsonProperty("productName")]
        public string ProductName { get; set; }
    }
}
