namespace EligibilityRuleEvaluator {
	public interface IEligibilityRuleEvaluator<in TContext, out TResult> {
		TResult EvaluateRules(EligibilityRuleSet ruleSet, TContext context);
	}
}