using Microsoft.AspNetCore.Hosting;
using RuleEngine.Domain.Models;
using RuleEngine.Domain.RequestResponseDto;
using RuleEngine.Logic.DbContext;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RuleEngine.Logic.RuleActions
{
    internal class PhysicalProductPackingSlipManager
    {
        private readonly AfterPaymentExecutionRequest _request;
        private readonly bool _duplicateSlipRequired;
        private readonly string _packingSlipPath;
        private readonly IWebHostEnvironment _environment;
        private readonly CustomersCollection _customersCollection;


        private bool isValidRequest;
        private string slipTemplate;
        private CustomerDetails customerDetails;

        public PhysicalProductPackingSlipManager(AfterPaymentExecutionRequest request, string packingSlipPath, bool duplicateSlipRequired, IWebHostEnvironment environment)
        {
            _request = request;
            _environment = environment;
            _packingSlipPath = packingSlipPath;
            _customersCollection = new CustomersCollection();
            _duplicateSlipRequired = duplicateSlipRequired;
        }

        public async Task<List<string>> Create()
        {
            return await Task.Run(() =>
            {
                return ValidateReqquest()
                        .GetCustomerDetails()
                        .CollectTemplate()
                        .GenerateSlipHtml();
            });
        }

        private PhysicalProductPackingSlipManager ValidateReqquest()
        {
            this.isValidRequest = _request != null && _request.CustomerId > 0 && !string.IsNullOrWhiteSpace(_packingSlipPath);
            return this;
        }

        private PhysicalProductPackingSlipManager CollectTemplate()
        {
            if (!this.isValidRequest)
                return this;

            this.slipTemplate = File.ReadAllText(Path.Combine(this._environment.ContentRootPath, _packingSlipPath)); ;
            return this;
        }

        private PhysicalProductPackingSlipManager GetCustomerDetails()
        {
            if (!this.isValidRequest)
                return this;

            this.customerDetails = _customersCollection.GetCustomer(_request.CustomerId);
            return this;
        }

        private List<string> GenerateSlipHtml()
        {
            if (!this.isValidRequest)
                return null;

            if (string.IsNullOrWhiteSpace(this.slipTemplate) || customerDetails == null)
            {
                this.isValidRequest = false;
                return null;
            }

            var slips = new List<string>() {
                GetSlip(false)
            };

            if (_duplicateSlipRequired)
                slips.Add(GetSlip(true));

            return slips;
        }

        private string GetSlip(bool isDuplicate)
        {
            var slip = slipTemplate.Replace("{{DATE}}", DateTime.Now.ToShortDateString())
            .Replace("{{CUSTOMER_COPY}}", isDuplicate ? "Internal Copy" : "Customer Copy")
            .Replace("{{BILL_TO}}", customerDetails.BillingAddress)
            .Replace("{{SHIP_TO}}", customerDetails.ShippingAddress)
            .Replace("{{ITEM}}", customerDetails.ShippingAddress)
            .Replace("{{SKU}}", customerDetails.ShippingAddress)
            .Replace("{{DESCRIPTION}}", customerDetails.ShippingAddress)
            .Replace("{{QUANTITY}}", customerDetails.ShippingAddress)
            .Replace("{{PRICE}}", customerDetails.ShippingAddress);
            return slip;
        }
    }
}
