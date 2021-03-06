using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuleEngine.Domain
{
    public interface IPackingSlip
    {
        string PackingSlipTemplate { get; }

        Task<List<string>> GeneratePackaingSlip();
    }
}
