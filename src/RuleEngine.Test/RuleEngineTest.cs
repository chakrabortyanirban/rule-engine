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
        public async Task InvalidInput(int caseId, string caseName, string expectedMsg)
        {
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _controller.PostPaymentWorkExecutions(GetRequestObject(caseName)));
            Assert.That(ex.Message, Is.EqualTo(expectedMsg));
        }

        private AfterPaymentExecutionRequest GetRequestObject(string caseName)
        {
            return caseName switch
            {
                "EmptyRequest" => RequestBuilder.InvalidRequestEmptyObject(),
                _ => null,
            };
        }
    }
}