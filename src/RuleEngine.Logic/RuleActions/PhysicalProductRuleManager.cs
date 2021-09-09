using Microsoft.AspNetCore.Hosting;
using RuleEngine.Domain.Models;
using RuleEngine.Domain.RequestResponseDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuleEngine.Logic.RuleActions
{
    internal class PhysicalProductRuleManager
    {
        private readonly AllProducts _products;
        private readonly AfterPaymentExecutionRequest _request;
        private readonly PhysicalProductPackingSlipManager _slipManager;

        public PhysicalProductRuleManager(AfterPaymentExecutionRequest request, AllProducts products, string packingSlipPath, IWebHostEnvironment environment)
        {
            _products = products;
            _request = request;
            _slipManager = new PhysicalProductPackingSlipManager(request, products, packingSlipPath, environment);
        }

        public async Task<bool> ExecuteCommitionPayment()
        {
            return await Task.Run(() =>
            {
                var product = _products.GetProduct(_request.ProductName);
                if (product == null || product.Price <= 0)
                    return false;

                var commitionAmount = product.Price * (product.AgentCommissionPercentage / 100);

                return commitionAmount > 0;
            });
        }

        public async Task<List<string>> GeneratePackingSlips()
        {
            return await _slipManager.Create();
        }

    }
}
