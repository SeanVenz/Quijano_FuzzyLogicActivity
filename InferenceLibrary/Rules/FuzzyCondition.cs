namespace InferenceLibrary.Rules
{
    /// <summary>
    /// A single AND condition on the left (IF) side of a fuzzy rule
    /// </summary>
    public class FuzzyCondition
    {
        public FuzzyCondition(MembershipFunction membershipFunction)
        {
            MembershipFunction = membershipFunction;
        }

        /// <summary>
        /// Membership function the condition corresponds to. E.g. "speed" - "fast"
        /// </summary>
        public MembershipFunction MembershipFunction { get; private set; }
        /// <summary>
        /// Numerical (sharp) value for the condition
        /// </summary>
        public double InputValue { get; set; }

        public bool Negated { get; set; }
    }
}
