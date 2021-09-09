using Microsoft.AspNetCore.Hosting;
using RuleEngine.Domain;
using RuleEngine.Domain.Models;
using RuleEngine.Domain.RequestResponseDto;
using RuleEngine.Logic.RuleActions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuleEngine.Logic.RuleDefinations
{
    public class PhysicalProduct : RuleContext, IPackingSlip, ICommitionPayment
    {
        private readonly PhysicalProductRuleManager _physicalProductRuleManager;

        public PhysicalProduct(AfterPaymentExecutionRequest request, AllProducts products, IWebHostEnvironment webHostEnvironment)
        {
            _physicalProductRuleManager = new PhysicalProductRuleManager(request, products, PackingSlipTemplate, webHostEnvironment);
        }

        public string PackingSlipTemplate { get { return "SlipTemplates/PhysicalProduct.html"; } }

        public override async Task<AfterPaymentExecutionResponse> ExecuteAction()
        {
            var response = new AfterPaymentExecutionResponse
            {
                SlipHtml = await GeneratePackaingSlip()
            };
            _ = await InitiateComitionPaymentForAgent();
            return response;
        }

        public async Task<List<string>> GeneratePackaingSlip()
        {
            return await _physicalProductRuleManager.GeneratePackingSlips();
        }

        public async Task<bool> InitiateComitionPaymentForAgent()
        {
            return await _physicalProductRuleManager.ExecuteCommitionPayment();
        }
    }
}
