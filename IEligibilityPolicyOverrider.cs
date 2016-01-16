using System;
using EligibilityRuleEvaluator.Models;

namespace EligibilityRuleEvaluator {
    public interface IEligibilityPolicyOverrider {
        int AddPolicyOverride(EligibilityPolicyOverride eligibilityPolicyOverride);
        void DisablePolicyOverride(int overrideId, Guid userId);
        EligibilityPolicyOverride GetPolicyOverride(RuleContext ruleContext, EligibilityTypeEnum eligibilityType);
    }
}