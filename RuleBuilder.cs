using System;
using System.Reflection;

using EligibilityRuleEvaluator.Models;
using EligibilityRuleEvaluator.Repositories;
using EligibilityRuleEvaluator.Rules;

using log4net;

namespace EligibilityRuleEvaluator {
	public class RuleBuilder {
	    private const string PLUGIN_NAMESPACE = "EligibilityRuleEvaluator.Rules";
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IEligibilityRepository _eligibilityRepository;

		public RuleBuilder(IEligibilityRepository eligibilityRepository) {
			_eligibilityRepository = eligibilityRepository;
		}

		public IEligibilityRule BuildRule(Eligibility_Rule ruleData) {
			if (ruleData == null)
				throw new ArgumentNullException(nameof(ruleData));

			var typeId = ruleData.Eligibility_Rule_Type_Id;
			var ruleType = _eligibilityRepository.GetRuleType(typeId);
			if (ruleType != null) {
				var ruleTypeName = ruleType.Clr_Type_Name;
				var ruleInstance = instantiateRule(ruleTypeName, ruleData);
				return ruleInstance;
			}
			return null;
		}

		private IEligibilityRule instantiateRule(string ruleTypeName, Eligibility_Rule ruleData) {
			try {
				// will throw exception if there isn't a class for the given rule type in DB and given assembly and namespace
				if (!String.IsNullOrEmpty(ruleTypeName)) {
					var type = Type.GetType(PLUGIN_NAMESPACE + "." + ruleTypeName);
					if (type != null) {
						var objectHandle = Activator.CreateInstance(type, ruleData) as IEligibilityRule;
						return objectHandle;
					}
				}
				_log.Warn("Could not create default instance of the rule plugin because TypeName was null or empty or not found");
			}
			catch (TypeLoadException e) {
				_log.Error(e.Message, e);
			}
			catch (Exception e) {
				_log.Error(e.Message, e);
			}
			return null;
		}
	}
}