using RuleEngine.Domain;
using RuleEngine.Domain.Dtos;
using RuleEngine.Domain;
using RuleEngine.Domain.RequestResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RuleEngine.Logic.RuleActions;

namespace RuleEngine.Logic.Rules
{
    public class Book : RuleContext<List<PackingSlip>>, IPackingSlip
    {
        private const bool DUPLICATE_SLIP_REQUIRED = true;
        private readonly AfterPaymentExecutionRequest _request;
        private readonly PackingSlipManager _packingSlipManager;

        public Book(AfterPaymentExecutionRequest request)
        {
            _request = request;
            _packingSlipManager = new PackingSlipManager(request, DUPLICATE_SLIP_REQUIRED);
        }

        public override async Task<List<PackingSlip>> ExecuteAction()
        {
            return await GeneratePackaingSlip(DUPLICATE_SLIP_REQUIRED);
        }

        public async Task<List<PackingSlip>> GeneratePackaingSlip(bool requiredDuplicate)
        {
            return await _packingSlipManager.Create();
        }
    }
}
