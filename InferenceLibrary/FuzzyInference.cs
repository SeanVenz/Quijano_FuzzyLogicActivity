using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using InferenceLibrary.Rules;

namespace InferenceLibrary
{
    /// <summary>
    /// Entry class for fuzzy inference
    /// </summary>
    public class FuzzyInference
    {
        public class ValuesNotPreparedException : Exception { }

        /// <summary>
        /// Set of fuzzy inference rules
        /// </summary>
        public IEnumerable<FuzzyRule> Rules { get; }

        public FuzzyInference(IEnumerable<FuzzyRule> rules)
        {
            Rules = rules;
        }

        /// <summary>
        /// Performs fuzzy inference based on a set of fuzzy rules
        /// </summary>
        /// <returns>A sharp value for a conclusion linguistic variable</returns>
        public double Infere()
        {
            if (!IsValid())
            {
                throw new ValuesNotPreparedException();
            }

            InferenceDetails.Reset();

            foreach (FuzzyRule rule in Rules)
            {
                rule.Conclusion.PremiseModifier = rule.Evaluate();
                InferenceDetails.Instance.RuleEvaluationResults.Add(rule, rule.Conclusion.PremiseModifier);
                Debug.WriteLine($"Rule: {rule.Text}; Value: {rule.Conclusion.PremiseModifier}");
            }


            return new Defuzzifier(Rules.Select(r => r.Conclusion)).Defuzzify();
        }

        /// <summary>
        /// Checks if all is set up for inference
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            // TODO: Check if all input values are set
            return true;
        }
    }
}
