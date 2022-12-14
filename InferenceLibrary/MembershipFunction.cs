using System;
using System.Diagnostics;

namespace InferenceLibrary
{
    /// <summary>
    /// Trapezoid function used for calculating how well does the value describe a variable semantic value
    /// </summary>
    public class MembershipFunction
    {
        public MembershipFunction(string id, string displayName)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("id must be present");
            }
            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException("displayName must be present");
            }
            Id = id;
            DisplayName = displayName;
            IncludeX2 = true;
            IncludeX3 = true;
        }
        public MembershipFunction(string id, string displayName, double x1, double x2, double x3, double x4)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("id must be present");
            }
            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException("displayName must be present");
            }
            if (x2 < x1 || x3 < x2 || x4 < x3)
            {
                throw new ArgumentException("Xn must be >= X(n-1)");
            }

            Id = id;
            DisplayName = displayName;
            X1 = x1;
            X2 = x2;
            X3 = x3;
            X4 = x4;
            IncludeX2 = true;
            IncludeX3 = true;
        }

        /// <summary>
        /// Internal id - used for search purposes. E.g. "slow"
        /// </summary>
        public string Id { get; private set; }
        /// <summary>
        /// Name used for display purposes only - E.g. "Very old"
        /// </summary>
        public string DisplayName { get; private set; }
        /// <summary>
        /// X value for left 0 value
        /// </summary>
        public double X1 { get; set; }
        /// <summary>
        /// X value for left 1 value
        /// </summary>
        public double X2 { get; set; }
        /// <summary>
        /// X value for right 1 value
        /// </summary>
        public double X3 { get; set; }
        /// <summary>
        /// X value for right 0 value
        /// </summary>
        public double X4 { get; set; }
        /// <summary>
        /// Min X value
        /// </summary>
        public double Min => X1;
        /// <summary>
        /// Max X value
        /// </summary>
        public double Max => X4;

        public bool Inverted { get; set; }

        public LinguisticVariable LinguisticVariable { get; set; }

        /// <summary>
        /// If true and X1 == X2 then left range will be infinite
        /// </summary>
        public bool IgnoreX1 { get; set; }

        /// <summary>
        /// If true and X1 == X2 then function will return 1 for X2 value
        /// </summary>
        public bool IncludeX2 { get; set; }
        /// <summary>
        /// If true and X3 == X4 then function will return 1 for X3 value
        /// </summary>
        public bool IncludeX3 { get; set; }
        /// <summary>
        /// If true and X3 == X4 then right range will be infinite
        /// </summary>
        public bool IgnoreX4 { get; set; }

        /// <summary>
        /// Calculates a degree of how well does the value fit the semantic value (e.g. how well does value: 60 describe age - "old") 
        /// </summary>
        /// <param name="value">Value from [0-1] range</param>
        /// <returns></returns>
        public double Fuzzify(double value)
        {
            double fuzzified = 0;
            if (X1 == X2 && value <= X1)
            {
                if (value == X1)
                {
                    fuzzified = IncludeX2 ? 1 : 0;
                }
                else if (value < X1)
                {
                    fuzzified = IgnoreX1 ? 1 : 0;
                }
            }
            else if (X3 == X4 && value >= X3)
            {
                if (value == X3)
                {
                    fuzzified = IncludeX3 ? 1 : 0;
                }
                else if (value > X3)
                {
                    fuzzified = IgnoreX4 ? 1 : 0;
                }
            }
            else if (X1 <= value && value < X2)
            {
                fuzzified = (value - X1)/(X2 - X1);
            }
            else if (X2 <= value && value <= X3)
            {
                fuzzified = 1;
            }
            else if (X3 < value && value <= X4)
            {
                fuzzified = (X4 - value)/(X4 - X3);
            }

            if (Inverted)
            {
                fuzzified = 1 - fuzzified;
            }

            Debug.WriteLine($"Fuzzified {FullDisplayName} for value {value} is: {fuzzified}");

            return fuzzified;
        }

        public string FullDisplayName
        {
            get
            {
                if (LinguisticVariable == null) { return DisplayName;}
                return LinguisticVariable.DisplayName + " - " + DisplayName;
            }
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
