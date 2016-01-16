using System;

namespace EligibilityRuleEvaluator.Models {
    public class Eligibility_Rule {
        public int Eligibility_Rule_Id { get; set; }
        public int Eligibility_Rule_Type_Id { get; set; }
        public int Eligibility_Ruleset_Id { get; set; }
        public DateTime Date_Created { get; set; }
        public bool Is_Deleted { get; set; }
        public int Created_By_Id { get; set; }
        public int Document_Proof_Type_Id { get; set; }
        public decimal? Amount_Due_Threshold { get; set; }
        public decimal? Amount_Paid_Threshold { get; set; }
    }
}
