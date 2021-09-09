using Newtonsoft.Json;
using System.Collections.Generic;

namespace RuleEngine.Domain.RequestResponseDto
{
    public class PackingSlipResponse
    {
        [JsonProperty("slipHtml")]
        public List<string> SlipHtml { get; set; }
    }
}
