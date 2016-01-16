using System.Collections.Generic;

namespace EligibilityRuleEvaluator {
	public class EligibilityResult : IEligibilityResult {
		public EligibilityResult(bool isEligible, EligibilityRuleSetAggregator aggregator) {
			Aggregator = aggregator;
			IsEligible = isEligible;
			RuleResults = new List<EligibilityRuleResult>();
			RuleSetResults = new List<EligibilityResult>();
		}

		public EligibilityRuleSetAggregator Aggregator { get; private set; }
		public bool IsEligible { get; protected internal set; }

		public EligibilityPolicyOverride Override { get; set; }

		public List<EligibilityRuleResult> RuleResults { get; private set; }
		public List<EligibilityResult> RuleSetResults { get; private set; }
	}

	public interface IEligibilityResult {
		bool IsEligible { get; }
	}
}