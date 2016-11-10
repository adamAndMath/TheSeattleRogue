using System;
using UnityEngine;

public class Room : ScriptableObject
{
    public static readonly Position RoomSize = new Position(16, 9);

    public Position size;
    //[EnumMask]
    public int entrencesUp;
    public int entrencesDown;
    public int entrencesLeft;
    public int entrencesRight;
    public Sprite background;
    public Column[] columns;
    public Wall[] walls;
    public Enemy[] spawnables;
    public Spawner[] spawners;
    public SpriteRenderer platform;
    public SpriteRenderer spike;

    public Position RealSize
    {
        get { return new Position(size.x*RoomSize.x - 1, size.y*RoomSize.y - 1); }
    }

    [Serializable]
    public struct Wall
    {
        public Sprite upLeft;
        public Sprite left;
        public Sprite downLeft;
        public Sprite up;
        public Sprite center;
        public Sprite down;
        public Sprite upRight;
        public Sprite right;
        public Sprite downRight;
        public Sprite upOnly;
        public Sprite upDown;
        public Sprite downOnly;
        public Sprite leftOnly;
        public Sprite leftRight;
        public Sprite rightOnly;
        public Sprite only;

        public Sprite slopeDownLeft;
        public Sprite slopeDownRight;
        public Sprite slopeUpLeft;
        public Sprite slopeUpRight;

        public Sprite this[int dir]
        {
            get
            {
                switch (dir)
                {
                    case 0: return only;
                    case 1: return downOnly;
                    case 2: return rightOnly;
                    case 3: return downRight;
                    case 4: return upOnly;
                    case 5: return upDown;
                    case 6: return upRight;
                    case 7: return right;
                    case 8: return leftOnly;
                    case 9: return downLeft;
                    case 10: return leftRight;
                    case 11: return down;
                    case 12: return upLeft;
                    case 13: return left;
                    case 14: return up;
                    case 15: return center;
                    default: throw new Exception();
                }
            }
        }

        public Sprite GetSlope(int dir)
        {
            return (dir & (int) Direction.Up) != 0
                ? ((dir & (int) Direction.Right) != 0 ? slopeUpRight : slopeUpLeft)
                : ((dir & (int) Direction.Right) != 0 ? slopeDownRight : slopeDownLeft);
        }
    }

    [Serializable]
    public class Column
    {
        public RoomPosition[] data;
    }

    [Serializable]
    public struct RoomPosition
    {
        public int wallID;
        public bool slope;
    }

    [Serializable]
    public struct Spawner
    {
        public Position position;
        public int spawnMask;
    }

    public RoomPosition this[Position pos]
    {
        get { return columns[pos.x].data[pos.y]; }
    }

    public int GetWallDir(Position pos)
    {
        int dir = 0;
        if (pos.y == RealSize.y - 1 || this[pos + Direction.Up].wallID > 0) dir |= (int)Direction.Up;
        if (pos.y == 0 || this[pos + Direction.Down].wallID > 0) dir |= (int)Direction.Down;
        if (pos.x == 0 || this[pos + Direction.Left].wallID > 0) dir |= (int)Direction.Left;
        if (pos.x == RealSize.x - 1 || this[pos + Direction.Right].wallID > 0) dir |= (int)Direction.Right;
        return dir;
    }

    public int GetWallDir(Position pos, int id)
    {
        int dir = 0;
        if (pos.y == RealSize.y - 1 || this[pos + Direction.Up].wallID == id) dir |= (int)Direction.Up;
        if (pos.y == 0 || this[pos + Direction.Down].wallID == id) dir |= (int)Direction.Down;
        if (pos.x == 0 || this[pos + Direction.Left].wallID == id) dir |= (int)Direction.Left;
        if (pos.x == RealSize.x - 1 || this[pos + Direction.Right].wallID == id) dir |= (int)Direction.Right;
        return dir;
    }
}
