using System;

using EligibilityRuleEvaluator.Models;
using EligibilityRuleEvaluator.Repositories;

namespace EligibilityRuleEvaluator {
	public class RuleSetStorer {
		private readonly IEligibilityRepository _eligibilityRepository;
		private readonly RuleStorer _ruleStorer;

		public RuleSetStorer(IEligibilityRepository eligibilityRepository) {
			if (eligibilityRepository == null)
				throw new ArgumentNullException(nameof(eligibilityRepository));

			_eligibilityRepository = eligibilityRepository;
			_ruleStorer = new RuleStorer(eligibilityRepository);
		}

		public virtual int SaveRootRuleSet(RuleContainer customer, SystemUser user, EligibilityRuleSet ruleSet) {
			if (customer == null)
				throw new ArgumentNullException(nameof(customer));
			if (user == null)
				throw new ArgumentNullException(nameof(user));
			if (ruleSet == null)
				throw new ArgumentNullException(nameof(ruleSet));


			try {
				var ruleSetId = saveRuleSet(customer, user, ruleSet, null);
				return ruleSetId;
			}
			catch (Exception e) {
				Console.WriteLine(e);
				throw;
			}
		}

		private int saveRuleSet(RuleContainer customer, SystemUser user, EligibilityRuleSet ruleSet, int? parentRuleSetId) {

			var ruleSetPoco = mapRuleSetToPoco(customer, user, ruleSet, parentRuleSetId);
			var ruleSetId = _eligibilityRepository.CreateRuleSet(ruleSetPoco);

			foreach (var rule in ruleSet.Rules) {
				_ruleStorer.saveRule(user, rule, ruleSetId);
			}

			foreach (var childRuleSet in ruleSet.ChildRuleSets) {
				saveRuleSet(customer, user, childRuleSet, ruleSetId);
			}
			
			return ruleSetId;
		}

		private Eligibility_Ruleset mapRuleSetToPoco(RuleContainer ruleContainer, SystemUser user, EligibilityRuleSet ruleSet, int? parentRuleSetId) {
			return new Eligibility_Ruleset {
				Eligibility_Type_Id = (int)ruleSet.EligibilityType,
				Rule_Container_Id = ruleContainer.Id,
				Created_By = user.Id,
				Date_Created = DateTime.Now,
				Is_Deleted = false,
				Ruleset_Aggregator_Id = (int)ruleSet.Aggregator,
				Parent_Ruleset_Id = parentRuleSetId
			};
		}
	}
}