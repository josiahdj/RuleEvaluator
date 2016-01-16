using EligibilityRuleEvaluator.Rules;

namespace EligibilityRuleEvaluator {
	public class EligibilityRuleResult : IEligibilityResult {
		public EligibilityRuleResult(IEligibilityRule rule, bool isEligible) {
			Rule = rule;
			IsEligible = isEligible;
		}

		public IEligibilityRule Rule { get; private set; }

		public bool IsEligible { get; private set; }
	}
}