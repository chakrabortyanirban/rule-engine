using System.Threading.Tasks;
using RuleEngine.Domain.RequestResponseDto;

namespace RuleEngine.Domain
{
    public abstract class RuleContext
    {
        public abstract Task<AfterPaymentExecutionResponse> ExecuteAction();
    }
}
