namespace AdventOfCode.Helpers
{
    internal static class Collections
    {
        internal static bool IsDescending(this IEnumerable<int> collection)
        {
            var previous = collection.First();

            foreach (var number in collection.Skip(1))
            {
                if (previous <= number)
                {
                    return false;
                }
                previous = number;
            }
            return true;
        }

        internal static bool IsAscending(this IEnumerable<int> collection)
        {
            var previous = collection.First();

            foreach (var number in collection.Skip(1))
            {
                if (number <= previous)
                {
                    return false;
                }
                previous = number;
            }
            return true;
        }

        internal static IEnumerable<int> GetSteps(this IEnumerable<int> collection)
        {
            var previous = collection.First();
            foreach (var number in collection.Skip(1))
            {
                yield return number - previous;
                previous = number;
            }
        }
    }

    internal class CollectionHelpersTests
    {
        [TestCase(new int[] { 6, 5, 4, 3, 2, 1 }, ExpectedResult = true)]
        [TestCase(new int[] { 3, 2, 1, 0, -1, -2 }, ExpectedResult = true)]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6 }, ExpectedResult = false)]
        [TestCase(new int[] { 1, 1, 1, 1, 1, 1 }, ExpectedResult = false)]
        [TestCase(new int[] { 1, 2, 1, 2, 1, 2 }, ExpectedResult = false)]
        public bool IsDescendingTest(IEnumerable<int> collection)
        {
            return collection.IsDescending();
        }

        [TestCase(new int[] { 1, 2, 3, 4, 5, 6 }, ExpectedResult = true)]
        [TestCase(new int[] { -2, -1, 0, 1, 2, 3 }, ExpectedResult = true)]
        [TestCase(new int[] { 6, 5, 4, 3, 2, 1 }, ExpectedResult = false)]
        [TestCase(new int[] { 3, 2, 1, 0, -1, -2 }, ExpectedResult = false)]
        [TestCase(new int[] { 1, 1, 1, 1, 1, 1 }, ExpectedResult = false)]
        [TestCase(new int[] { 1, 2, 1, 2, 1, 2 }, ExpectedResult = false)]
        public bool IsAscendingTest(IEnumerable<int> collection)
        {
            return collection.IsAscending();
        }

        [TestCase(new int[] { 1, 2, 3, 4, 5, 6 }, ExpectedResult = new int[] { 1, 1, 1, 1, 1 })]
        [TestCase(new int[] { 6, 5, 4, 3, 2, 1 }, ExpectedResult = new int[] { -1, -1, -1, -1, -1 })]
        [TestCase(new int[] { 1, 1, 1, 1, 1, 1 }, ExpectedResult = new int[] { 0, 0, 0, 0, 0 })]
        [TestCase(new int[] { 1, 2, 1, 2, 1, 2 }, ExpectedResult = new int[] { 1, -1, 1, -1, 1 })]
        [TestCase(new int[] { 3, 2, 1, 0, -1, -2 }, ExpectedResult = new int[] { -1, -1, -1, -1, -1 })]
        public IEnumerable<int> GetStepsTest(IEnumerable<int> collection)
        {
            return collection.GetSteps();
        }
    }
}
