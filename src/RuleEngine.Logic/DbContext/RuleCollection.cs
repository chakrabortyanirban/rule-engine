using RuleEngine.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using RuleEngine.Domain.Models;
using System.Threading.Tasks;

namespace RuleEngine.Logic.DbContext
{

    /// <summary>
    ///  This class should populate from DB and controled by some admin UI interface. 
    ///  For simplicty considering all rules and a getter is part of this rule collection class.
    /// </summary>
    public class RuleCollection
    {
        private readonly IEnumerable<RuleEntry> Rules;

        public RuleCollection()
        {
            Rules = new List<RuleEntry>()
            {
                new RuleEntry{RuleName="1", ProductType=ProductTypeEnum.PhysicalProduct, Product="Book" , Price =100,  Active = true },
                new RuleEntry{RuleName="2", ProductType= ProductTypeEnum.MembershipProduct, Product="Membership", Price =50, Active = true },
                new RuleEntry{RuleName="3", ProductType=ProductTypeEnum.MembershipProduct, Product="Upgrade", Price = 10, Active = true },
                new RuleEntry{RuleName="4", ProductType=ProductTypeEnum.VideoProduct, Product="Video", Price = 5, Active = false },
            };
        }

        public async Task<RuleEntry> GetRule(string product)
        {
            return await Task.Run(() => Rules.Where(r => r.Active && r.Product.Equals(product, StringComparison.InvariantCultureIgnoreCase))?.FirstOrDefault());
        }
    }
}
