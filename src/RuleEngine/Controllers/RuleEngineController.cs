using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RuleEngine.Domain.Models;
using RuleEngine.Logic.DbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuleEngine.Domain.RequestResponseDto;

namespace RuleEngine.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RuleEngineController : ControllerBase
    {
        private readonly ILogger<RuleEngineController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly AllProducts _allProducts;

        public RuleEngineController(ILogger<RuleEngineController> logger, IWebHostEnvironment environment, IOptions<AllProducts> allProducts)
        {
            _environment = environment;
            _logger = logger;
            _allProducts = new AllProducts { Products = allProducts.Value.Products.Where(p => p.Active).ToList() };
        }

        [HttpPost]
        [Route("PostPaymentExecutions")]
        public async Task<ActionResult<AfterPaymentExecutionResponse>> PostPaymentWorkExecutions(AfterPaymentExecutionRequest request)
        {
            // Request validation
            if (string.IsNullOrWhiteSpace(request?.ProductName) || request.CustomerId <= 0)
                throw new ArgumentException("Request object is null or required parameter meesing.");

            // Find actual product with rule
            var product = _allProducts.GetProduct(request.ProductName);

            if (string.IsNullOrWhiteSpace(product?.ProductName))
                throw new ArgumentException("Invalid product name! Attached rule is missing.");

            // Get Rule instance
            var assembly = Assembly.GetAssembly(typeof(CustomersCollection));
            var ruleType = assembly?.GetType("RuleEngine.Logic.RuleDefinations." + product.ProductType.ToString());
            var ruleConstructor = ruleType.GetConstructor(new Type[] { typeof(AfterPaymentExecutionRequest), typeof(AllProducts), typeof(IWebHostEnvironment) });
            var ruleObject = ruleConstructor.Invoke(new object[] { request, _allProducts, _environment });

            // Get Execution Method 
            MethodInfo method
                 = ruleType.GetMethod("ExecuteAction");

            // Execute action
            var response = await (Task<AfterPaymentExecutionResponse>)method.Invoke(ruleObject, null);
            return response;
        }
    }
}
