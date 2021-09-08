using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RuleEngine.Domain.RequestResponse;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ActionResult<AfterPaymentExecutionResponse>> PostPaymentWorkExecutions(AfterPaymentExecutionRequest request)
        {
            // Request validation
            if (string.IsNullOrWhiteSpace(request?.ProductName))
                throw new ArgumentException("Request object is null or required parameter meesing.");

            return null;
        }
    }
}
