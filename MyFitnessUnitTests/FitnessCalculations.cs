using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyFitness.Calculations;

namespace MyFitnessUnitTests
{
    [TestClass]
    public class FitnessCalculationTests
    {
        private Fitness _fitness;

        public FitnessCalculationTests()
        {
            _fitness = new Fitness();
        }

        [TestMethod]
        public void TestMethod1()
        {
            List<Activities> activites = new List<Activities>();

            var response = _fitness.FourtyTwoDayCalculation()
        }
    }
}
