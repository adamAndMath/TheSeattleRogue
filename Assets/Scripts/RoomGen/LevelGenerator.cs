using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Room[] rooms;
    public Range pathDist;
    public Range exstraRooms;

    List<Position> positions = new List<Position>();

    [Serializable]
    public struct Range
    {
        public int min;
        public int max;

        public int Random { get { return UnityEngine.Random.Range(min, max + 1); } }
    }

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

        public static bool operator ==(Position a, Position b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Position a, Position b)
        {
            return a.x != b.x || a.y != b.y;
        }

        public static implicit operator Position(Room.Direction dir)
        {
            switch (dir)
            {
                case Room.Direction.Up: return new Position(0, 1);
                case Room.Direction.Down: return new Position(0, -1);
                case Room.Direction.Left: return new Position(-1, 0);
                case Room.Direction.Right: return new Position(1, 0);
                default: throw new Exception();
            }
        }
    }

    void Start()
    {
        Position position = new Position();
        int dir = UnityEngine.Random.Range(0, 4);
        positions.Add(position);
        int pathLength = pathDist.Random;

        for (int i = 0; i < pathLength; i++)
        {
            position += (Room.Direction) (1 << dir);
            positions.Add(position);
            dir = (dir + UnityEngine.Random.Range(0, 3))%4;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        for (int i = 1; i < positions.Count; i++)
        {
            Gizmos.DrawLine(new Vector3(positions[i - 1].x, positions[i - 1].y), new Vector3(positions[i].x, positions[i].y));
        }
    }
}
