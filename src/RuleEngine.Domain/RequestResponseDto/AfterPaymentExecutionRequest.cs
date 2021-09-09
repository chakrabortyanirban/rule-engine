using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RuleEngine.Domain.Models;

namespace RuleEngine.Domain.RequestResponseDto
{
    public class AfterPaymentExecutionRequest
    {
        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("customerId")]
        public int CustomerId { get; set; }

        [JsonProperty("linksForNonPhysicalProducts")]
        public string LinksForNonPhysicalProducts { get; set; } = string.Empty;
    }
}
