using EligibilityRuleEvaluator.Rules;

namespace EligibilityRuleEvaluator.Models {
    /// <summary>
    /// This is the user applying the rules, used for auditing purposes and for rights, etc.
    /// </summary>
    public class SystemUser {
        public int Id { get; set; }
    }
}
