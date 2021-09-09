using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Domain.RequestResponseDto
{
    public class PackingSlipResponse
    {
        [JsonProperty("slipHtml")]
        public List<string> SlipHtml { get; set; }
    }
}
