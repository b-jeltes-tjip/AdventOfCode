namespace AdventOfCode.Helpers;

internal struct Position(int y, int x)
{
    public int Y { get; set; } = y;
    public int X { get; set; } = x;

    public static Position operator +(Position a, (int Y, int X) b)
    {
        return new Position(a.Y + b.Y, a.X + b.X);
    }

    public static Position operator -(Position a, (int Y, int X) b)
    {
        return new Position(a.Y - b.Y, a.X - b.X);
    }
}
