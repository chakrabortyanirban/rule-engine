using System.Linq;
using RuleEngine.Domain.Models;
using System.Collections.Generic;

namespace RuleEngine.Logic.DbContext
{
    public class CustomersCollection
    {
        private readonly List<CustomerDetails> _customers;

        public CustomersCollection()
        {
            _customers = GetAllCustomers();
        }

        private static List<CustomerDetails> GetAllCustomers()
        {
            var customers = new List<CustomerDetails>();
            for (int i = 1; i <= 10; i++)
            {
                customers.Add(new CustomerDetails
                {
                    CustomerId = i,
                    ShippingAddress = "Shipping address " + i,
                    BillingAddress = "Billing address " + i,
                    ContactNo = i + "99999",
                    CustomerEmail = i + "@gmail.com",
                    CustomerName = "Customer " + i,
                    IsActiveMember = false,
                    MembershipSlot = 0
                });
            }

            return customers;
        }

        public CustomerDetails GetCustomer(int id)
        {
            return _customers.FirstOrDefault(c => c.CustomerId == id);
        }
    }
}
