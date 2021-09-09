using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using RuleEngine.Controllers;
using RuleEngine.Domain.RequestResponseDto;
using RuleEngine.Test.Helpers;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using RuleEngine.Domain.Models;
using Microsoft.Extensions.Options;

namespace RuleEngine.Test
{
    [TestFixture]
    public class RuleEngineTest
    {
        private Mock<ILogger<RuleEngineController>> _logger;
        private RuleEngineController _controller;
        private Mock<IWebHostEnvironment> _environment;
        private Mock<IOptions<AllProducts>> _allProducts;
        private readonly string _testDirectoryPath;
        private readonly string _contentRootPath;

        public RuleEngineTest()
        {
            _testDirectoryPath = TestContext.CurrentContext.TestDirectory;
            _contentRootPath = _testDirectoryPath.Replace("RuleEngine.Test", "RuleEngine");
        }

        [SetUp]
        public void Initialize()
        {
            _logger = new Mock<ILogger<RuleEngineController>>();
            _allProducts = new Mock<IOptions<AllProducts>>();
            _environment = new Mock<IWebHostEnvironment>();
            _environment.SetupGet(x => x.ContentRootPath).Returns(_contentRootPath);

            var productList = new ResponseReaderHelper(_contentRootPath +"/SlipTemplates/Product.json").GetMockResponseFromJsonFile();
            _allProducts.SetupGet(x => x.Value).Returns(JsonConvert.DeserializeObject<AllProducts>(productList));

            _controller = new RuleEngineController(_logger.Object, _environment.Object, _allProducts.Object);
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
        [TestCase(3, "GetSlipForBookPurchase", 1, "The War of the poor")]
        [TestCase(4, "GetSlipForBookPurchase", 2, "The art of Reading Minds")]
        public async Task GetSlipForBookPurchase(int caseId, string caseName, int customerId, string product)
        {
            var expectedOutput = new ResponseReaderHelper(_testDirectoryPath + "/CaseResponses/" + caseId + "/ExpectedResponse.json").GetMockResponseFromJsonFile();
            var reqObj = GetRequestObject(caseName, customerId, product);
            var response = await _controller.PostPaymentWorkExecutions(reqObj);
            Assert.AreEqual(response?.Value.SlipHtml.Count, 2);

            var finalOutput = Newtonsoft.Json.JsonConvert.SerializeObject(response.Value, Newtonsoft.Json.Formatting.Indented);
            ContentAssert.JsonAreEquivalents(expectedOutput, finalOutput);
        }

        [Test]
        [TestCase(5, "GetSlipForMembershipPurchase", 1, "Membership")]
        public async Task GetSlipForMembershipPurchase(int caseId, string caseName, int customerId, string product)
        {
            var expectedOutput = new ResponseReaderHelper(_testDirectoryPath + "/CaseResponses/" + caseId + "/ExpectedResponse.json").GetMockResponseFromJsonFile();
            var reqObj = GetRequestObject(caseName, customerId, product);
            var response = await _controller.PostPaymentWorkExecutions(reqObj);
            Assert.AreEqual(response?.Value.SlipHtml.Count, 1);

            var finalOutput = Newtonsoft.Json.JsonConvert.SerializeObject(response.Value);
            ContentAssert.JsonAreEquivalents(expectedOutput, finalOutput);
        }

        [Test]
        [TestCase(6, "GetSlipForMembershipUpgradePurchase", 2, "Upgrade")]
        public async Task GetSlipForMembershipUpgradePurchase(int caseId, string caseName, int customerId, string product)
        {
            var expectedOutput = new ResponseReaderHelper(_testDirectoryPath + "/CaseResponses/" + caseId + "/ExpectedResponse.json").GetMockResponseFromJsonFile();
            var reqObj = GetRequestObject(caseName, customerId, product);
            var response = await _controller.PostPaymentWorkExecutions(reqObj);
            Assert.AreEqual(response?.Value.SlipHtml.Count, 1);

            var finalOutput = Newtonsoft.Json.JsonConvert.SerializeObject(response.Value);
            ContentAssert.JsonAreEquivalents(expectedOutput, finalOutput);
        }

        [Test]
        [TestCase(7, "GetSlipForVideoPurchase", 2, "Learning to Ski")]
        public async Task GetSlipForVideoPurchase(int caseId, string caseName, int customerId, string product)
        {
            var expectedOutput = new ResponseReaderHelper(_testDirectoryPath + "/CaseResponses/" + caseId + "/ExpectedResponse.json").GetMockResponseFromJsonFile();
            var reqObj = GetRequestObject(caseName, customerId, product);
            var response = await _controller.PostPaymentWorkExecutions(reqObj);
            Assert.AreEqual(response?.Value.SlipHtml.Count, 1);

            var finalOutput = Newtonsoft.Json.JsonConvert.SerializeObject(response.Value);
            ContentAssert.JsonAreEquivalents(expectedOutput, finalOutput);
        }

        private AfterPaymentExecutionRequest GetRequestObject(string caseName, int customerId, string product)
        {
            return caseName switch
            {
                "EmptyRequest" => RequestBuilder.InvalidRequestEmptyObject(),
                "InvalidProduct" => RequestBuilder.InvalidRequestWrongProductName(),
                "GetSlipForBookPurchase" => RequestBuilder.ValidRequestForProductPurchase(customerId, product),
                "GetSlipForMembershipPurchase" => RequestBuilder.ValidRequestForProductPurchase(customerId, product),
                "GetSlipForMembershipUpgradePurchase" => RequestBuilder.ValidRequestForProductPurchase(customerId, product),
                "GetSlipForVideoPurchase" => RequestBuilder.ValidRequestForProductPurchase(customerId, product),
                _ => null,
            };
        }
    }
}