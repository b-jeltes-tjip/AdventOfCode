using AdventOfCode.Helpers;

namespace AdventOfCode.Year_2024;

internal class Day4
{
    [TestCase("Day4ExampleInput.txt", "XMAS", ExpectedResult = 18)]
    [TestCase("Day4Input.txt", "XMAS", ExpectedResult = 2571)]
    public int PartOne(string fileName, string phrase)
    {
        string fileText = Text.FromFile("Year_2024", fileName);

        var grid = TextGrid.CreateGrid(fileText);

        var xCoordinates = TextGrid.GetCharacterCoordinates(grid, phrase[0]);

        return xCoordinates.Sum(coord => TextGrid.SearchAllDirections(grid, phrase, coord).Count(match => match));
    }

    [TestCase("Day4ExampleInput.txt", "MAS", ExpectedResult = 9)]
    [TestCase("Day4Input.txt", "MAS", ExpectedResult = 1992)]
    public int PartTwo(string fileName, string phrase)
    {
        string fileText = Text.FromFile("Year_2024", fileName);

        var grid = TextGrid.CreateGrid(fileText);

        return SearchFor_X_MAS(grid, phrase);
    }


    [TestCase("Day4ExampleInput.txt", "X", ExpectedResult = 19)]
    public int GetXCoordinates(string fileName, string phrase)
    {
        string fileText = Text.FromFile("Year_2024", fileName);
        var grid = TextGrid.CreateGrid(fileText);
        var xCoordinates = TextGrid.GetCharacterCoordinates(grid, phrase[0]);
        return xCoordinates.Count();
    }

    private static int SearchFor_X_MAS(char[][] grid, string phrase)
    {
        var aCoordinates = TextGrid.GetCharacterCoordinates(grid, 'A');
        int matches = 0;
        foreach ((int y, int x) in aCoordinates)
        {
            // Only search diagonally
            // In both directions
            bool isMatch =
                (TextGrid.SearchForPhrase(grid, phrase, (y - 1, x - 1), (1, 1))
                || TextGrid.SearchForPhrase(grid, new string(phrase.Reverse().ToArray()), (y - 1, x - 1), (1, 1))) 
                &
                (TextGrid.SearchForPhrase(grid, phrase, (y + 1, x - 1), (-1, 1))
                || TextGrid.SearchForPhrase(grid, new string (phrase.Reverse().ToArray()), (y + 1, x - 1), (-1, 1)));
            if (isMatch)
                matches += 1;
        }
        return matches;
    }
}
