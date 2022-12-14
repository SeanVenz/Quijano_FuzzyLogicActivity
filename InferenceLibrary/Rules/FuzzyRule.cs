using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;

namespace InferenceLibrary.Rules
{
    public enum ERuleOperator
    {
        And,
        Or
    }

    /// <summary>
    /// Represents an (IF-THEN) fuzzy rule 
    /// </summary>
    public class FuzzyRule
    {
        /// <summary>
        /// IF (left) part of the rule - a set of AND connected conditions
        /// </summary>
        public IEnumerable<FuzzyCondition> Conditions { get; private set; }

        public ERuleOperator RuleOperator { get; set; }
        /// <summary>
        /// THEN (right) part of the rule
        /// </summary>
        public FuzzyConclusion Conclusion { get; private set; }

        public FuzzyRule(IEnumerable<FuzzyCondition> conditions, FuzzyConclusion conclusion)
        {

            if (conditions == null || !conditions.Any())
            {
                throw new ArgumentException("conditions must be present");
            }
            Conditions = conditions;
            Conclusion = conclusion;
            RuleOperator = ERuleOperator.And;
        }

        /// <summary>
        /// Calculates a conclusion's PremiseModifier based on left side conditions
        /// </summary>
        /// <returns>Numerical (sharp) value - based on conditions and their input values</returns>
        public double Evaluate()
        {
            return new FuzzyRuleEvaluator(this).Evaluate();
        }

        public string Text
        {
            get {
                var builder = new StringBuilder();
                builder.Append("IF ");
                var condTexts = Conditions.Select(
                    c =>
                        string.Format("{0} {1} {2}", c.MembershipFunction.LinguisticVariable.DisplayName,
                                                     c.Negated ? "IS NOT" : "IS",
                                                     c.MembershipFunction.DisplayName));

                builder.Append(string.Join(RuleOperator == ERuleOperator.And ? " AND " : " OR ", condTexts));
                builder.Append(" THEN ");
                builder.Append(
                    $"{Conclusion.MembershipFunction.LinguisticVariable.DisplayName} IS {Conclusion.MembershipFunction.DisplayName}");

                return builder.ToString();
            }
        }
    }
}
