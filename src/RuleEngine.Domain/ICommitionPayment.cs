using System.Threading.Tasks;

namespace RuleEngine.Domain
{
    public interface ICommitionPayment
    {
        string PackingSlipTemplate { get; }

        Task<bool> InitiateComitionPaymentForAgent();
    }
}
