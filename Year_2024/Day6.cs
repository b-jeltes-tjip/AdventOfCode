using AdventOfCode.Helpers;

namespace AdventOfCode.Year_2024;

internal class Day6
{
    //[TestCase("Day6Input.txt", ExpectedResult = 487)] // limited to 500 actions
    [TestCase("Day6Input.txt", ExpectedResult = 4656)] // 5273 actions taken
    [TestCase("Day6InputExample.txt", ExpectedResult = 41)] // 55 actions taken
    public int PartOne(string inputFile)
    {
        var puzzleInput = Text.FromFile("Year_2024", inputFile);

        var textGrid = TextGrid.CreateGrid(puzzleInput);

        (int Y, int X) = TextGrid.GetCharacterCoordinates(textGrid, '^').Single();

        var guard = new Guard(new Position(Y, X), '^');
        var grid = new Grid(textGrid);

        for (int i = 0; i < 5000000; i++)
        {
            var actionResult = guard.Act(grid);
            if (actionResult == Guard.ActionResult.ExitedGrid)
            {
                break;
            }
        }

        return grid.Data.SelectMany(x => x).Count(x => x == 'X');
    }

    internal class Grid(char[][] data)
    {
        public char[][] Data { get; } = data;

        public char this[int y, int x]
        {
            get => Data[y][x];
            set => Data[y][x] = value;
        }

        public int Height => Data.Length;

        public int Width => Data[0].Length;

        public bool IsInBounds(Position position) => IsInBounds(position.Y, position.X);

        public bool IsInBounds(int y, int x)
        {
            return y >= 0 && y < Height && x >= 0 && x < Width;
        }

        public void MarkPosition(Position position, char mark)
        {
            MarkPosition(position.Y, position.X, mark);
        }

        public void MarkPosition(int y, int x, char mark)
        {
            Data[y][x] = mark;
        }
    }

    internal class Guard(Position position, char directionCharacter)
    {
        public Position Position { get; private set; } = position;

        public (int Y, int X) CurrentDirection { get; private set; } = DirectionVectorMap[directionCharacter];

        private static readonly char[] DirectionCharacters = ['^', '>', 'v', '<'];

        private static readonly (int Y, int X)[] DirectionVectors = [(-1, 0), (0, 1), (1, 0), (0, -1)];

        private static readonly Dictionary<char, (int, int)> DirectionVectorMap = new()
        {
            ['^'] = (-1, 0),
            ['>'] = (0, 1),
            ['v'] = (1, 0),
            ['<'] = (0, -1)
        };

        public enum ActionResult
        {
            Moved, Rotated, ExitedGrid,
        }

        private (int Y, int X) GetNextDirection()
        {
            var currentIndex = Array.IndexOf(DirectionVectors, CurrentDirection);
            var nextIndex = (currentIndex + 1) % DirectionVectors.Length;
            return DirectionVectors[nextIndex];
        }

        public ActionResult Act(Grid grid)
        {
            if (!grid.IsInBounds(OneStepForward))
            {
                grid.MarkPosition(Position, mark: 'X');
                MoveForward();
                return ActionResult.ExitedGrid;
            }
            else if (DetectCollision(grid))
            {
                RotateRight90Degrees();
                return ActionResult.Rotated;
            }
            else
            {
                grid.MarkPosition(Position, mark: 'X');
                MoveForward();
                return ActionResult.Moved;
            }
        }

        public Position OneStepForward => Position + CurrentDirection;

        public char GetObjectInFront(Grid grid)
        {
            return grid[Position.Y + CurrentDirection.Y, Position.X + CurrentDirection.X];
        }

        public void MoveForward()
        {
            Position += CurrentDirection;
        }

        public void RotateRight90Degrees()
        {
            CurrentDirection = GetNextDirection();
        }

        public bool DetectCollision(Grid grid)
        {
            return GetObjectInFront(grid) == '#';
        }
    }
}
