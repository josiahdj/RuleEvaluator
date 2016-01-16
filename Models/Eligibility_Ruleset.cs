using System;

namespace EligibilityRuleEvaluator.Models {
    public class Eligibility_Ruleset {
        public int Eligibility_Ruleset_Id;
        public int Ruleset_Aggregator_Id;
        public int Eligibility_Type_Id;
        public int? Parent_Ruleset_Id { get; set; }
        public int Rule_Container_Id { get; set; }
        public int Created_By { get; set; }
        public DateTime Date_Created { get; set; }
        public bool Is_Deleted { get; set; }
    }
}
