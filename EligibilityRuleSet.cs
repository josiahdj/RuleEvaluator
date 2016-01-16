using System.Collections.Generic;

using EligibilityRuleEvaluator.Rules;

namespace EligibilityRuleEvaluator {
	public class EligibilityRuleSet {
		public EligibilityRuleSet() {
			Aggregator = EligibilityRuleSetAggregator.Unknown;
			EligibilityType = EligibilityTypeEnum.Unknown;
			
			Rules = new List<IEligibilityRule>();
			ChildRuleSets = new List<EligibilityRuleSet>();
		}

		public int Id { get; set; }

		public EligibilityRuleSetAggregator Aggregator { get; set; }

		public List<IEligibilityRule> Rules { get; set; }

		public List<EligibilityRuleSet> ChildRuleSets { get; set; }

		public EligibilityTypeEnum EligibilityType { get; set; }
	}

}