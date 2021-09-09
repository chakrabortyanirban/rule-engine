using RuleEngine.Domain.RequestResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Test.Helpers
{
    internal class RequestBuilder
    {
        public static AfterPaymentExecutionRequest InvalidRequestEmptyObject()
        {
            return null;
        }

        public static AfterPaymentExecutionRequest InvalidRequestWrongProductName()
        {
            return new AfterPaymentExecutionRequest { ProductName = "abcProduct" };
        }

        public static AfterPaymentExecutionRequest ValidRequestForBookPurchase(int customerId , string product)
        {
            return new AfterPaymentExecutionRequest
            {
                ProductName = product,
                CustomerId = customerId,
                LinksForNonPhysicalProducts = null
        };
    }
}
}
