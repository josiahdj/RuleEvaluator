using System;

using EligibilityRuleEvaluator.Repositories;
using EligibilityRuleEvaluator.Rules;

namespace EligibilityRuleEvaluator {
	public class RuleSetDeleter {
		private readonly IEligibilityRepository _eligibilityRepository;

		public RuleSetDeleter(IEligibilityRepository eligibilityRepository) {
			if (eligibilityRepository == null)
				throw new ArgumentNullException(nameof(eligibilityRepository));

			_eligibilityRepository = eligibilityRepository;
		}

		public virtual void DeleteRootRuleSet(EligibilityRuleSet ruleSet) {
			if (ruleSet == null)
				throw new ArgumentNullException(nameof(ruleSet));

			deleteRuleSet(ruleSet);
		}

		private void deleteRuleSet(EligibilityRuleSet ruleSet) {


			foreach (var rule in ruleSet.Rules) {
				deleteRule(rule);
			}

			foreach (var childRuleSet in ruleSet.ChildRuleSets) {
				deleteRuleSet(childRuleSet);
			}

			_eligibilityRepository.DeleteRuleSet(ruleSet.Id);

			
		}

		private void deleteRule(IEligibilityRule rule) {
			if (rule == null)
				throw new ArgumentNullException(nameof(rule));

			_eligibilityRepository.DeleteRule(rule.Id);
			
		}

	}
}