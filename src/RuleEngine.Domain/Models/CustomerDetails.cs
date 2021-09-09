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

        public bool IsActiveMember { get; set; }

        public int MembershipSlot { get; set; } // 0 = Trial 1= basic 2 Premimum 3 = enterprise
    }
}
