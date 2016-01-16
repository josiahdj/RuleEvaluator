using System;
using System.Collections.Generic;

using EligibilityRuleEvaluator.Models;

namespace EligibilityRuleEvaluator.Repositories {
	public interface IEligibilityRepository {
		Eligibility_Ruleset GetRootEligibilityRuleSet(RuleContainer customer, EligibilityTypeEnum eligibilityType);

		Eligibility_Ruleset GetRuleSet(int eligibilityRulesetId);

		List<Eligibility_Rule> GetRuleSetRules(int eligibilityRulesetId);

		Eligibility_Rule_Type GetRuleType(int ruleTypeId);

		List<Eligibility_Ruleset> GetChildRuleSets(int eligibilityRulesetId);

		Eligibility_Rule_Type GetRuleType(string ruleTypeName);

		int CreateRuleSet(Eligibility_Ruleset ruleSet);

		int CreateRule(Eligibility_Rule rule);

		void DeleteRuleSet(int ruleSetId);

		void DeleteRule(int ruleId);

		int AddPolicyOverride(Eligibility_Override eligibilityPolicyOverride);

		Eligibility_Override GetPolicyOverride(int ruleContextId, EligibilityTypeEnum eligibilityType);

		int DisablePolicyOverride(int overrideId, Guid userId);

		int ReplaceExistingPolicyWithNewOne(EligibilityRuleSet rootRuleSet, RuleContainer customer, SystemUser createdBy);

		List<Eligibility_Rule_Type> GetRuleTypes(EligibilityTypeEnum eligibilityTypeEnum);
	}

}
