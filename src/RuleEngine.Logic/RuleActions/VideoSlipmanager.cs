
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuleEngine.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using RuleEngine.Domain.RequestResponseDto;
using RuleEngine.Logic.DbContext;
using System.IO;

namespace RuleEngine.Logic.RuleActions
{
    public class VideoSlipmanager
    {
        private readonly Product _product;
        private readonly AllProducts _products;
        private readonly string _packingSlipPath;
        private readonly IWebHostEnvironment _environment;
        private readonly AfterPaymentExecutionRequest _request;
        private readonly CustomersCollection _customersCollection;

        private bool isValidRequest;
        private string slipTemplate;
        private string slipHtml;
        private CustomerDetails customerDetails;

        public VideoSlipmanager(AfterPaymentExecutionRequest request, AllProducts products, string packingSlipPath, IWebHostEnvironment environment)
        {
            _products = products;
            _product = products.GetProduct(request.ProductName);
            _request = request;
            _environment = environment;
            _customersCollection = new CustomersCollection();
            _packingSlipPath = packingSlipPath;
        }

        public async Task<List<string>> Create()
        {
            return await Task.Run(() =>
            {
                return ValidateReqquest()
                        .GetCustomerDetails()
                        .CollectTemplate()
                        .GenerateSlipHtml()
                        .CheckForFreeVideoProduct();
            });
        }
        private VideoSlipmanager ValidateReqquest()
        {
            this.isValidRequest = _request != null && _request.CustomerId > 0 && !string.IsNullOrWhiteSpace(_packingSlipPath);
            return this;
        }

        private VideoSlipmanager CollectTemplate()
        {
            if (!this.isValidRequest)
                return this;

            this.slipTemplate = File.ReadAllText(Path.Combine(this._environment.ContentRootPath, _packingSlipPath)); ;
            return this;
        }

        private VideoSlipmanager GetCustomerDetails()
        {
            if (!this.isValidRequest)
                return this;

            this.customerDetails = _customersCollection.GetCustomer(_request.CustomerId);
            return this;
        }

        private VideoSlipmanager GenerateSlipHtml()
        {
            if (!this.isValidRequest)
                return null;

            if (string.IsNullOrWhiteSpace(this.slipTemplate) || customerDetails == null)
            {
                this.isValidRequest = false;
                return null;
            }

            this.slipHtml = GetSlip(false, _product);

            return this;
        }

        private List<string> CheckForFreeVideoProduct()
        {
            if (!this.isValidRequest)
                return null;

            if (string.IsNullOrWhiteSpace(this.slipHtml) || customerDetails == null)
            {
                this.isValidRequest = false;
                return null;
            }

            var slips = new List<string>();
            if (_product.ProductType == ProductTypeEnum.VideoProduct && _product.RelatedFreeProductId > 0)
                this.slipHtml = this.slipHtml.Replace("{{FREEITEMROW}}", AddFreeItemRow(_products.GetProduct(_product.RelatedFreeProductId)));

            this.slipHtml = this.slipHtml.Replace("{{FREEITEMROW}}", string.Empty);
            slips.Add(this.slipHtml);
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

        private string AddFreeItemRow(Product product)
        {
            return $"<tr><td>{product.ProductName}</td><td>{product.ProductId}</td><td>{string.Empty}</td><td>{1}</td><td>Free</td></tr>";
        }
    }
}
