using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public GameObject door;
    public Room endRoom;
    public Room[] rooms;
    public Range pathDist;
    public Range exstraRooms;
    public SpriteRenderer wallPrefab;
    public SpriteRenderer backgroundPrefab;
    [EnumMask]
    public Room.Direction forceStartDir;

    public Position min;
    public Position max;

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

        public static implicit operator Room.Direction(Position pos)
        {
            if (pos.y > 0) return Room.Direction.Up;
            if (pos.y < 0) return Room.Direction.Down;
            if (pos.x > 0) return Room.Direction.Right;
            if (pos.x < 0) return Room.Direction.Left;
            throw new Exception();
        }
    }

    void Start()
    {
        Random.InitState(Random.Range(0, 1000000));
        RoomHandler.Instance.rooms = new RoomInstance[max.x - min.y + 1, max.y - min.y + 1];
        RoomHandler.Instance.min = min;
        int dir = Random.Range(0, 4);
        Position position;

        do
        {
            position = new Position();
            positions.Clear();
            positions.Add(position);
            int pathLength = pathDist.Random;

            for (int i = 0; i < pathLength; i++)
            {
                position += (Room.Direction)(1 << dir);
                positions.Add(position);
                dir = (dir + Random.Range(3, 6)) % 4;
            }
        } while (ValidatePath());

        Position endPosition = position;

        for (int i = 0; i < positions.Count; i++)
        {
            Room.Direction direction = 0;

            if (i > 0)
            {
                direction |= positions[i - 1] - positions[i];
            }
            else
            {
                direction |= forceStartDir;
            }

            if (i < positions.Count - 1)
            {
                direction |= positions[i + 1] - positions[i];
            }

            extraPositions.Add(positions[i], direction);
        }

        int extra = exstraRooms.Random;

        for (int i = 0; i < extra; i++)
        {
            do
            {
                dir = Random.Range(0, 3);

                KeyValuePair<Position, Room.Direction> extPos = extraPositions.ElementAt(Random.Range(0, extraPositions.Count));
                position = extPos.Key;
            } while (ValidateExtra(position, dir));
            extraPositions.Add(position + (Room.Direction)(1 << dir), (Room.Direction)(1 << (dir ^ 2)));
            extraPositions[position] |= (Room.Direction)(1 << dir);
        }

        foreach (var extraPosition in extraPositions)
        {
            if (extraPosition.Key == endPosition)
            {
                GenerateEndRoom(endRoom, extraPosition.Key, new Vector3(Room.RoomSize.x, Room.RoomSize.y), extraPosition.Value);
                continue;
            }

            Room[] avalableRooms = rooms.Where(r => RoomFit(r, extraPosition.Key, extraPosition.Value)).ToArray();
            Room room = avalableRooms[Random.Range(0, avalableRooms.Length - 1)];
            GenerateRoom(room, extraPosition.Key, new Vector3(Room.RoomSize.x, Room.RoomSize.y));
        }
    }

    private bool RoomFit(Room room, Position position, Room.Direction direction)
    {
        return room.size.x == 1 && room.size.y == 1 &&
               ((room.entrencesUp & 1) == 1) == ((direction & Room.Direction.Up) != 0) &&
               ((room.entrencesDown & 1) == 1) == ((direction & Room.Direction.Down) != 0) &&
               ((room.entrencesLeft & 1) == 1) == ((direction & Room.Direction.Left) != 0) &&
               ((room.entrencesRight & 1) == 1) == ((direction & Room.Direction.Right) != 0);
    }

    private bool ValidatePath()
    {
        for (int i = 0; i < positions.Count; i++)
        {
            for (int j = i + 1; j < positions.Count; j++)
            {
                if (positions[i] == positions[j])
                    return true;
            }

            if (positions[i].x < min.x || positions[i].x > max.x ||
                positions[i].y < min.y || positions[i].y > max.y)
                return true;
        }

        return false;
    }

    private bool ValidateExtra(Position pos, int dir)
    {
        Position newPos = pos + (Room.Direction) (1 << dir);
        return extraPositions.ContainsKey(newPos) ||
               newPos.x < min.x || newPos.x > max.x ||
               newPos.y < min.y || newPos.y > max.y;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        foreach (var position in extraPositions)
        {
            for (int i = 0; i < 4; i++)
            {
                if (((int)position.Value & (1 << i)) == 0)
                    continue;

                Position posTo = position.Key + (Room.Direction)(1 << i);
                Vector3 from = new Vector3(position.Key.x, position.Key.y);
                Vector3 to = new Vector3(posTo.x, posTo.y);
                Gizmos.DrawLine(from, to);
            }
        }
    }

    private void GenerateEndRoom(Room room, Position pos, Vector3 size, Room.Direction directions)
    {
        GameObject roomObject = new GameObject(room.name, typeof(RoomInstance));
        roomObject.transform.position = new Vector3(pos.x * size.x, pos.y * size.y);
        roomObject.GetComponent<RoomInstance>().room = room;
        RoomHandler.Instance[pos] = roomObject.GetComponent<RoomInstance>();
        roomObject.SetActive(false);

        SpriteRenderer background = Instantiate(backgroundPrefab);
        background.transform.SetParent(roomObject.transform);
        background.transform.localPosition = size / 2 - new Vector3(1, 1);
        background.GetComponent<SpriteRenderer>().sprite = room.background;

        GameObject door = Instantiate(this.door);
        door.transform.SetParent(roomObject.transform);
        door.transform.localPosition = new Vector3(size.x/2, 1);

        Position position = new Position();

        GenerateWall(room, roomObject, new Position(-1, -1), 15, 1);
        GenerateWall(room, roomObject, new Position(room.RealSize.x, -1), 15, 1);
        GenerateWall(room, roomObject, new Position(-1, room.RealSize.y), 15, 1);
        GenerateWall(room, roomObject, new Position(room.RealSize.x, room.RealSize.y), 15, 1);

        GenerateSide((directions & Room.Direction.Left) != 0, room, roomObject, -1, 1);
        GenerateSide((directions & Room.Direction.Right) != 0, room, roomObject, room.RealSize.x, -1);
        GenerateTop((directions & Room.Direction.Down) != 0, room, roomObject, -1, 1);
        GenerateTop((directions & Room.Direction.Up) != 0, room, roomObject, room.RealSize.y, -1);

        foreach (var column in room.columns)
        {
            foreach (var posData in column.data)
            {
                if (posData.wallID == -1)
                {
                    GeneratePlatform(room, roomObject, position);
                }
                else if (posData.wallID == -2)
                {
                    SpriteRenderer spike = Instantiate(room.spike);
                    spike.transform.SetParent(roomObject.transform);
                    spike.transform.localPosition = new Vector3(position.x, position.y);
                    int dir = room.GetWallDir(position);

                    if ((dir & (int)Room.Direction.Down) == 0)
                    {
                        if ((dir & (int)Room.Direction.Left) != 0)
                            spike.transform.localRotation = Quaternion.Euler(0, 0, 270);
                        else if ((dir & (int)Room.Direction.Right) != 0)
                            spike.transform.localRotation = Quaternion.Euler(0, 0, 90);
                        else if ((dir & (int)Room.Direction.Up) != 0)
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
                        slope.transform.localPosition = new Vector3(position.x, position.y);
                        slope.GetComponent<SpriteRenderer>().sprite = room.walls[posData.wallID - 1].GetSlope(dir);
                        EdgeCollider2D col = slope.GetComponent<EdgeCollider2D>();

                        Vector2 point = new Vector2(
                            (dir & (int)Room.Direction.Up) != 0 ? 0.5F : -0.5F,
                            (dir & (int)Room.Direction.Right) != 0 ? -0.5F : 0.5F);

                        col.points = new[] { point, -point };
                    }
                    else
                    {
                        GenerateWall(room, roomObject, position, room.GetWallDir(position, posData.wallID), posData.wallID);
                    }
                }

                position.y++;
            }

            position.y = 0;
            position.x++;
        }

        foreach (var spawner in room.spawners)
        {
            if (spawner.spawnMask == 0)
                continue;

            int i;

            do
            {
                i = Random.Range(0, 30);
            } while (((1 << i) & spawner.spawnMask) == 0);

            Enemy enemy = Instantiate(room.spawnables[i]);
            enemy.transform.SetParent(roomObject.transform, false);
            enemy.transform.localPosition += new Vector3(spawner.position.x, spawner.position.y);
        }
    }

    private void GenerateRoom(Room room, Position pos, Vector3 size)
    {
        GameObject roomObject = new GameObject(room.name, typeof(RoomInstance));
        roomObject.transform.position = new Vector3(pos.x * size.x, pos.y * size.y);
        roomObject.GetComponent<RoomInstance>().room = room;
        RoomHandler.Instance[pos] = roomObject.GetComponent<RoomInstance>();
        roomObject.SetActive(false);

        SpriteRenderer background = Instantiate(backgroundPrefab);
        background.transform.SetParent(roomObject.transform);
        background.transform.localPosition = size/2 - new Vector3(1, 1);
        background.GetComponent<SpriteRenderer>().sprite = room.background;

        Position position = new Position();

        GenerateWall(room, roomObject, new Position(-1, -1), 15, 1);
        GenerateWall(room, roomObject, new Position(room.RealSize.x, -1), 15, 1);
        GenerateWall(room, roomObject, new Position(-1, room.RealSize.y), 15, 1);
        GenerateWall(room, roomObject, new Position(room.RealSize.x, room.RealSize.y), 15, 1);

        GenerateSide(room.entrencesLeft == 1, room, roomObject, -1, 1);
        GenerateSide(room.entrencesRight == 1, room, roomObject, room.RealSize.x, -1);
        GenerateTop(room.entrencesDown == 1, room, roomObject, -1, 1);
        GenerateTop(room.entrencesUp == 1, room, roomObject, room.RealSize.y, -1);

        foreach (var column in room.columns)
        {
            foreach (var posData in column.data)
            {
                if (posData.wallID == -1)
                {
                    GeneratePlatform(room, roomObject, position);
                }
                else if (posData.wallID == -2)
                {
                    SpriteRenderer spike = Instantiate(room.spike);
                    spike.transform.SetParent(roomObject.transform);
                    spike.transform.localPosition = new Vector3(position.x, position.y);
                    int dir = room.GetWallDir(position);

                    if ((dir & (int) Room.Direction.Down) == 0)
                    {
                        if ((dir & (int) Room.Direction.Left) != 0)
                            spike.transform.localRotation = Quaternion.Euler(0, 0, 270);
                        else if ((dir & (int) Room.Direction.Right) != 0)
                            spike.transform.localRotation = Quaternion.Euler(0, 0, 90);
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
                        slope.transform.localPosition = new Vector3(position.x, position.y);
                        slope.GetComponent<SpriteRenderer>().sprite = room.walls[posData.wallID - 1].GetSlope(dir);
                        EdgeCollider2D col = slope.GetComponent<EdgeCollider2D>();

                        Vector2 point = new Vector2(
                            (dir & (int) Room.Direction.Up) != 0 ? 0.5F : -0.5F,
                            (dir & (int) Room.Direction.Right) != 0 ? -0.5F : 0.5F);

                        col.points = new[] { point, -point };
                    }
                    else
                    {
                        GenerateWall(room, roomObject, position, room.GetWallDir(position, posData.wallID), posData.wallID);
                    }
                }
                
                position.y++;
            }
            
            position.y = 0;
            position.x++;
        }

        foreach (var spawner in room.spawners)
        {
            if (spawner.spawnMask == 0)
                continue;

            int i;

            do
            {
                i = Random.Range(0, 30);
            } while (((1 << i) & spawner.spawnMask) == 0);

            Enemy enemy = Instantiate(room.spawnables[i]);
            enemy.transform.SetParent(roomObject.transform, false);
            enemy.transform.localPosition += new Vector3(spawner.position.x, spawner.position.y);
        }
    }

    private static void GeneratePlatform(Room room, GameObject roomObject, Position position)
    {
        SpriteRenderer platform = Instantiate(room.platform);
        platform.transform.SetParent(roomObject.transform);
        platform.transform.localPosition = new Vector3(position.x, position.y);
    }

    private void GenerateSide(bool entrence, Room room, GameObject roomObject, int x, int check)
    {
        int dir = 1 << (2 - check);
        int antiDir = 1 << (2 + check);

        if (entrence)
        {
            GenerateWall(room, roomObject, new Position(x, 0), 5 | antiDir | (room[new Position(x + check, 0)].wallID == 1 ? dir : 0), 1);
            GenerateWall(room, roomObject, new Position(x, 1), 5 | antiDir | (room[new Position(x + check, 1)].wallID == 1 ? dir : 0), 1);
            GenerateWall(room, roomObject, new Position(x, 2), 4 | antiDir | (room[new Position(x + check, 2)].wallID == 1 ? dir : 0), 1);
            GenerateWall(room, roomObject, new Position(x, 5), 1 | antiDir | (room[new Position(x + check, 5)].wallID == 1 ? dir : 0), 1);
            GenerateWall(room, roomObject, new Position(x, 6), 5 | antiDir | (room[new Position(x + check, 6)].wallID == 1 ? dir : 0), 1);
            GenerateWall(room, roomObject, new Position(x, 7), 5 | antiDir | (room[new Position(x + check, 7)].wallID == 1 ? dir : 0), 1);
        }
        else
        {
            for (int y = 0; y < 8; y++)
            {
                GenerateWall(room, roomObject, new Position(x, y), 7 | (room[new Position(x + check, y)].wallID == 1 ? 8 : 0), 1);
            }
        }
    }

    private void GenerateTop(bool entrence, Room room, GameObject roomObject, int y, int check)
    {
        int dir = 1 << (1 - check);
        int antiDir = 1 << (1 + check);

        if (entrence)
        {
            for (int x = 0; x < 5; x++)
            {
                GenerateWall(room, roomObject, new Position(x, y), 10 | antiDir | (room[new Position(x, y + check)].wallID == 1 ? dir : 0), 1);
                GenerateWall(room, roomObject, new Position(14 - x, y), 10 | antiDir | (room[new Position(14 - x, y + check)].wallID == 1 ? dir : 0), 1);
            }
            
            GenerateWall(room, roomObject, new Position(5, y), 2 | antiDir | (room[new Position(5, y + check)].wallID == 1 ? dir : 0), 1);
            GenerateWall(room, roomObject, new Position(9, y), 8 | antiDir | (room[new Position(9, y + check)].wallID == 1 ? dir : 0), 1);
            GeneratePlatform(room, roomObject, new Position(6, y));
            GeneratePlatform(room, roomObject, new Position(7, y));
            GeneratePlatform(room, roomObject, new Position(8, y));
        }
        else
        {
            for (int x = 0; x < 15; x++)
            {
                GenerateWall(room, roomObject, new Position(x, y), 10 | antiDir | (room[new Position(x, y + check)].wallID == 1 ? dir : 0), 1);
            }
        }
    }

    private void GenerateWall(Room room, GameObject roomObject, Position position, int direction, int wallID)
    {
        SpriteRenderer wall = Instantiate(wallPrefab);
        wall.transform.SetParent(roomObject.transform);
        wall.transform.localPosition = new Vector3(position.x, position.y);
        wall.sprite = room.walls[wallID - 1][direction];
    }
}
