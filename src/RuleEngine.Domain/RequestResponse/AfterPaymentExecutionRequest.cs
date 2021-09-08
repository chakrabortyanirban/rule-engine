using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RuleEngine.Domain.Dtos;

namespace RuleEngine.Domain.RequestResponse
{
    public class AfterPaymentExecutionRequest : CustomerDetails
    {
        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("linksForNonPhysicalProducts")]
        public string LinksForNonPhysicalProducts { get; set; } = string.Empty;
    }
}
