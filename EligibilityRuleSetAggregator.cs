namespace EligibilityRuleEvaluator {
	// keep in sync with [dbo].[Eligibility_Ruleset_Aggregator]
	public enum EligibilityRuleSetAggregator {
		Unknown = 0, // default
		All = 1, // All must be true
		Any = 2, // Any can be true
		None = 3 // None must be true
	}
}