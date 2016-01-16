using EligibilityRuleEvaluator.Rules;

namespace EligibilityRuleEvaluator.Models {
    /// <summary>
    /// This is the user applying the rules, used for auditing purposes and for rights, etc.
    /// </summary>
    public class SystemUser {
        public int Id { get; set; }
    }

    /// <summary>
    /// This is the outer boundary within which the rules apply.
    /// e.g. for a multi-tenant application, this would be the tenant.
    /// </summary>
    public class RuleContainer {
        public int Id { get; set; }
    }

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
