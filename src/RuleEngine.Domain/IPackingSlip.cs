using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Domain
{
    public interface IPackingSlip
    {
        string PackingSlipTemplate { get; }

        Task<List<string>> GeneratePackaingSlip(bool requiredDuplicate);
    }
}
