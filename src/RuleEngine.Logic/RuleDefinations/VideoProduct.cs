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
    public class VideoProduct : RuleContext, IPackingSlip
    {
        private readonly VideoSlipmanager _videoSlipmanager;

        public VideoProduct(AfterPaymentExecutionRequest request, AllProducts products, IWebHostEnvironment webHostEnvironment)
        {
            _videoSlipmanager = new VideoSlipmanager(request, products, PackingSlipTemplate, webHostEnvironment);
        }

        public string PackingSlipTemplate { get { return "SlipTemplates/Video.html"; } }

        public override async Task<AfterPaymentExecutionResponse> ExecuteAction()
        {
            var response = new AfterPaymentExecutionResponse
            {
                SlipHtml = await GeneratePackaingSlip()
            };
            return response;
        }

        public async Task<List<string>> GeneratePackaingSlip()
        {
            return await _videoSlipmanager.Create();
        }
    }
}
