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


    [TestCase("Day4ExampleInput.txt", "X", ExpectedResult = 19)]
    public int GetXCoordinates(string fileName, string phrase)
    {
        string fileText = Text.FromFile("Year_2024", fileName);
        var grid = TextGrid.CreateGrid(fileText);
        var xCoordinates = TextGrid.GetCharacterCoordinates(grid, phrase[0]);
        return xCoordinates.Count();
    }

}
