using System.Collections.Generic;
using InferenceLibrary;
using InferenceLibrary.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InferenceLibraryTests
{
    [TestClass]
    public class IntegrationTest
    {
        [TestMethod]
        public void InferenceTest()
        {
            var cold = new MembershipFunction("cold", "Cold", 0, 0, 10, 20);
            var hot = new MembershipFunction("hot", "Hot", 20, 30, 40, 40);
            var temp = new LinguisticVariable("water", "Water", new [] {cold, hot});

            var low = new MembershipFunction("low", "Low", 0, 0, 10, 20);
            var high = new MembershipFunction("high", "High", 20, 30, 40, 40);
            var power = new LinguisticVariable("power", "Power", new[] { low, high });
            
            var rule1 = new FuzzyRule(new[] { new FuzzyCondition(cold) }, new FuzzyConclusion(low));
            var rule2 = new FuzzyRule(new[] { new FuzzyCondition(hot) }, new FuzzyConclusion(high));
            
            var inference = new FuzzyInference(new[] { rule1, rule2  });
            foreach (var condition in rule1.Conditions)
            {
                condition.InputValue = 10;
            }
            foreach (var condition in rule2.Conditions)
            {
                condition.InputValue = 10;
            }
            Assert.AreEqual(5, inference.Infere());
            foreach (var condition in rule1.Conditions)
            {
                condition.InputValue = 35;
            }
            foreach (var condition in rule2.Conditions)
            {
                condition.InputValue = 35;
            }
            Assert.AreEqual(35, inference.Infere());
            foreach (var condition in rule1.Conditions)
            {
                condition.InputValue = 20;
            }
            foreach (var condition in rule2.Conditions)
            {
                condition.InputValue = 20;
            }
            Assert.AreEqual(0, inference.Infere());
        }
    }
}
