using Microsoft.AspNetCore.Hosting;
using RuleEngine.Domain.RequestResponseDto;
using RuleEngine.Logic.DbContext;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuleEngine.Logic.RuleActions
{
    internal class PhysicalProductRuleManager
    {
        private readonly AfterPaymentExecutionRequest _request;
        private readonly PhysicalProductPackingSlipManager _slipManager;

        public PhysicalProductRuleManager(AfterPaymentExecutionRequest request, string packingSlipPath, IWebHostEnvironment environment)
        {
            _request = request;
            _slipManager = new PhysicalProductPackingSlipManager(request, packingSlipPath, environment);
        }

        /// <summary>
        /// Consider 10% agent commition. Only commission calculated for simplicity
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ExecuteCommitionPayment()
        {
            var ruleColection = await new RuleCollection().GetRule(_request.ProductName);
            if (ruleColection == null || ruleColection.Price <= 0)
                return false;

            var commitionAmount = ruleColection.Price * 0.1;

            return commitionAmount > 0;
        }

        public async Task<List<string>> GeneratePackingSlips()
        {
            return await _slipManager.Create();
        }

    }
}
