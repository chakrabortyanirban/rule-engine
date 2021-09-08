using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using RuleEngine.Controllers;
using RuleEngine.Domain.RequestResponse;
using RuleEngine.Test.Helpers;

namespace RuleEngine.Test
{
    [TestFixture]
    public class RuleEngineTest
    {
        private Mock<ILogger<RuleEngineController>> _logger;
        private RuleEngineController _controller;

        [SetUp]
        public void Initialize()
        {
            _logger = new Mock<ILogger<RuleEngineController>>();
            _controller = new RuleEngineController(_logger.Object);
        }

        [Test]
        [TestCase(1, "EmptyRequest", "Request object is null or required parameter meesing.")]
        [TestCase(2, "InvalidProduct", "Invalid product name! Attached rule is missing.")]
        public async Task InvalidInput(int caseId, string caseName, string expectedMsg)
        {
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _controller.PostPaymentWorkExecutions(GetRequestObject(caseName)));
            Assert.That(ex.Message, caseId == 1 ? Is.EqualTo(expectedMsg) : caseId == 2 ? Is.EqualTo(expectedMsg) : Is.EqualTo("not expected outcome"));
        }

        [Test]
        [TestCase(1, "GetSlipForBookPurchase")]
        public async Task GetSlipForBookPurchase(int caseId, string caseName)
        {
            var reqObj = GetRequestObject(caseName);
            var response = await _controller.PostPaymentWorkExecutions(reqObj);
            Assert.AreEqual(response?.Value.Count, 2);

            var customerCopy = response.Value[0];

            Assert.AreEqual(customerCopy.Address, reqObj.ShippingAddress);
            Assert.AreEqual(customerCopy.ContactNumber, reqObj.ContactNo);
            Assert.AreEqual(customerCopy.CustomerName, reqObj.CustomerName);
            Assert.AreEqual(customerCopy.IsCustomerCopy, true);
        }

        private AfterPaymentExecutionRequest GetRequestObject(string caseName)
        {
            return caseName switch
            {
                "EmptyRequest" => RequestBuilder.InvalidRequestEmptyObject(),
                "InvalidProduct" => RequestBuilder.InvalidRequestWrongProductName(),
                "GetSlipForBookPurchase" => RequestBuilder.ValidRequestForBookPurchase(),
                _ => null,
            };
        }
    }
}