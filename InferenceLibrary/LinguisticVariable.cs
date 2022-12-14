using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceLibrary
{
    /// <summary>
    /// Represents a linguistic variable - e.g. "Age"
    /// </summary>
    public class LinguisticVariable
    {
        private IEnumerable<MembershipFunction> _membershipFunctions;

        public LinguisticVariable(string id, string displayName = "", string displayUnit = "")
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("id must be present");
            }

            Id = id;
            DisplayName = displayName;
            DisplayUnit = displayUnit;
        }

        public LinguisticVariable(string id, string displayName, IEnumerable<MembershipFunction> membershipFunctions, string displayUnit = "")
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
            MembershipFunctions = membershipFunctions;
            DisplayUnit = displayUnit;
        }

        /// <summary>
        /// Internal id for search purposes (e.g. "age")
        /// </summary>
        public string Id { get; private set; }
        /// <summary>
        /// Name for display purposes (e.g. "Salary value")
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Unit for display purposes (e.g. "years")
        /// </summary>
        public string DisplayUnit { get; set; }
        /// <summary>
        /// Min X value
        /// </summary>
        public double Min => MembershipFunctions.Select(m => m.Min).Min();
        /// <summary>
        /// Max X value
        /// </summary>
        public double Max => MembershipFunctions.Select(m => m.Max).Max();

        /// <summary>
        /// Membership functions for this variable - e.g. ("slow", "medium", "fast") for "speed" variable
        /// </summary>
        public IEnumerable<MembershipFunction> MembershipFunctions
        {
            get { return _membershipFunctions; }
            set
            {
                if (value == null || !value.Any())
                {
                    throw new ArgumentException("membershipFunctions must be present");
                }
                _membershipFunctions = value;
                foreach (var membershipFunction in _membershipFunctions)
                {
                    membershipFunction.LinguisticVariable = this;
                }
            }
        }
    }
}
