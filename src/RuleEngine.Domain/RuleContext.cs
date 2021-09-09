using RuleEngine.Domain.RequestResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Domain
{
    public abstract class RuleContext
    {
        public abstract Task<AfterPaymentExecutionResponse> ExecuteAction();
    }
}
