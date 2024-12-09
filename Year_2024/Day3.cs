using System.Text.RegularExpressions;
using AdventOfCode.Helpers;
using FluentAssertions;

namespace AdventOfCode.Year_2024;

internal class Day3
{
    private static readonly string mulPattern = "mul\\([0-9]{1,3},[0-9]{1,3}\\)";

    private static readonly string doPattern = "do\\(\\)";

    private static readonly string dontPattern = "don't\\(\\)";

    private string getMulDoDontPattern() => $"({mulPattern}|{doPattern}|{dontPattern})";

    [TestCase("Day3Input.txt", ExpectedResult = 188192787)]
    public int PartOne(string fileName)
    {
        string fileText = Text.FromFile("Year_2024", fileName);

        var multiplicationCommands = Regex.Matches(fileText, mulPattern).Select(x => x.Value);

        var commaSeparatedNumbers = multiplicationCommands.Select(x => x.Trim('m', 'u', 'l', '(', ')'));
        var numbers = commaSeparatedNumbers.Select(x => x.Split(',')).Select(x => (int.Parse(x[0]), int.Parse(x[1])));
        return numbers.Select(x => x.Item1 * x.Item2).Sum();
    }

    [TestCase("Day3Input.txt", ExpectedResult = 113965544)]
    public int PartTwo(string fileName)
    {
        string fileText = Text.FromFile("Year_2024", fileName);
        var commands = Regex.Matches(fileText, getMulDoDontPattern()).Select(x => new Command(x.Value));

        return PerformMultiplications(commands).Sum();
    }

    [TestCase("mul(1,2)", ExpectedResult = true)]
    [TestCase("mul(12,34)", ExpectedResult = true)]
    [TestCase("mul(000,000)", ExpectedResult = true)]
    [TestCase("mul(123,456)", ExpectedResult = true)]
    [TestCase("mul(567,890)", ExpectedResult = true)]
    public bool MulRegexMatch(string input)
    {
        return Regex.Match(input, mulPattern).Success;
    }

    [TestCase("do()", ExpectedResult = true)]
    [TestCase("don't()", ExpectedResult = true)]
    public bool DoDontRegexMatch(string input)
    {
        return Regex.Match(input, doPattern).Success || Regex.Match(input, dontPattern).Success;
    }

    [Test]
    public void MulDoDontRegex()
    {
        string input = "do()aksmul(123,456)jdfdon't()mul(123,456)aosidfj";
        var matches = Regex.Matches(input, getMulDoDontPattern());
        var commands = matches.Select(match => new Command(match.Value));

        commands.Count(x => x.CommandType == CommandType.mul).Should().Be(2);
        commands.Count(x => x.CommandType == CommandType.@do).Should().Be(1);
        commands.Count(x => x.CommandType == CommandType.dont).Should().Be(1);
    }

    internal enum CommandType
    {
        mul,
        @do,
        dont,
        unknown
    }

    private static CommandType GetCommandType(string command)
    {
        return command switch
        {
            _ when Regex.Match(command, mulPattern).Success => CommandType.mul,
            _ when Regex.Match(command, doPattern).Success => CommandType.@do,
            _ when Regex.Match(command, dontPattern).Success => CommandType.dont,
            _ => CommandType.unknown,
        };
    }

    internal record Command
    {
        public CommandType CommandType { get; init; }
        public int? Number1 { get; init; }
        public int? Number2 { get; init; }

        public Command(string commandString)
        {
            CommandType = GetCommandType(commandString);
            if (CommandType == CommandType.unknown)
            {
                throw new ArgumentException($"Invalid command string: {commandString}");
            }

            if (CommandType == CommandType.mul)
            {
                var numbers = commandString.Trim('m', 'u', 'l', '(', ')').Split(',');
                Number1 = int.Parse(numbers[0]);
                Number2 = int.Parse(numbers[1]);
                if (!Number1.HasValue || !Number2.HasValue)
                {
                    throw new ArgumentException($"Invalid command string: {commandString}");
                }
            }
        }
    }

    internal static IEnumerable<int> PerformMultiplications(IEnumerable<Command> commands)
    {
        bool mulInstructionsEnabled = true;
        foreach (var command in commands)
        {
            switch (command.CommandType)
            {
                case CommandType.mul:
                    if (mulInstructionsEnabled)
                    {
                        if (command.Number1.HasValue && command.Number2.HasValue)
                        {
                            yield return (int)command.Number1 * (int)command.Number2;
                        }
                    }
                    break;
                case CommandType.@do:
                    mulInstructionsEnabled = true;
                    break;
                case CommandType.dont:
                    mulInstructionsEnabled = false;
                    break;
            }
        }
    }
}
