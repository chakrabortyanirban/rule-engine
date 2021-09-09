using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using RuleEngine.Controllers;
using RuleEngine.Domain.RequestResponseDto;
using RuleEngine.Test.Helpers;
using Microsoft.AspNetCore.Hosting;

namespace RuleEngine.Test
{
    [TestFixture]
    public class RuleEngineTest
    {
        private Mock<ILogger<RuleEngineController>> _logger;
        private RuleEngineController _controller;
        private Mock<IWebHostEnvironment> _environment;
        private readonly string _testDirectoryPath;
        

        public RuleEngineTest()
        {
            _testDirectoryPath = TestContext.CurrentContext.TestDirectory;
        }

        [SetUp]
        public void Initialize()
        {
            _logger = new Mock<ILogger<RuleEngineController>>();

            _environment = new Mock<IWebHostEnvironment>();
            _environment.SetupGet(x => x.ContentRootPath).Returns(_testDirectoryPath.Replace("RuleEngine.Test", "RuleEngine"));

            _controller = new RuleEngineController(_logger.Object, _environment.Object);
        }

        [Test]
        [TestCase(1, "EmptyRequest", "Request object is null or required parameter meesing.")]
        [TestCase(2, "InvalidProduct", "Invalid product name! Attached rule is missing.")]
        public async Task InvalidInput(int caseId, string caseName, string expectedMsg)
        {
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _controller.PostPaymentWorkExecutions(GetRequestObject(caseName, 0, string.Empty)));
            Assert.That(ex.Message, caseId == 1 ? Is.EqualTo(expectedMsg) : caseId == 2 ? Is.EqualTo(expectedMsg) : Is.EqualTo("not expected outcome"));
        }

        [Test]
        [TestCase(1, "GetSlipForBookPurchase", 1, "book")]
        [TestCase(2, "GetSlipForBookPurchase", 2, "book")]
        public async Task GetSlipForBookPurchase(int caseId, string caseName, int customerId, string product)
        {
            var expectedOutput = new ResponseReaderHelper(_testDirectoryPath + "/CaseResponses/" + caseId + "/ExpectedResponse.json").GetMockResponseFromJsonFile();
            var reqObj = GetRequestObject(caseName, customerId, product);
            var response = await _controller.PostPaymentWorkExecutions(reqObj);
            Assert.AreEqual(response?.Value.SlipHtml.Count, 2);

            var finalOutput = Newtonsoft.Json.JsonConvert.SerializeObject(response.Value);
            ContentAssert.JsonAreEquivalents(expectedOutput, finalOutput);
        }

        private AfterPaymentExecutionRequest GetRequestObject(string caseName, int customerId, string product)
        {
            return caseName switch
            {
                "EmptyRequest" => RequestBuilder.InvalidRequestEmptyObject(),
                "InvalidProduct" => RequestBuilder.InvalidRequestWrongProductName(),
                "GetSlipForBookPurchase" => RequestBuilder.ValidRequestForBookPurchase(customerId, product),
                _ => null,
            };
        }
    }
}