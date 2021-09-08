using RuleEngine.Domain.Dtos;
using RuleEngine.Domain.RequestResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Logic.RuleActions
{
    internal class PackingSlipManager
    {
        private readonly AfterPaymentExecutionRequest _request;
        private readonly bool _duplicateSlipRequired;

        public PackingSlipManager(AfterPaymentExecutionRequest request, bool duplicateSlipRequired)
        {
            _request = request;
            _duplicateSlipRequired = duplicateSlipRequired;
        }

        public async Task<List<PackingSlip>> Create()
        {
            var slips = new List<PackingSlip>() {
                await GenerateSlip(false)
            };

            if (_duplicateSlipRequired)
                slips.Add(await GenerateSlip(true));

            return slips;
        }

        private async Task<PackingSlip> GenerateSlip(bool isDuplicate)
        {
            return await Task.Run(() =>
            {
                return new PackingSlip
                {
                    Address = _request.ShippingAddress,
                    ContactNumber = _request.ContactNo,
                    CustomerName = _request.CustomerName,
                    IsCustomerCopy = !isDuplicate,
                    ProductQrCode = null // TODO: nice to haave
                };
            });
        }


    }
}
