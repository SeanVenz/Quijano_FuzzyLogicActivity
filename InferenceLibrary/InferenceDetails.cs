using System.Collections.Generic;
using InferenceLibrary.Rules;

namespace InferenceLibrary
{
    public class InferenceDetails
    {
        private static InferenceDetails _instance;

        public static void Reset()
        {
            _instance = new InferenceDetails();
        }

        public static InferenceDetails Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;

                }

                _instance = new InferenceDetails();
                return _instance;
            }
        }
        
        public Dictionary<MembershipFunction, double> FuzzificationResults = new Dictionary<MembershipFunction, double>();
        public Dictionary<FuzzyRule, double> RuleEvaluationResults = new Dictionary<FuzzyRule, double>();
        public List<Point> AggregatedFunction = new List<Point>();
        public Point Result;

        public struct Point
        {
            public double X { get; }
            public double Y { get; }

            public Point(double x, double y)
            {
                this.X = x;
                this.Y = y;
            }
        }
    }
}
