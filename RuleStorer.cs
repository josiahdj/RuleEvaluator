using System;

using EligibilityRuleEvaluator.Models;
using EligibilityRuleEvaluator.Repositories;
using EligibilityRuleEvaluator.Rules;

namespace EligibilityRuleEvaluator {
	public class RuleStorer {
		private readonly IEligibilityRepository _eligibilityRepository;

		public RuleStorer(IEligibilityRepository eligibilityRepository) { _eligibilityRepository = eligibilityRepository; }

		public int saveRule(SystemUser user, IEligibilityRule rule, int ruleSetId) {
			if (rule == null)
				throw new ArgumentNullException(nameof(rule));

			var rulePoco = mapRuleToPoco(user, rule, ruleSetId);
			try {
				var ruleId = _eligibilityRepository.CreateRule(rulePoco);
				return ruleId;
			}
			catch (Exception e) {
				Console.WriteLine(e);
				throw;
			}
		}

		private Eligibility_Rule mapRuleToPoco(SystemUser user, IEligibilityRule rule, int ruleSetId) {
			var rulePoco = rule.ConvertToEntity();
			if (rulePoco.Eligibility_Rule_Type_Id == 0) {
				var ruleTypeName = rule.GetType().Name;
				var ruleType = _eligibilityRepository.GetRuleType(ruleTypeName); // NOTE: doesn't check for whether this rule type can be in this eligibility type. could potentially cause problems?
				if (ruleType == null)
					throw new Exception($"the rule type '{ruleTypeName}' was not found in the DB and so cannot be saved.");
				rulePoco.Eligibility_Rule_Type_Id = ruleType.Eligibility_Rule_Type_Id;
			}
			rulePoco.Date_Created = DateTime.Now;
			rulePoco.Is_Deleted = false;
			rulePoco.Created_By_Id = user.Id;
			rulePoco.Eligibility_Ruleset_Id = ruleSetId;
			return rulePoco;
		}
	}
}