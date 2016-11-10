using UnityEngine;
using Random = UnityEngine.Random;

public class RomSerialGenerator : MonoBehaviour
{
    public GameObject door;
    public Room endRoom;
    public Room[] rooms;
    [EnumMask]
    public Room.Direction endDirections;
    public SpriteRenderer wallPrefab;
    public SpriteRenderer backgroundPrefab;

    void Start()
    {
        LevelGenerator.Position position = new LevelGenerator.Position();

        for (; position.x < rooms.Length; position.x++)
        {
            GenerateRoom(rooms[position.x], position, new Vector3(Room.RoomSize.x, Room.RoomSize.y));
        }

        GenerateEndRoom(endRoom, position, new Vector3(Room.RoomSize.x, Room.RoomSize.y), endDirections);
    }

    private void GenerateEndRoom(Room room, LevelGenerator.Position pos, Vector3 size, Room.Direction directions)
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
        door.transform.localPosition = new Vector3(size.x / 2 - 0.5F, 0.5F);

        LevelGenerator.Position position = new LevelGenerator.Position();

        GenerateWall(room, roomObject, new LevelGenerator.Position(-1, -1), 15, 1);
        GenerateWall(room, roomObject, new LevelGenerator.Position(room.RealSize.x, -1), 15, 1);
        GenerateWall(room, roomObject, new LevelGenerator.Position(-1, room.RealSize.y), 15, 1);
        GenerateWall(room, roomObject, new LevelGenerator.Position(room.RealSize.x, room.RealSize.y), 15, 1);

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

    private void GenerateRoom(Room room, LevelGenerator.Position pos, Vector3 size)
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

        LevelGenerator.Position position = new LevelGenerator.Position();

        GenerateWall(room, roomObject, new LevelGenerator.Position(-1, -1), 15, 1);
        GenerateWall(room, roomObject, new LevelGenerator.Position(room.RealSize.x, -1), 15, 1);
        GenerateWall(room, roomObject, new LevelGenerator.Position(-1, room.RealSize.y), 15, 1);
        GenerateWall(room, roomObject, new LevelGenerator.Position(room.RealSize.x, room.RealSize.y), 15, 1);

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

    private static void GeneratePlatform(Room room, GameObject roomObject, LevelGenerator.Position position)
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
            GenerateWall(room, roomObject, new LevelGenerator.Position(x, 0), 5 | antiDir | (room[new LevelGenerator.Position(x + check, 0)].wallID == 1 ? dir : 0), 1);
            GenerateWall(room, roomObject, new LevelGenerator.Position(x, 1), 5 | antiDir | (room[new LevelGenerator.Position(x + check, 1)].wallID == 1 ? dir : 0), 1);
            GenerateWall(room, roomObject, new LevelGenerator.Position(x, 2), 4 | antiDir | (room[new LevelGenerator.Position(x + check, 2)].wallID == 1 ? dir : 0), 1);
            GenerateWall(room, roomObject, new LevelGenerator.Position(x, 5), 1 | antiDir | (room[new LevelGenerator.Position(x + check, 5)].wallID == 1 ? dir : 0), 1);
            GenerateWall(room, roomObject, new LevelGenerator.Position(x, 6), 5 | antiDir | (room[new LevelGenerator.Position(x + check, 6)].wallID == 1 ? dir : 0), 1);
            GenerateWall(room, roomObject, new LevelGenerator.Position(x, 7), 5 | antiDir | (room[new LevelGenerator.Position(x + check, 7)].wallID == 1 ? dir : 0), 1);
        }
        else
        {
            for (int y = 0; y < 8; y++)
            {
                GenerateWall(room, roomObject, new LevelGenerator.Position(x, y), 7 | (room[new LevelGenerator.Position(x + check, y)].wallID == 1 ? 8 : 0), 1);
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
                GenerateWall(room, roomObject, new LevelGenerator.Position(x, y), 10 | antiDir | (room[new LevelGenerator.Position(x, y + check)].wallID == 1 ? dir : 0), 1);
                GenerateWall(room, roomObject, new LevelGenerator.Position(14 - x, y), 10 | antiDir | (room[new LevelGenerator.Position(14 - x, y + check)].wallID == 1 ? dir : 0), 1);
            }

            GenerateWall(room, roomObject, new LevelGenerator.Position(5, y), 2 | antiDir | (room[new LevelGenerator.Position(5, y + check)].wallID == 1 ? dir : 0), 1);
            GenerateWall(room, roomObject, new LevelGenerator.Position(9, y), 8 | antiDir | (room[new LevelGenerator.Position(9, y + check)].wallID == 1 ? dir : 0), 1);
            GeneratePlatform(room, roomObject, new LevelGenerator.Position(6, y));
            GeneratePlatform(room, roomObject, new LevelGenerator.Position(7, y));
            GeneratePlatform(room, roomObject, new LevelGenerator.Position(8, y));
        }
        else
        {
            for (int x = 0; x < 15; x++)
            {
                GenerateWall(room, roomObject, new LevelGenerator.Position(x, y), 10 | antiDir | (room[new LevelGenerator.Position(x, y + check)].wallID == 1 ? dir : 0), 1);
            }
        }
    }

    private void GenerateWall(Room room, GameObject roomObject, LevelGenerator.Position position, int direction, int wallID)
    {
        SpriteRenderer wall = Instantiate(wallPrefab);
        wall.transform.SetParent(roomObject.transform);
        wall.transform.localPosition = new Vector3(position.x, position.y);
        wall.sprite = room.walls[wallID - 1][direction];
    }
}
