using System;
using InferenceLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InferenceLibraryTests
{
    [TestClass]
    public class MembershipFunctionTest
    {
        [TestMethod]
        public void FuzzifyTest()
        {
            var membershipFunction = new MembershipFunction("old", "Old", 30, 50, 50, 70);

            Assert.AreEqual(0, membershipFunction.Fuzzify(0));
            Assert.AreEqual(0, membershipFunction.Fuzzify(100));
            Assert.AreEqual(0.5, membershipFunction.Fuzzify(40));
            Assert.AreEqual(0.5, membershipFunction.Fuzzify(60));
            Assert.AreEqual(1, membershipFunction.Fuzzify(50));
        }

        [TestMethod]
        public void LeftBorderFuzzifyTest()
        {
            var membershipFunction = new MembershipFunction("old", "Old", 30, 50, 70, 70);

            Assert.AreEqual(0, membershipFunction.Fuzzify(30));
            Assert.AreEqual(0, membershipFunction.Fuzzify(20));
        }

        [TestMethod]
        public void InfiniteFalseLeftBorderFuzzifyTest()
        {
            var membershipFunction = new MembershipFunction("old", "Old", 30, 50, 70, 70);

            Assert.AreEqual(0, membershipFunction.Fuzzify(30));
            Assert.AreEqual(0, membershipFunction.Fuzzify(20));
        }

        [TestMethod]
        public void InfiniteTrueLeftBorderFuzzifyTest()
        {
            var membershipFunction = new MembershipFunction("old", "Old", 30, 30, 70, 70) {IgnoreX1 = true, IncludeX2 = true};

            Assert.AreEqual(1, membershipFunction.Fuzzify(30));
            Assert.AreEqual(1, membershipFunction.Fuzzify(20));
        }
        [TestMethod]
        public void NonInfiniteLeftBorderIncludedFuzzifyTest()
        {
            var membershipFunction = new MembershipFunction("old", "Old", 30, 30, 70, 70) { IgnoreX1 = false, IncludeX2 = true };

            Assert.AreEqual(1, membershipFunction.Fuzzify(30));
            Assert.AreEqual(0, membershipFunction.Fuzzify(20));
        }

        [TestMethod]
        public void SingleValueFuzzifyTest()
        {
            var membershipFunction = new MembershipFunction("old", "Old", 100, 100, 100, 100);

            Assert.AreEqual(0, membershipFunction.Fuzzify(99));
            Assert.AreEqual(1, membershipFunction.Fuzzify(100));
            Assert.AreEqual(0, membershipFunction.Fuzzify(101));
        }

        [TestMethod]
        public void InfiniteFalseRightBorderFuzzifyTest()
        {
            var membershipFunction = new MembershipFunction("old", "Old", 30, 50, 70, 70);

            Assert.AreEqual(1, membershipFunction.Fuzzify(70));
            Assert.AreEqual(0, membershipFunction.Fuzzify(80));
        }

        [TestMethod]
        public void InfiniteTrueRightBorderFuzzifyTest()
        {
            var membershipFunction = new MembershipFunction("old", "Old", 30, 30, 70, 70) { IncludeX3 = true, IgnoreX4 = true };

            Assert.AreEqual(1, membershipFunction.Fuzzify(70));
            Assert.AreEqual(1, membershipFunction.Fuzzify(80));
        }
        [TestMethod]
        public void NonInfiniteRightBorderIncludedFuzzifyTest()
        {
            var membershipFunction = new MembershipFunction("old", "Old", 30, 30, 70, 70) { IgnoreX4 = false, IncludeX3 = true };

            Assert.AreEqual(1, membershipFunction.Fuzzify(70));
            Assert.AreEqual(0, membershipFunction.Fuzzify(80));
        }

        [TestMethod]
        public void InverseRightBorderFuzzifyTest()
        {
            var membershipFunction = new MembershipFunction("old", "Old", 30, 50, 70, 70) {Inverted = true};

            Assert.AreEqual(0, membershipFunction.Fuzzify(70));
        }

        [TestMethod]
        public void InverseIgnoredRightBorderFuzzifyTest()
        {
            var membershipFunction = new MembershipFunction("old", "Old", 30, 50, 70, 70) { Inverted = true, IgnoreX4 = true, IncludeX3 = true};

            Assert.AreEqual(0, membershipFunction.Fuzzify(70));
        }
    }
}
