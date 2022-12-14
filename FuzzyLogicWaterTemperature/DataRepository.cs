using System.Collections.Generic;
using System.Linq;
using InferenceLibrary;
using InferenceLibrary.Rules;

namespace ItoFuzzyLogicInference
{
    public class DataRepository
    {

        public IEnumerable<FuzzyRule> FuzzyRules { get; set; }

        public IEnumerable<LinguisticVariable> LinguisticVariables { get; set; }

        public bool Initialized => (FuzzyRules != null && LinguisticVariables != null);

        /// <summary>
        /// Returns all right side variables from rules
        /// </summary>
        public IEnumerable<LinguisticVariable> ConclusionVariables
        {
            get { return FuzzyRules.Select(r => r.Conclusion.MembershipFunction.LinguisticVariable).Distinct(); }
        }

        /// <summary>
        /// Returns all rules for which the conclusion's variable is equal to variable parameter
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public IEnumerable<FuzzyRule> RulesForVariable(LinguisticVariable variable)
        {
            return FuzzyRules.Where(r => r.Conclusion.MembershipFunction.LinguisticVariable == variable);
        }

        /// <summary>
        /// Returns a list of conditions from rules that are related to a given linguistic variable
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public IEnumerable<FuzzyCondition> ConditionsForVariable(LinguisticVariable variable)
        {
            return
                FuzzyRules.SelectMany(r => r.Conditions).Where(c => c.MembershipFunction.LinguisticVariable.Id == variable.Id);
        }

        /// <summary>
        /// Returns a list of variables that need an input in order to infere value of given conclusion variable
        /// </summary>
        /// <param name="conclusionVariable"></param>
        /// <returns></returns>
        public IEnumerable<LinguisticVariable> InputVariables(LinguisticVariable conclusionVariable)
        {
            var rules = RulesForVariable(conclusionVariable);
            return rules.SelectMany(
                r => r.Conditions.Select(c => c.MembershipFunction.LinguisticVariable)
            ).Distinct();
        }
    }
}
