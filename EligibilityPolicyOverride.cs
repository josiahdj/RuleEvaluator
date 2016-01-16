using System;

namespace EligibilityRuleEvaluator {
	public class EligibilityPolicyOverride {
		public int Id { get; set; }
		public bool ShouldPass { get; set; }
		public Guid UserId { get; set; }
		public EligibilityOverrideReason OverrideReason { get; set; }
		public EligibilityTypeEnum EligibilityType { get; set; }
		public int RuleContextId { get; set; }
		public int PolicyId { get; set; }
		public DateTime DateCreated { get; set; }
	}
}