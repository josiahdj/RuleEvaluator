using System;

using AutoMapper;

using EligibilityRuleEvaluator.Models;
using EligibilityRuleEvaluator.Repositories;

namespace EligibilityRuleEvaluator {
	public class EligibilityPolicyOverrider : IEligibilityPolicyOverrider {
		private readonly IEligibilityRepository _eligibilityRepository;

		public EligibilityPolicyOverrider(IEligibilityRepository eligibilityRepository) {
			_eligibilityRepository = eligibilityRepository;
		}

		public int AddPolicyOverride(EligibilityPolicyOverride eligibilityPolicyOverride) {
			throwIfOneExists(eligibilityPolicyOverride);

			var eligibilityOverride = Mapper.Map<EligibilityPolicyOverride, Eligibility_Override>(eligibilityPolicyOverride);
			return _eligibilityRepository.AddPolicyOverride(eligibilityOverride);
		}

		private void throwIfOneExists(EligibilityPolicyOverride eligibilityPolicyOverride) {
			var existingOverride = _eligibilityRepository.GetPolicyOverride(eligibilityPolicyOverride.RuleContextId, eligibilityPolicyOverride.EligibilityType);
			if (existingOverride != null)
				throw new InvalidOperationException("You can't have more than 1 override at a time.");
		}

		public EligibilityPolicyOverride GetPolicyOverride(RuleContext ruleContext, EligibilityTypeEnum eligibilityType) {
			var eligibilityOverride = _eligibilityRepository.GetPolicyOverride(ruleContext.Id, eligibilityType);
			return Mapper.Map<Eligibility_Override, EligibilityPolicyOverride>(eligibilityOverride);
		}

		public void DisablePolicyOverride(int overrideId, Guid userId) {
			var numberUpdated = _eligibilityRepository.DisablePolicyOverride(overrideId, userId);
			if (numberUpdated > 1)
				throw new InvalidOperationException("There was more than 1 override disabled. There should be only one override.");
			if (numberUpdated == 0)
				throw new InvalidOperationException("There was no override to disable. There should be one (and only one) override.");
		}
	}
}