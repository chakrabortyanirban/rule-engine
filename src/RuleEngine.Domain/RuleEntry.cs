using RuleEngine.Domain.Models;

namespace RuleEngine.Domain
{
    public class RuleEntry
    {
        public string RuleName { get; set; }

        public ProductTypeEnum ProductType { get; set; }

        public string Product { get; set; }

        public double Price { get; set; }

        public bool Active { get; set; } = true;
    }
}
