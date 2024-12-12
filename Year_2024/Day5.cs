using AdventOfCode.Helpers;

namespace AdventOfCode.Year_2024;

internal class Day5
{
    // Determine which updates are correct 
    // Get the middle number of each update
    // Get total of all middle numbers from correct updates

    [TestCase("InputDay5Example.txt", ExpectedResult = 143)]
    [TestCase("InputDay5.txt", ExpectedResult = 4135)]
    public int PartOne(string fileName)
    {
        string fileText = Text.FromFile("Year_2024", fileName);

        var groups = fileText.Split("\r\n\r\n");
        IEnumerable<OrderingRule> orderingRules = groups[0]
            .Split("\r\n")
            .Select(x => x.Split('|')
            .Select(y => int.Parse(y)))
            .Select(x => new OrderingRule { FirstPageNumber = x.First(), SecondPageNumber = x.Last() });

        IEnumerable<Update> updates = groups[1]
            .Split("\r\n")
            .Select(x => x.Split(",")
            .Select(y => int.Parse(y))).Select(z => new Update() { PageNumbers = z.ToList() });

        var updatesInRightOrder = updates.Where(x => x.IsInRightOrder(orderingRules));

        return updatesInRightOrder.Sum(x => x.MiddleNumber);
    }

    // Post-solution notes:
    // This solution simply applies the rules twice, however, I feel like this is not water-tight and that the fact that this works is a coincidence.
    // After the first pass corrects the order of the pages, the result may be incorrect according to previously applied rules because they are not taken in account by the rule that alters the order.

    // Example: 97, 29, 75, 47, 13
    // According to the third-to-last rule, 47 comes before 29.
    // Result: 97, 47, 29, 75, 13
    // This is incorrect according to an earlier rule: 75 comes before 47 
    // Because the rules are handled in sequence and that earlier rule is not applied again, the page numbers are now in incorrect order

    // An easy fix for this would be to check after a pass if the result is correct. If not, you run the rules again.
    // This is repeated until the page numbers follow the rules.

    // However, I feel that it should be possible to apply the rules once and get the correct result

    //[TestCase("InputDay5Example.txt", ExpectedResult = 135)] // Incorrect: rule-by-rule remove & insert
    //[TestCase("InputDay5Example.txt", ExpectedResult = 89)] // Incorrect: all rules with swapping
    //[TestCase("InputDay5Example.txt", ExpectedResult = 73)] // Incorrect: all rules with remove & insert
    //[TestCase("InputDay5Example.txt", ExpectedResult = 105)] // Incorrect: all rules with remove & insert with corrected insertion
    [TestCase("InputDay5Example.txt", ExpectedResult = 123)]
    //[TestCase("InputDay5.txt", ExpectedResult = 5285)]
    public int PartTwo(string fileName)
    {
        string fileText = Text.FromFile("Year_2024", fileName);

        var groups = fileText.Split("\r\n\r\n");
        IEnumerable<OrderingRule> orderingRules = groups[0]
            .Split("\r\n")
            .Select(x => x.Split('|')
            .Select(y => int.Parse(y)))
            .Select(x => new OrderingRule { FirstPageNumber = x.First(), SecondPageNumber = x.Last() });

        IEnumerable<Update> updates = groups[1]
            .Split("\r\n")
            .Select(x => x.Split(",")
            .Select(y => int.Parse(y))).Select(z => new Update() { PageNumbers = z.ToList() });

        var updatesInWrongOrder = updates.Where(x => !x.IsInRightOrder(orderingRules));

        var correctedUpdates = updatesInWrongOrder
            .Select(x => new Update() { PageNumbers = x.ApplyRules(orderingRules).ToList() })
            // initial solution
            //.Select(x => new Update() { PageNumbers = x.ApplyRules(orderingRules).ToList() })
            ;

        return correctedUpdates.Sum(x => x.MiddleNumber);
    }

    internal record OrderingRule
    {
        public int FirstPageNumber { get; init; }

        public int SecondPageNumber { get; init; }
    }

    internal record Update
    {
        public List<int> PageNumbers { get; init; } = [];

        public int MiddleNumber => PageNumbers.ElementAt(PageNumbers.Count / 2);

        public bool IsInRightOrder(IEnumerable<OrderingRule> orderingRules)
        {
            return orderingRules.All(IsInRightOrder);
        }

        public static bool IsInRightOrder(List<int> pageNumbers, IEnumerable<OrderingRule> orderingRules)
        {
            return orderingRules.All(rule => IsInRightOrder(pageNumbers, rule));
        }

        private bool IsInRightOrder(OrderingRule orderingRule)
        {
            var ruleNumbersArePresent = RuleIsRelevant(orderingRule);
            if (ruleNumbersArePresent)
            {
                var firstIndex = PageNumbers.IndexOf(orderingRule.FirstPageNumber);
                var secondIndex = PageNumbers.IndexOf(orderingRule.SecondPageNumber);
                return firstIndex < secondIndex;
            }

            return true;
        }

        internal static bool IsInRightOrder(List<int> pageNumbers, OrderingRule orderingRule)
        {
            if (RuleIsRelevant(pageNumbers, orderingRule))
            {
                var firstIndex = pageNumbers.IndexOf(orderingRule.FirstPageNumber);
                var secondIndex = pageNumbers.IndexOf(orderingRule.SecondPageNumber);
                return firstIndex < secondIndex;
            }

            return true;
        }

        internal IEnumerable<int> ApplyRules(IEnumerable<OrderingRule> orderingRules)
        {
            var resultingList = PageNumbers.ToList();
            // New solution: repeat until correct
            while (!IsInRightOrder(resultingList, orderingRules))
            {
                foreach (var rule in orderingRules)
                {
                    resultingList = ApplyRule(resultingList, rule).ToList();
                }
            }

            // initial solution
            //foreach (var rule in orderingRules)
            //{
            //    resultingList = ApplyRule(resultingList, rule).ToList();
            //}
            return resultingList;
        }

        internal IEnumerable<int> ApplyRule(List<int> pageNumbers, OrderingRule orderingRule)
        {
            var resultingList = pageNumbers.ToList();
            if (!RuleIsRelevant(resultingList, orderingRule))
            {
                // Do nothing
            }
            else if (IsInRightOrder(resultingList, orderingRule))
            {
                // Do nothing
            }
            else
            {
                // Swap places?
                //Swap(resultingList, resultingList.IndexOf(orderingRule.FirstPageNumber), resultingList.IndexOf(orderingRule.SecondPageNumber));
                // Move last number before first number?
                var firstPageNumber = orderingRule.FirstPageNumber;
                bool removeResult = resultingList.Remove(firstPageNumber);
                if (!removeResult)
                {
                    throw new Exception($"Could not remove second page number {firstPageNumber} from {string.Join(' ', pageNumbers)}");
                }
                resultingList.Insert(resultingList.IndexOf(orderingRule.SecondPageNumber), firstPageNumber);
            }
            return resultingList;
        }

        public static void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        private bool RuleIsRelevant(OrderingRule orderingRule)
            => RuleIsRelevant(PageNumbers, orderingRule);

        private static bool RuleIsRelevant(List<int> pageNumbers, OrderingRule orderingRule)
        {
            return pageNumbers.Contains(orderingRule.FirstPageNumber)
                && pageNumbers.Contains(orderingRule.SecondPageNumber);
        }
    }

    [TestCase(new int[] { 1, 2, 3 }, ExpectedResult = new int[] {3, 1, 2})] 
    public int[] RuleApplicationTest(int[] pageNumbers)
    {
        // 3 comes before 1
        var orderingRule = new OrderingRule { FirstPageNumber = 3, SecondPageNumber = 1 };
        var update = new Update { PageNumbers = pageNumbers.ToList() };
        return update.ApplyRule(pageNumbers.ToList(), orderingRule).ToArray();
    }

    [TestCase(new int[] { 1, 2, 3 }, ExpectedResult = new int[] {2, 3, 1})] 
    [TestCase(new int[] { 1, 5, 2, 8, 3 }, ExpectedResult = new int[] {2, 3, 1})] 
    public int[] MultipleRuleApplicationsTest(int[] pageNumbers)
    {
        // 3 comes before 1
        IEnumerable<OrderingRule> orderingRules =
            [
            new OrderingRule { FirstPageNumber = 3, SecondPageNumber = 1 },
            new OrderingRule { FirstPageNumber = 2, SecondPageNumber = 3 },
            new OrderingRule { FirstPageNumber = 999, SecondPageNumber = 998 },
            ];

        var update = new Update { PageNumbers = pageNumbers.ToList() };
        return update.ApplyRules(orderingRules).ToArray();
    }
}
