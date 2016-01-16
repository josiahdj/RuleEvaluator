using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using EligibilityRuleEvaluator.Models;
using EligibilityRuleEvaluator.Repositories;

namespace EligibilityRuleEvaluator {
	public class RuleSetBuilder : IRuleSetBuilder {
		private readonly IEligibilityRepository _eligibilityRepository;
		private readonly RuleBuilder _ruleBuilder;

		public RuleSetBuilder(IEligibilityRepository eligibilityRepository) {
			if (eligibilityRepository == null)
				throw new ArgumentNullException(nameof(eligibilityRepository));

			_eligibilityRepository = eligibilityRepository;
			_ruleBuilder = new RuleBuilder(_eligibilityRepository);
		}

		public virtual EligibilityRuleSet BuildRootRuleSet(RuleContainer customer, EligibilityTypeEnum eligibilityType) {
			if (customer == null)
				throw new ArgumentNullException(nameof(customer));

			var ruleSet = _eligibilityRepository.GetRootEligibilityRuleSet(customer, eligibilityType);
			if (ruleSet == null)
				throw new NoNullAllowedException($"There is no root {eligibilityType} Eligibility RuleSet for this {nameof(customer)}");
			
			var finalRuleSet = buildRuleSet(ruleSet);

			return finalRuleSet;
		}

		private EligibilityRuleSet buildRuleSet(Eligibility_Ruleset ruleSetData) {
			if (ruleSetData == null)
				throw new ArgumentNullException(nameof(ruleSetData));

			var finalRuleSet = buildFinalRuleSet(ruleSetData);

			var rules = _eligibilityRepository.GetRuleSetRules(ruleSetData.Eligibility_Ruleset_Id);
			addRulesToRuleSet(rules, finalRuleSet);

			var childRuleSets = _eligibilityRepository.GetChildRuleSets(ruleSetData.Eligibility_Ruleset_Id);
			addRuleSetsToRuleSet(childRuleSets, finalRuleSet);

			return finalRuleSet;
		}

		private EligibilityRuleSet buildFinalRuleSet(Eligibility_Ruleset ruleSetData) {
			var aggregator = convertAggregatorIdToEnum(ruleSetData);

			var eligibilityType = convertEligibilityTypeToEnum(ruleSetData);

			var finalRuleSet = new EligibilityRuleSet {Aggregator = aggregator, EligibilityType = eligibilityType, Id = ruleSetData.Eligibility_Ruleset_Id};
			return finalRuleSet;
		}

		private EligibilityRuleSetAggregator convertAggregatorIdToEnum(Eligibility_Ruleset ruleSetData) {
			EligibilityRuleSetAggregator aggregator;
			Enum.TryParse(ruleSetData.Ruleset_Aggregator_Id.ToString(), out aggregator);
			return aggregator;
		}

		private EligibilityTypeEnum convertEligibilityTypeToEnum(Eligibility_Ruleset ruleSetData) {
			EligibilityTypeEnum eligibilityType;
			Enum.TryParse(ruleSetData.Eligibility_Type_Id.ToString(), out eligibilityType);
			return eligibilityType;
		}

		private void addRulesToRuleSet(IEnumerable<Eligibility_Rule> rulesData, EligibilityRuleSet finalRuleSet) {
			if (rulesData == null)
				return;

			var eligibilityRules = rulesData.Select(_ruleBuilder.BuildRule);
			foreach (var eligibilityRule in eligibilityRules)
				finalRuleSet.Rules.Add(eligibilityRule);
		}

		private void addRuleSetsToRuleSet(IEnumerable<Eligibility_Ruleset> childRuleSetsData, EligibilityRuleSet finalRuleSet) {
			if (childRuleSetsData == null)
				return;

			var finalChildRuleSets = childRuleSetsData.Select(buildRuleSet); // NOTE: recursive call to buildRuleSet
			foreach (var finalChildRuleSet in finalChildRuleSets)
				finalRuleSet.ChildRuleSets.Add(finalChildRuleSet);
		}
	}
}