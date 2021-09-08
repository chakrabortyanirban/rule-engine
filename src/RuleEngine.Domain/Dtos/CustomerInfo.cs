using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Domain.Dtos
{
    public class CustomerDetails
    {
        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("shippingAddress")]
        public string ShippingAddress { get; set; }

        [JsonProperty("paymentDateTime")]
        public DateTime PaymentDateTime { get; set; }

        [JsonProperty("customerEmail")]
        public string CustomerEmail { get; set; }

        [JsonProperty("contactNo")]
        public string ContactNo { get; set; }

        [JsonProperty("ccEmailsIfAny")]
        public List<string> CcEmailsIfAny { get; set; } = null;
    }
}
