
using EligibilityRuleEvaluator.Models;

namespace EligibilityRuleEvaluator.Rules {
	public interface IEligibilityRule {
		bool IsEligible(RuleContext ruleContext);

		Eligibility_Rule ConvertToEntity();

		int Id { get; }

		string Description { get; }
	}
}