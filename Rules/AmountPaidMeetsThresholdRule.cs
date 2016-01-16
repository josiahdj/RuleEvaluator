using System;

using EligibilityRuleEvaluator.Models;

namespace EligibilityRuleEvaluator.Rules {
	public class AmountPaidMeetsThresholdRule : IEligibilityRule {
		private readonly Eligibility_Rule _ruleData = new Eligibility_Rule();

		public AmountPaidMeetsThresholdRule(Eligibility_Rule ruleData) : this(ruleData.Amount_Paid_Threshold.GetValueOrDefault(0)) {
			if (ruleData == null)
				throw new ArgumentNullException(nameof(ruleData));

			_ruleData = ruleData;
		}

		public AmountPaidMeetsThresholdRule(decimal threshold) {
			Threshold = threshold;
			_ruleData.Amount_Paid_Threshold = threshold;
		}

		public int Id => _ruleData.Eligibility_Rule_Id;

	    public decimal Threshold { get; private set; }

		public bool IsEligible(RuleContext ruleContext) {
		    return ruleContext?.TotalPaid >= Threshold;
		}

		public Eligibility_Rule ConvertToEntity() {
			if (_ruleData == null)
				throw new NullReferenceException("ruleData is null but shouldn't be.");
			return _ruleData;
		}

		public string Description => $"Amount paid is >= {Threshold:C}";
	}
}