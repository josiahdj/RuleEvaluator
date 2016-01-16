using System;

using EligibilityRuleEvaluator.Models;

namespace EligibilityRuleEvaluator.Rules {
	public class AmountDueLessThanThresholdRule : IEligibilityRule {
		private readonly Eligibility_Rule _ruleData = new Eligibility_Rule();

		public AmountDueLessThanThresholdRule(Eligibility_Rule ruleData) : this(ruleData.Amount_Due_Threshold.GetValueOrDefault(0)) {
			if (ruleData == null)
				throw new ArgumentNullException(nameof(ruleData));

			_ruleData = ruleData;
		}

		public AmountDueLessThanThresholdRule(decimal threshold) {
			Threshold = threshold;
			_ruleData.Amount_Due_Threshold = threshold;
		}

		public int Id => _ruleData.Eligibility_Rule_Id;

	    public decimal Threshold { get; private set; }

		public bool IsEligible(RuleContext ruleContext) {
			if (ruleContext == null)
				return false;

		    var meetsThreshold = ruleContext.NetDue <= Threshold;
			return meetsThreshold;
		}

		public Eligibility_Rule ConvertToEntity() {
			if (_ruleData == null)
				throw new NullReferenceException("ruleData is null but shouldn't be.");
			return _ruleData;
		}

		public string Description => $"Amount due is <= {Threshold:C}";
	}
}