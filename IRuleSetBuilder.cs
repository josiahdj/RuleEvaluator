using EligibilityRuleEvaluator.Models;

namespace EligibilityRuleEvaluator {
    public interface IRuleSetBuilder {
        EligibilityRuleSet BuildRootRuleSet(RuleContainer customer, EligibilityTypeEnum eligibilityType);
    }
}