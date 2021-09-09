using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RuleEngine.Domain.RequestResponseDto;
using RuleEngine.Logic.DbContext;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace RuleEngine.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RuleEngineController : ControllerBase
    {
        private readonly ILogger<RuleEngineController> _logger;
        private readonly IWebHostEnvironment _environment;

        public RuleEngineController(ILogger<RuleEngineController> logger, IWebHostEnvironment environment)
        {
            _environment = environment;
            _logger = logger;
        }

        [HttpPost]
        [Route("PostPaymentExecutions")]
        public async Task<ActionResult<AfterPaymentExecutionResponse>> PostPaymentWorkExecutions(AfterPaymentExecutionRequest request)
        {
            // Request validation
            if (string.IsNullOrWhiteSpace(request?.ProductName) || request.CustomerId <= 0)
                throw new ArgumentException("Request object is null or required parameter meesing.");

            // Find applicable rule 
            var ruleCollection = new RuleCollection();
            var ruleEntry = await ruleCollection.GetRule(request.ProductName);

            if (string.IsNullOrWhiteSpace(ruleEntry?.Product))
                throw new ArgumentException("Invalid product name! Attached rule is missing.");

            // Get Rule instance
            var assembly = Assembly.GetAssembly(ruleCollection.GetType());
            var ruleType = assembly?.GetType(assembly.GetName().Name + ".RuleDefinations." + ruleEntry.ProductType.ToString());
            var ruleConstructor = ruleType.GetConstructor(new Type[] { typeof(AfterPaymentExecutionRequest), typeof(IWebHostEnvironment) });
            var ruleObject = ruleConstructor.Invoke(new object[] { request, _environment });

            // Get Execution Method 
            MethodInfo method
                 = ruleType.GetMethod("ExecuteAction");

            // Execute action
            var response = await (Task<AfterPaymentExecutionResponse>)method.Invoke(ruleObject, null);
            return response;
        }
    }
}
