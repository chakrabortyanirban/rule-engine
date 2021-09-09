using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using RuleEngine.Domain.Models;
using RuleEngine.Domain.RequestResponseDto;
using RuleEngine.Logic.DbContext;

namespace RuleEngine.Logic.RuleActions
{
    internal class PhysicalProductPackingSlipManager
    {
        private readonly AfterPaymentExecutionRequest _request;
        private readonly bool _duplicateSlipRequired;
        private readonly string _packingSlipPath;
        private readonly Product _product;
        private readonly IWebHostEnvironment _environment;
        private readonly CustomersCollection _customersCollection;


        private bool isValidRequest;
        private string slipTemplate;
        private CustomerDetails customerDetails;

        public PhysicalProductPackingSlipManager(AfterPaymentExecutionRequest request, AllProducts products, string packingSlipPath, IWebHostEnvironment environment)
        {
            _request = request;
            _product = products.GetProduct(request.ProductName);
            _environment = environment;
            _packingSlipPath = packingSlipPath;
            _customersCollection = new CustomersCollection();
            _duplicateSlipRequired = _product.ProductType == ProductTypeEnum.PhysicalProduct;
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
                GetSlip(false, _product)
            };

            if (_duplicateSlipRequired)
                slips.Add(GetSlip(true, _product));

            return slips;
        }

        private string GetSlip(bool isDuplicate, Product product)
        {
            var slip = slipTemplate.Replace("{{DATE}}", DateTime.Now.ToShortDateString())
            .Replace("{{CUSTOMER_COPY}}", isDuplicate ? "Internal Copy" : "Customer Copy")
            .Replace("{{BILL_TO}}", customerDetails.BillingAddress)
            .Replace("{{SHIP_TO}}", customerDetails.ShippingAddress)
            .Replace("{{ITEM}}", product?.ProductName)
            .Replace("{{SKU}}", product?.ProductId.ToString())
            .Replace("{{DESCRIPTION}}", string.Empty)
            .Replace("{{QUANTITY}}", "1")
            .Replace("{{PRICE}}", product.Price.ToString());
            return slip;
        }
    }
}
