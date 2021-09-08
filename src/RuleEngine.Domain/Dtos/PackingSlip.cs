using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Domain.Dtos
{
    public class PackingSlip
    {
        public string CustomerName { get; set; }

        public string Address { get; set; }

        public string ContactNumber { get; set; }

        public byte[] ProductQrCode { get; set; }  // TODO 

        public bool IsCustomerCopy { get; set; }
    }
}
