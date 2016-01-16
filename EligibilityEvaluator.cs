using System;

using EligibilityRuleEvaluator.Models;

namespace EligibilityRuleEvaluator {
    /// <summary>
    /// This class is the library's composition root, or the class you'd use in an application to evaluate rules.
    /// You'd pass in instances of the dependencies and then call GetEligibilityInfo on a given ruleContext 
    /// (or set of contextual data) for a given type of eligibility.
    /// </summary>
    public class EligibilityEvaluator {
		private readonly IEligibilityRuleEvaluator<RuleContext, EligibilityResult> _ruleEvaluator;
		private readonly IRuleSetBuilder _ruleSetBuilder;
		private readonly IEligibilityPolicyOverrider _overrider;

		public EligibilityEvaluator(IEligibilityRuleEvaluator<RuleContext, EligibilityResult> ruleEvaluator, IRuleSetBuilder ruleSetBuilder, IEligibilityPolicyOverrider overrider) {
			_ruleEvaluator = ruleEvaluator;
			_ruleSetBuilder = ruleSetBuilder;
			_overrider = overrider;
		}

        /// <summary>
        /// This is the business end of the library
        /// </summary>
        /// <param name="ruleContext">the contextual data which provides the information necessary to determine eligibility.</param>
        /// <param name="eligibilityType">the type of eligibility check to perform.</param>
        /// <returns></returns>
		public EligibilityResult GetEligibilityInfo(RuleContext ruleContext, EligibilityTypeEnum eligibilityType) {
			if (ruleContext == null)
				throw new ArgumentNullException(nameof(ruleContext));

			return computeReleaseEligibility(ruleContext, eligibilityType);
		}

		private EligibilityResult computeReleaseEligibility(RuleContext ruleContext, EligibilityTypeEnum eligibilityType) {

			var rootRuleSet = _ruleSetBuilder.BuildRootRuleSet(ruleContext.RuleContainer, eligibilityType);

			var eligibilityResult = _ruleEvaluator.EvaluateRules(rootRuleSet, ruleContext);
            
			updateResultWithOverride(eligibilityResult, ruleContext, eligibilityType);

			return eligibilityResult;
		}

        /// <summary>
        /// The "override" feature allows for manual/administrative overrides on a case-by-case basis, which will override the result of the rule evaluation.
        /// This also attaches a reason for the override so a human can see what's going on in the UI.
        /// </summary>
        /// <param name="eligibilityResult">the result of an eligibility rule evaluation</param>
        /// <param name="ruleContext">the contextual data which provides the information necessary to determine eligibility.</param>
        /// <param name="eligibilityType">the type of eligibility check to perform.</param>
		private void updateResultWithOverride(EligibilityResult eligibilityResult, RuleContext ruleContext, EligibilityTypeEnum eligibilityType) {
			var overrideResult = _overrider.GetPolicyOverride(ruleContext, eligibilityType);
			if (overrideResult != null) {
				eligibilityResult.Override = overrideResult;
			    eligibilityResult.IsEligible = overrideResult.ShouldPass;
			}
		}
	}
}