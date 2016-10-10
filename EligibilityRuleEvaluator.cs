using System;
using System.Linq;

using EligibilityRuleEvaluator.Models;
using EligibilityRuleEvaluator.Rules;

namespace EligibilityRuleEvaluator {
	public class EligibilityRuleEvaluator : IEligibilityRuleEvaluator<RuleContext, EligibilityResult> {
		public EligibilityResult EvaluateRules(EligibilityRuleSet ruleSet, RuleContext ruleContext) {

			var aggregateOperator = ruleSet.Aggregator;

			var rootEligibilityResult = getInitialEligibilityResult(aggregateOperator);

			return aggregateRuleSet(ruleSet, ruleContext, rootEligibilityResult);
		}

		private EligibilityResult getInitialEligibilityResult(EligibilityRuleSetAggregator aggregateOperator) {
			// initial assumption is 'fail' for everything but 'All', which should pass by default
			var initialValue = aggregateOperator == EligibilityRuleSetAggregator.All;
			return new EligibilityResult(initialValue, aggregateOperator);
		}

		private EligibilityResult aggregateRuleSet(EligibilityRuleSet ruleSet, RuleContext ruleContext, EligibilityResult rootResult) {
			var aggregateOperator = ruleSet.Aggregator;

			var combinator = getCombinator(aggregateOperator);

			var rulesPass = aggregateRules(ruleSet, ruleContext, combinator, ref rootResult);
			
			Func<bool, EligibilityRuleSet, bool> ruleSetEvalFunc =
				(currentRuleSetIsValid, childRuleSet) =>
					{
						var initialRuleSetResult = getInitialEligibilityResult(childRuleSet.Aggregator);
						var finalRuleSetResult = aggregateRuleSet(childRuleSet, ruleContext, initialRuleSetResult);
						var isEligible = combinator(currentRuleSetIsValid, finalRuleSetResult.IsEligible);
						rootResult.RuleSetResults.Add(finalRuleSetResult);

						return isEligible;
					}; // NOTE: recursive call to aggregateRuleSet

			// then, we go on to recursively aggragate all the rules within any children rulesets of this ruleset, which
			// means we call this very method until there are no more child rulesest or rules (when we "return")
			var ruleSetIsEligible = ruleSet.ChildRuleSets.Aggregate(rulesPass, ruleSetEvalFunc);
			rootResult.IsEligible = ruleSetIsEligible;
			return rootResult;
		}

		private Func<bool, bool, bool> getCombinator(EligibilityRuleSetAggregator aggregateOperator) {
			Func<bool, bool, bool> combinator = null;
			switch (aggregateOperator) {
				case EligibilityRuleSetAggregator.Any:
					combinator = eitherAreTrue;
					break;
				case EligibilityRuleSetAggregator.All:
					combinator = bothAreTrue;
					break;
				case EligibilityRuleSetAggregator.None:
					combinator = neitherAreTrue;
					break;
			}
			return combinator;
		}

		private bool eitherAreTrue(bool l, bool r) {
			return l || r;
		}

		private bool bothAreTrue(bool l, bool r) {
			return l && r;
		}

		private bool neitherAreTrue(bool l, bool r) {
			return !l && !r;
		}

        /// <summary>
        /// This is the thing that does all the "real" work. It recursively evaluates the rules and returns a true/false for is/isn't eligible. Also rolls up
        /// individual rule evaluation results (for diagnostics and/or UI display) in <paramref name="resultSetResult"/>.<code>RuleResults</code> (List of <see cref="EligibilityRuleResult"/>)
        /// </summary>
        /// <param name="ruleSet">the rule holder (the node)</param>
        /// <param name="ruleContext">the thing to which we're applying the rules</param>
        /// <param name="combinator">the 'either', 'both', or 'neither' that is used to aggregate the individual rule results into a single result for the whole ruleSet tree</param>
        /// <param name="resultSetResult">this is an result accumulator of sorts, to hold the rule results so that they can be displayed to a human, rule by rule (i.e. why this context 
        /// passed/failed the evaluation)</param>
        /// <returns>true for Is Eligible, false for Is Not Eligible</returns>
		private bool aggregateRules(EligibilityRuleSet ruleSet, RuleContext ruleContext, Func<bool, bool, bool> combinator, ref EligibilityResult resultSetResult) {
			EligibilityResult result = resultSetResult;
			Func<bool, IEligibilityRule, bool> ruleEvalFunc =
				(currentRuleIsValid, eligibilityRule) =>
					{
						bool isEligible = result.IsEligible;
						if (eligibilityRule != null) // skip null rules (probably a rule def in db but no class implementation?
							isEligible = eligibilityRule.IsEligible(ruleContext);
						var accumulatedValue = combinator(isEligible, currentRuleIsValid);
						result.RuleResults.Add(new EligibilityRuleResult(eligibilityRule, isEligible));
						return accumulatedValue;
					};

			var rulesAreEligible = ruleSet.Rules.Aggregate(resultSetResult.IsEligible, ruleEvalFunc);
			resultSetResult.IsEligible = rulesAreEligible;

			return rulesAreEligible;
		}
	}

}