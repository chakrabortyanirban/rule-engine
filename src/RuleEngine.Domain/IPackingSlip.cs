using RuleEngine.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Domain
{
    public interface IPackingSlip
    {
        Task<List<PackingSlip>> GeneratePackaingSlip(bool requiredDuplicate);
    }
}
