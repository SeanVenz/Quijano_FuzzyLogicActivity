namespace InferenceLibrary.Rules
{
    /// <summary>
    /// Right (THEN) side of fuzzy rule clause
    /// </summary>
    public class FuzzyConclusion
    {
        public FuzzyConclusion(MembershipFunction membershipFunction)
        {
            MembershipFunction = membershipFunction;
        }

        /// <summary>
        /// A function the conclusion corresponds to. E.g. "speed" - "fast"
        /// </summary>
        public MembershipFunction MembershipFunction { get; private set; }
        /// <summary>
        /// Calculated result of premise
        /// </summary>
        public double PremiseModifier { get; set; }
    }
}
