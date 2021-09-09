using Microsoft.AspNetCore.Hosting;
using RuleEngine.Domain;
using RuleEngine.Domain.RequestResponseDto;
using RuleEngine.Logic.RuleActions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuleEngine.Logic.RuleDefinations
{
    public class MembershipProduct : RuleContext, IPackingSlip
    {
        private readonly MembershipSlipManager _packingSlipManager;

        public MembershipProduct(AfterPaymentExecutionRequest request, IWebHostEnvironment webHostEnvironment)
        {
            _packingSlipManager = new MembershipSlipManager(request, request.ProductName.Contains("upgrade", System.StringComparison.CurrentCultureIgnoreCase));
        }

        public string PackingSlipTemplate { get { return "SlipTemplates/Membership.html"; } } // not in use in this scenario

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
            return await _packingSlipManager.Create();
        }
    }
}
