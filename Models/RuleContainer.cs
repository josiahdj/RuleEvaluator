namespace EligibilityRuleEvaluator.Models {
    /// <summary>
    /// This is the outer boundary within which the rules apply.
    /// e.g. for a multi-tenant application, this would be the tenant.
    /// </summary>
    public class RuleContainer {
        public int Id { get; set; }
    }
}