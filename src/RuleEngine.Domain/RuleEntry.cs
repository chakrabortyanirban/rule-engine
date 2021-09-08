namespace RuleEngine.Domain
{
    public class RuleEntry
    {
        public string RuleName { get; set; }

        public string AppliedProductFor { get; set; }

        public string Operations { get; set; }

        public bool Active { get; set; } = true;
    }
}
