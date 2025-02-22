﻿using FluentAssertions;
using AdventOfCode.Helpers;

namespace AdventOfCode.Year_2024
{
    internal class Day1
    {
        public enum CalculationMethod
        {
            RightMinusLeft,
            LeftMinusRight,
            Magnitude,
        }

        [TestCase("Day1Input.txt", CalculationMethod.RightMinusLeft, ExpectedResult = 877009)]
        [TestCase("Day1Input.txt", CalculationMethod.LeftMinusRight, ExpectedResult = -877009)]
        [TestCase("Day1Input.txt", CalculationMethod.Magnitude, ExpectedResult = 1590491)] // this is the correct answer
        public int Part_One(string fileName, CalculationMethod calculationMethod)
        {
            string fileText = Text.FromFile("Year_2024", fileName);
            GetLeftAndRightList(fileText, out var leftList, out var rightList);

            leftList = leftList.OrderBy(x => x);
            rightList = rightList.OrderBy(x => x);

            IEnumerable<int> differences = calculationMethod switch
            {
                CalculationMethod.RightMinusLeft => leftList.Select((x, i) => rightList.ElementAt(i) - x),
                CalculationMethod.LeftMinusRight => leftList.Select((x, i) => x - rightList.ElementAt(i)),
                CalculationMethod.Magnitude => leftList.Select((x, i) => int.Abs(x - rightList.ElementAt(i))),
                _ => [],
            };

            return differences.Sum();
        }

        [TestCase("Day1Input.txt", ExpectedResult = 22588371)]
        public int Part_Two(string fileName)
        {
            string fileText = Text.FromFile("Year_2024", fileName);
            GetLeftAndRightList(fileText, out var leftList, out var rightList);

            return leftList.Select(x => x * rightList.Count(y => y == x)).Sum();
        }

        private static void GetLeftAndRightList(string a, out IEnumerable<int> leftList, out IEnumerable<int> rightList)
        {
            var b = a.Split("\r\n");
            a[0].Should().Be('1');
            b.Should().HaveCountGreaterThan(0);

            // "123456  123456"
            // "123456  123456"
            // "123456  123456"
            // "123456  123456"
            var c = b.Select(x => x.Split("  ").Select(y => int.Parse(y)));

            // [int 123456, int 123456]
            // [int 123456, int 123456]
            // [int 123456, int 123456]
            // [int 123456, int 123456]
            leftList = c.Select(c => c.First());
            rightList = c.Select(c => c.Last());
        }
    }
}
