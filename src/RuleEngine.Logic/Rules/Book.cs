using RuleEngine.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuleEngine.Logic.RuleActions;
using RuleEngine.Domain.RequestResponseDto;
using Microsoft.AspNetCore.Hosting;

namespace RuleEngine.Logic.Rules
{
    public class Book : RuleContext, IPackingSlip
    {
        private const bool DUPLICATE_SLIP_REQUIRED = true;
        private readonly PhysicalProductPackingSlipManager _packingSlipManager;

        public Book(AfterPaymentExecutionRequest request, IWebHostEnvironment webHostEnvironment)
        {
            _packingSlipManager = new PhysicalProductPackingSlipManager(request, PackingSlipTemplate, DUPLICATE_SLIP_REQUIRED, webHostEnvironment);
        }

        public string PackingSlipTemplate { get { return "SlipTemplates/PhysicalProduct.html"; } }

        public override async Task<AfterPaymentExecutionResponse> ExecuteAction()
        {
            var response = new AfterPaymentExecutionResponse
            {
                SlipHtml = await GeneratePackaingSlip(DUPLICATE_SLIP_REQUIRED)
            };
            return response;
        }

        public async Task<List<string>> GeneratePackaingSlip(bool requiredDuplicate)
        {
            return await _packingSlipManager.Create();
        }
    }
}
