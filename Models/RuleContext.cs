namespace EligibilityRuleEvaluator.Models {
    /// <summary>
    /// This is the thing the rules apply to
    /// </summary>
    public class RuleContext {
        public int Id { get; set; }
        public RuleContainer RuleContainer { get; set; }
        public decimal NetDue { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalDue { get; set; }

        public static bool HasValidDocumentProof(DocumentProofType documentProofType) { return documentProofType == DocumentProofType.DriversLicense; }
    }
}