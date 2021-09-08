using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RuleEngine.Domain.Dtos;
using RuleEngine.Domain.RequestResponse;
using RuleEngine.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RuleEngine.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RuleEngineController : ControllerBase
    {
        private readonly ILogger<RuleEngineController> _logger;

        public RuleEngineController(ILogger<RuleEngineController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("PostPaymentExecutions")]
        public async Task<ActionResult<List<PackingSlip>>> PostPaymentWorkExecutions(AfterPaymentExecutionRequest request)
        {
            // Request validation
            if (string.IsNullOrWhiteSpace(request?.ProductName))
                throw new ArgumentException("Request object is null or required parameter meesing.");

            // Find applicable rule 
            var ruleCollection = new RuleCollection();
            var ruleName = await ruleCollection.GetRule(request.ProductName);

            if (string.IsNullOrWhiteSpace(ruleName))
                throw new ArgumentException("Invalid product name! Attached rule is missing.");

            // Get Rule instance
            var assembly = Assembly.GetAssembly(ruleCollection.GetType());
            var t = assembly?.GetType(assembly.GetName().Name + ".Rules." + ruleName);
            var ruleConstructor = t.GetConstructor(new Type[] { typeof(AfterPaymentExecutionRequest) });
            var ruleObject = ruleConstructor.Invoke(new object[] { request });

            // Get Execution Method 
            MethodInfo method
                 = t.GetMethod("ExecuteAction");

            //var xx = method.MakeGenericMethod(dynamic);

            // Execute action
            var response = await (Task<List<PackingSlip>>)method.Invoke(ruleObject, null);
            return response;
        }
    }
}
