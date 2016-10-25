using System;
using UnityEngine;

public class Room : ScriptableObject
{
    public LevelGenerator.Position size;
    [EnumMask]
    public Direction entrences;
    public Column[] columns;
    public Wall[] walls;

    [Flags]
    public enum Direction { Up = 1, Down = 2, Left = 4, Right = 8 }

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

        public Sprite this[int dir]
        {
            get
            {
                switch (dir)
                {
                    case 0: return only;
                    case 1: return downOnly;
                    case 2: return upOnly;
                    case 3: return upDown;
                    case 4: return rightOnly;
                    case 5: return downRight;
                    case 6: return upRight;
                    case 7: return right;
                    case 8: return leftOnly;
                    case 9: return downLeft;
                    case 10: return upLeft;
                    case 11: return left;
                    case 12: return leftRight;
                    case 13: return down;
                    case 14: return up;
                    case 15: return center;
                    default: throw new Exception();
                }
            }
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
    }

    public RoomPosition this[LevelGenerator.Position pos]
    {
        get { return columns[pos.x].data[pos.y]; }
    }
}
