using System;

[Flags]
public enum Direction { Up = 1, Left = 2, Down = 4, Right = 8 }

[Serializable]
public struct Position
{
    public int x;
    public int y;

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override bool Equals(object obj)
    {
        return obj is Position && (this == (Position)obj);
    }

    public static Position operator +(Position a, Position b)
    {
        return new Position(a.x + b.x, a.y + b.y);
    }

    public static Position operator -(Position a, Position b)
    {
        return new Position(a.x - b.x, a.y - b.y);
    }

    public static bool operator ==(Position a, Position b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(Position a, Position b)
    {
        return a.x != b.x || a.y != b.y;
    }

    public static implicit operator Position(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up: return new Position(0, 1);
            case Direction.Down: return new Position(0, -1);
            case Direction.Left: return new Position(-1, 0);
            case Direction.Right: return new Position(1, 0);
            default: throw new Exception();
        }
    }

    public static implicit operator Direction(Position pos)
    {
        if (pos.y > 0) return Direction.Up;
        if (pos.y < 0) return Direction.Down;
        if (pos.x > 0) return Direction.Right;
        if (pos.x < 0) return Direction.Left;
        throw new Exception();
    }
}