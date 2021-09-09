using Newtonsoft.Json;

namespace RuleEngine.Domain.RequestResponseDto
{
    public class AfterPaymentExecutionRequest
    {
        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("customerId")]
        public int CustomerId { get; set; }
    }
}
