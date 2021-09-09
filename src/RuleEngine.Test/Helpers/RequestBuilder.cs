using RuleEngine.Domain.RequestResponseDto;

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
            return new AfterPaymentExecutionRequest { ProductName = "abcProduct", CustomerId = 2 };
        }

        public static AfterPaymentExecutionRequest ValidRequestForProductPurchase(int customerId, string product)
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
