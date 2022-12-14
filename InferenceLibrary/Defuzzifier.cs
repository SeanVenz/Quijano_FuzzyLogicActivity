using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using InferenceLibrary.Rules;

namespace InferenceLibrary
{
    /// <summary>
    /// Used for calculating output (sharp) values based on rule conclusion values 
    /// 
    /// </summary>
    public class Defuzzifier
    {
        private readonly IEnumerable<FuzzyConclusion> _conclusions;

        /// <summary>
        /// How many steps should the MinMax linguistic variable range be split into?
        /// </summary>
        public const int RangeStepsCount = 100;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="conclusions">Conclusions with calculated premise modifiers</param>
        public Defuzzifier(IEnumerable<FuzzyConclusion> conclusions)
        {
            _conclusions = conclusions;
        }

        /// <summary>
        /// Calculates output (sharp) value based on rules conclusion values using Mean of Maxima method
        /// </summary>
        /// <returns>Numerical value for a linguistic variable</returns>
        public double Defuzzify()
        {
            var minX = _conclusions.Select(c => c.MembershipFunction.Min).Min();
            var maxX = _conclusions.Select(c => c.MembershipFunction.Max).Max();

            var maxFuzzificationValue = 0.0;
            var currentMaxStart = 0.0;
            var longestMaxStart = 0.0;
            var currentLength = 0.0;
            var maxLength = 0.0;
            var step = 1.0;
            if (maxX - minX > 100)
            {
                step = (maxX - minX) / RangeStepsCount;
            }

            for (var i = minX; i <= maxX; i += step)
            {
                var maxFuzVal = _conclusions.Select(c => c.PremiseModifier * c.MembershipFunction.Fuzzify(i)).Max();

                if (maxFuzVal > maxFuzzificationValue) // New max found
                {
                    maxFuzzificationValue = maxFuzVal;
                    currentMaxStart = i;
                    currentLength = 0.0;
                }
                else if (maxFuzzificationValue == maxFuzVal && 0 < maxFuzVal)
                {
                    if (currentLength == 0 && i - longestMaxStart > step)
                    {
                        currentMaxStart = i;
                    }
                    else
                    {
                        currentLength += step;
                    }
                }
                else // Value is lower than a previous max, reset 
                {
                    currentLength = 0;
                }

                if (currentLength >= maxLength)
                {
                    longestMaxStart = currentMaxStart;
                    maxLength = currentLength;
                }
                InferenceDetails.Instance.AggregatedFunction.Add(new InferenceDetails.Point(i, maxFuzVal));
                Debug.WriteLine($"Longest max start: {longestMaxStart}");
                Debug.WriteLine($"Max length: {maxLength}");
                Debug.WriteLine($"Max Fuz val: {maxFuzVal}");
                Debug.WriteLine($"Current length: {currentLength}");
            }
            
            var mid = longestMaxStart + maxLength / 2.0;

            InferenceDetails.Instance.Result = new InferenceDetails.Point(mid, maxFuzzificationValue);

            return mid;
        }
    }
}
