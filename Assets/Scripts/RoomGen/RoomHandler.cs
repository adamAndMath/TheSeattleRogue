using UnityEngine;
using System.Collections;

public class RoomHandler : MonoBehaviour
{
    public static RoomHandler Instance { get; private set; }

    public Position min;
    public RoomInstance[,] rooms;

    public RoomInstance this[Position pos]
    {
        get
        {
            return rooms[pos.x - min.x, pos.y - min.y];
        }

        set
        {
            rooms[pos.x - min.x, pos.y - min.y] = value;
        }
    }

	void Awake ()
    {
        Instance = this;
	}
}
