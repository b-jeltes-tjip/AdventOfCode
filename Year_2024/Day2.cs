using AdventOfCode.Helpers;

namespace AdventOfCode.Year_2024
{
    internal class Day2
    {
        [TestCase("Day2Input.txt", ExpectedResult = 246)]
        public int PartOne(string fileName)
        {
            string fileText = Text.FromFile("Year_2024", fileName);

            var reports = fileText
                .Split("\r\n")
                .Select(x => x.Split(' ')
                .Select(y => int.Parse(y)).ToList());

            return reports.Count(x => x.IsSafe());
        }

        //[TestCase("Day2Input.txt", ExpectedResult = 955)]
        //[TestCase("Day2Input.txt", ExpectedResult = 1000)]
        [TestCase("Day2Input.txt", ExpectedResult = 318)]
        public int PartTwo(string fileName)
        {
            string fileText = Text.FromFile("Year_2024", fileName);

            var reports = fileText
                .Split("\r\n")
                .Select(x => x.Split(' ')
                .Select(y => int.Parse(y)).ToList());

            return reports
                .Count(x => x.DampenedOptions().Any(y => y.IsSafe()));
        }
    }

    internal static class ReportExtensions
    {
        internal static bool IsSafe(this IEnumerable<int> report)
        {
            var descending = 
                (report.IsDescending() || report.IsAscending())
                && report.GetSteps().All(x => int.Abs(x) <= 3);
            return descending;
        }

        internal static IEnumerable<IEnumerable<int>> DampenedOptions(this IEnumerable<int> report)
        {
            for (int j = 0; j < report.Count(); j++)
            {
                yield return report.GetDampenedOption(j);
            }
        }

        internal static IEnumerable<int> GetDampenedOption(this IEnumerable<int> report, int index)
        {
            var list = report.ToList();
            list.RemoveAt(index);
            return list;
        }
    }
}
