using Newtonsoft.Json;
using System.Collections.Generic;

namespace RuleEngine.Domain.RequestResponseDto
{
    public class AfterPaymentExecutionResponse : PackingSlipResponse
    {
        [JsonProperty("logs")]
        public List<string> Logs { get; set; }
    }
}
