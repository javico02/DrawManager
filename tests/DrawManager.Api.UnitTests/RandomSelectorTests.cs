using DrawManager.Api.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DrawManager.Api.UnitTests
{
    public class RandomSelectorTests
    {
        /// <summary>
        /// Ensure that when randomly choosing items from an array, all items are chosen with roughly equal probability.
        /// </summary>
        [Theory]
        [InlineData(2000000, 5, 100, 90)]
        [InlineData(2000000, 1, 100, 75)]
        public void TakeRandom_Array_Uniformity(int trialsQty, int toSelectQty, int elementsQty, int inRangeQty)
        {
            // Arrange
            int expectedCount = trialsQty / (elementsQty / toSelectQty);
            var timesChosen = new int[elementsQty];
            var randomSelector = new RandomSelector();
            var source = new int[elementsQty];
            for (var i = 0; i < source.Length; i++)
                source[i] = i;

            // Act
            for (var trial = 0; trial < trialsQty; trial++)
            {
                foreach (var selected in randomSelector.TakeRandom(source, toSelectQty, elementsQty))
                    timesChosen[selected]++;
            }

            var avg = timesChosen.Average();
            var max = timesChosen.Max();
            var min = timesChosen.Min();
            var allowedDifference = expectedCount / elementsQty;
            var countInRange = timesChosen.Count(i => i >= expectedCount - allowedDifference && i <= expectedCount + allowedDifference);

            // Assert
            AssertBetween(avg, expectedCount - 2, expectedCount + 2, "Average");
            Assert.True(countInRange >= inRangeQty, string.Format("Not enough were in range: {0}", countInRange));
        }

        /// <summary>
        /// Ensure that when randomly choosing items from an IEnumerable that is not an IList, 
        /// all items are chosen with roughly equal probability.
        /// </summary>
        [Theory]
        [InlineData(2000000, 5, 100, 90)]
        [InlineData(2000000, 1, 100, 75)]
        public void TakeRandom_IEnumerable_Uniformity(int trialsQty, int toSelectQty, int elementsQty, int inRangeQty)
        {
            // Arrange
            int expectedCount = trialsQty / (elementsQty / toSelectQty);
            var timesChosen = new int[elementsQty];
            var randomSelector = new RandomSelector();

            // Act
            for (var trial = 0; trial < trialsQty; trial++)
            {
                foreach (var selected in randomSelector.TakeRandom(Range(0, elementsQty), toSelectQty, timesChosen.Length))
                    timesChosen[selected]++;
            }

            var avg = timesChosen.Average();
            var max = timesChosen.Max();
            var min = timesChosen.Min();
            var allowedDifference = expectedCount / elementsQty;
            var countInRange = timesChosen.Count(i => i >= expectedCount - allowedDifference && i <= expectedCount + allowedDifference);

            // Assert
            AssertBetween(avg, expectedCount - 2, expectedCount + 2, "Average");
            Assert.True(countInRange >= inRangeQty, string.Format("Not enough were in range: {0}", countInRange));
        }

        private IEnumerable<int> Range(int low, int count)
        {
            for (var i = low; i < low + count; i++)
                yield return i;
        }

        private static void AssertBetween(int x, int low, int high, String message)
        {
            Assert.True(x > low, string.Format("Value {0} is less than lower limit of {1}. {2}", x, low, message));
            Assert.True(x < high, string.Format("Value {0} is more than upper limit of {1}. {2}", x, high, message));
        }

        private static void AssertBetween(double x, double low, double high, String message)
        {
            Assert.True(x > low, string.Format("Value {0} is less than lower limit of {1}. {2}", x, low, message));
            Assert.True(x < high, string.Format("Value {0} is more than upper limit of {1}. {2}", x, high, message));
        }
    }
}
