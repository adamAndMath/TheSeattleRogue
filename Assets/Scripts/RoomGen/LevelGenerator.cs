using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public Room[] rooms;
    public Range pathDist;
    public Range exstraRooms;
    public SpriteRenderer wallPrefab;

    readonly List<Position> positions = new List<Position>();
    readonly Dictionary<Position, Room.Direction> extraPositions = new Dictionary<Position, Room.Direction>();

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
        int dir = Random.Range(0, 4);
        positions.Add(position);
        int pathLength = pathDist.Random;
        int extra = exstraRooms.Random;

        for (int i = 0; i < pathLength; i++)
        {
            position += (Room.Direction) (1 << dir);
            positions.Add(position);
            dir = (dir + Random.Range(3, 6))%4;
        }

        for (int i = 0; i < extra; i++)
        {
            do
            {
                dir = Random.Range(0, 3);

                if (Random.Range(0, 2) == 1 && extraPositions.Count > 0)
                {
                    KeyValuePair<Position, Room.Direction> extPos = extraPositions.ElementAt(Random.Range(0, extraPositions.Count));
                    position = extPos.Key;
                }
                else
                {
                    position = positions[Random.Range(0, positions.Count)];
                }
            } while (positions.Contains(position + (Room.Direction)(1 << dir)) || extraPositions.ContainsKey(position + (Room.Direction)(1 << dir)));
            extraPositions.Add(position + (Room.Direction)(1 << dir), (Room.Direction)(1 << (dir ^ 2)));
        }

        GenerateRoom(rooms[0], new Position(0, 0), new Vector3(rooms[0].size.x, rooms[0].size.y));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Quaternion rot = Quaternion.Euler(0, 0, 45);
        Quaternion rotInv = Quaternion.Euler(0, 0, -45);

        for (int i = 1; i < positions.Count; i++)
        {
            Vector3 from = new Vector3(positions[i - 1].x, positions[i - 1].y);
            Vector3 to = new Vector3(positions[i].x, positions[i].y);
            Gizmos.DrawLine(from, to);
            Gizmos.DrawLine((from + to) / 2, (from + to) / 2 + rot * (from - to) / 3);
            Gizmos.DrawLine((from + to) / 2, (from + to) / 2 + rotInv * (from - to) / 3);
        }

        Gizmos.color = Color.magenta;

        foreach (var position in extraPositions)
        {
            Position posTo = position.Key + position.Value;
            Vector3 from = new Vector3(position.Key.x, position.Key.y);
            Vector3 to = new Vector3(posTo.x, posTo.y);
            Gizmos.DrawLine(from, to);
        }
    }

    private void GenerateRoom(Room room, Position pos, Vector3 size)
    {
        GameObject roomObject = new GameObject(room.name);
        Position position = new Position();

        foreach (var column in room.columns)
        {
            foreach (var posData in column.data)
            {
                if (posData.wallID == -1)
                {
                    SpriteRenderer platform = Instantiate(room.platform);
                    platform.transform.SetParent(roomObject.transform);
                    platform.transform.localPosition = new Vector3(position.x - size.x / 2, position.y - size.y / 2);
                }
                else if (posData.wallID == -2)
                {
                    SpriteRenderer spike = Instantiate(room.spike);
                    spike.transform.SetParent(roomObject.transform);
                    spike.transform.localPosition = new Vector3(position.x - size.x / 2, position.y - size.y / 2);
                    int dir = room.GetWallDir(position);

                    if ((dir & (int) Room.Direction.Down) == 0)
                    {
                        if ((dir & (int) Room.Direction.Left) != 0)
                            spike.transform.localRotation = Quaternion.Euler(0, 0, 90);
                        else if ((dir & (int) Room.Direction.Right) != 0)
                            spike.transform.localRotation = Quaternion.Euler(0, 0, 270);
                        else if ((dir & (int) Room.Direction.Up) != 0)
                            spike.transform.localRotation = Quaternion.Euler(0, 0, 180);
                    }
                }
                else if (posData.wallID != 0)
                {
                    if (posData.slope)
                    {
                        int dir = room.GetWallDir(position, posData.wallID);
                        GameObject slope = new GameObject("Slope", typeof(SpriteRenderer), typeof(EdgeCollider2D));
                        slope.transform.SetParent(roomObject.transform);
                        slope.transform.localPosition = new Vector3(position.x - size.x / 2, position.y - size.y / 2);
                        slope.GetComponent<SpriteRenderer>().sprite = room.walls[posData.wallID - 1].GetSlope(dir);
                        EdgeCollider2D col = slope.GetComponent<EdgeCollider2D>();

                        Vector2 point = new Vector2(
                            (dir & (int) Room.Direction.Up) != 0 ? 0.5F : -0.5F,
                            (dir & (int) Room.Direction.Right) != 0 ? -0.5F : 0.5F);

                        col.points = new[] { point, -point };
                    }
                    else
                    {
                        SpriteRenderer wall = Instantiate(wallPrefab);
                        wall.transform.SetParent(roomObject.transform);
                        wall.transform.localPosition = new Vector3(position.x - size.x/2, position.y - size.y/2);
                        wall.sprite = room.walls[posData.wallID - 1][room.GetWallDir(position, posData.wallID)];
                    }
                }
                
                position.y++;
            }
            
            position.y = 0;
            position.x++;
        }
    }
}
