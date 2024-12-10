namespace AdventOfCode.Helpers
{
    internal static class TextGrid
    {
        internal static char[][] CreateGrid(string text)
        {
            var lines = text.Split("\r\n");
            return lines.Select(x => x.ToCharArray()).ToArray();
        }

        internal static bool SearchRight(char[][] grid, string phrase, (int y, int x) start)
        {
            return SearchForPhrase(grid, phrase, start, (0, 1));
        }

        internal static bool SearchLeft(char[][] grid, string phrase, (int y, int x) start)
        {
            return SearchForPhrase(grid, phrase, start, (0, -1));
        }

        internal static bool SearchUp(char[][] grid, string phrase, (int y, int x) start)
        {
            return SearchForPhrase(grid, phrase, start, (-1, 0));
        }

        internal static bool SearchDown(char[][] grid, string phrase, (int y, int x) start)
        {
            return SearchForPhrase(grid, phrase, start, (1, 0));
        }

        internal static bool SearchDiagonalDownRight(char[][] grid, string phrase, (int y, int x) start)
        {
            return SearchForPhrase(grid, phrase, start, (1, 1));
        }

        internal static IEnumerable<bool> SearchAllDirections(char[][] grid, string phrase, (int y, int x) start)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    yield return SearchForPhrase(grid, phrase, start, (y, x));
                }
            }
        }

        internal static bool SearchForPhrase(char[][] grid, string phrase, (int y, int x) start, (int y, int x) direction)
        {
            for (int k = 0; k < phrase.Length; k++)
            {
                (int y, int x) = (start.y + direction.y * k, start.x + direction.x * k);
                if (y < 0 || y >= grid.Length || x < 0 || x >= grid[y].Length)
                {
                    // Out of bounds
                    return false;
                }
                if (grid[y][x] != phrase[k])
                {
                    return false;
                }
            }
            return true;
        }

        internal static IEnumerable<(int, int)> GetCharacterCoordinates(char[][] grid, char c)
        {
            for (int y = 0; y < grid.Length; y++)
            {
                char[]? line = grid[y];
                for (int x = 0; x < line.Length; x++)
                {
                    char character = line[x];
                    if (character == c)
                    {
                        yield return (y, x);
                    }
                }
            }
        }
    }
}
