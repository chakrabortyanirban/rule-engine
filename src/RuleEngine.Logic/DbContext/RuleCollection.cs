using RuleEngine.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
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
                new RuleEntry{RuleName="1", AppliedProductFor="Book", Operations="Book" , Active = true },
                new RuleEntry{RuleName="2", AppliedProductFor="NewMembership", Operations="Membership", Active = true },
                new RuleEntry{RuleName="3", AppliedProductFor="UpgradeMembership", Operations="Upgrade", Active = false },
                new RuleEntry{RuleName="4", AppliedProductFor="Video", Operations="Video", Active = false },
            };
        }

        public async Task<string> GetRule(string product)
        {
            return await Task.Run(() => Rules.Where(r => r.Active && r.AppliedProductFor.Equals(product, StringComparison.InvariantCultureIgnoreCase))?.FirstOrDefault()?.Operations);
        }
    }
}
