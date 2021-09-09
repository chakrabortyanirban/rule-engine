using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Domain.Models
{
    public class CustomerDetails
    {
        public int CustomerId { get; set; }
       
        public string CustomerName { get; set; }
       
        public string ShippingAddress { get; set; }

        public string BillingAddress { get; set; }
        
        public string CustomerEmail { get; set; }
        
        public string ContactNo { get; set; }
    }
}
