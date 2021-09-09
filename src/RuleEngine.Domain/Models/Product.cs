using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RuleEngine.Domain.Models
{
    public class Product
    {
        [JsonProperty("productId")]
        public int ProductId { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("productType")]
        public ProductTypeEnum ProductType { get; set; }

        [JsonProperty("productCategory")]
        public ProductCategoryEnum ProductCategory { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("agentCommissionPercentage")]
        public double AgentCommissionPercentage { get; set; }

        [JsonProperty("relatedFreeProductId")]
        public int RelatedFreeProductId { get; set; }
    }

    public class AllProducts
    {
        [JsonProperty("products")]
        public List<Product> Products { get; set; }

        public Product GetProduct(string productName)
        {
            return this.Products.Find(p => p.Active && p.ProductName.Equals(productName, StringComparison.InvariantCultureIgnoreCase));
        }

        public Product GetProduct(int productId)
        {
            return this.Products.Find(p => p.Active && p.ProductId == productId);
        }
    }
}
