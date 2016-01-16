using System;

using EligibilityRuleEvaluator.Models;

namespace EligibilityRuleEvaluator.Rules {
	public class AmountPaidMoreThanPercentOfAmountDue : IEligibilityRule {
		private readonly Eligibility_Rule _ruleData = new Eligibility_Rule();

		public AmountPaidMoreThanPercentOfAmountDue(Eligibility_Rule ruleData) : this(ruleData.Amount_Paid_Threshold.GetValueOrDefault(0)) {
			if (ruleData == null)
				throw new ArgumentNullException(nameof(ruleData));

			_ruleData = ruleData;
		}

		public AmountPaidMoreThanPercentOfAmountDue(decimal percent) {
			Threshold = percent;
			_ruleData.Amount_Paid_Threshold = percent;
		}

		public int Id => _ruleData.Eligibility_Rule_Id;

	    public decimal Threshold { get; private set; }

		public bool IsEligible(RuleContext ruleContext) {
		    return ruleContext != null && ruleContext.TotalPaid >= Threshold * ruleContext.TotalDue;
		}

		public Eligibility_Rule ConvertToEntity() {
			if (_ruleData == null)
				throw new NullReferenceException("ruleData is null but shouldn't be.");
			return _ruleData;
		}

		public string Description => $"Amount Paid is >= {Threshold:P} of Amount Due";
	}
}
