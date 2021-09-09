using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RuleEngine.Domain.RequestResponseDto
{
    public class AfterPaymentExecutionResponse : PackingSlipResponse
    {
        [JsonProperty("logs")]
        public List<string> Logs { get; set; }
    }
}
