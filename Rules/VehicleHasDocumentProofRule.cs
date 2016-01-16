using System;

using EligibilityRuleEvaluator.Models;

namespace EligibilityRuleEvaluator.Rules {
	public class VehicleHasDocumentProofRule : IEligibilityRule {
		private readonly Eligibility_Rule _ruleData = new Eligibility_Rule();
		private readonly DocumentProofType _documentProofType;

		public VehicleHasDocumentProofRule(DocumentProofType documentProofType) {
			_documentProofType = documentProofType;
			_ruleData.Document_Proof_Type_Id = (int)documentProofType;
		}

		public VehicleHasDocumentProofRule(Eligibility_Rule ruleData) {
			if (ruleData == null)
				throw new ArgumentNullException(nameof(ruleData));

			_ruleData = ruleData;
			Enum.TryParse(ruleData.Document_Proof_Type_Id.ToString(), out _documentProofType);
		}


		public int Id => _ruleData.Eligibility_Rule_Id;

	    public string Description => $"Vehicle has document '{DocumentProofType}'";

	    public DocumentProofType DocumentProofType => _documentProofType;

	    public bool IsEligible(RuleContext ruleContext) {
			if (ruleContext == null)
				throw new ArgumentNullException(nameof(ruleContext));

			return RuleContext.HasValidDocumentProof(DocumentProofType);
		}

		public Eligibility_Rule ConvertToEntity() {
			if (_ruleData == null)
				throw new NullReferenceException("ruleData is null but shouldn't be.");
			return _ruleData;
		}
	}
}