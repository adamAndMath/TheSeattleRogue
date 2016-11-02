using System;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    public Vector2 min;
    public Vector2 max;
    public Vector2 offset;
    public float extra;
    private float z;
    private Camera cam;
    public float camMoveTime;

    private bool camIsMoving;
    private Vector3 camMoveFrom;
    private Vector3 camMoveTo;
    private float timer;
    private RoomInstance currentRoom;
    private RoomInstance nextRoom;

    void Start()
    {
        z = transform.position.z;
        cam = GetComponent<Camera>();

        if (RoomHandler.Instance)
        {
            currentRoom = RoomHandler.Instance[new LevelGenerator.Position(
                Mathf.FloorToInt(transform.position.x/Room.RoomSize.x),
                Mathf.FloorToInt(transform.position.y/Room.RoomSize.y))];

            if (currentRoom)
                currentRoom.gameObject.SetActive(true);
        }
    }

    void LateUpdate()
    {
        Vector3 playerPos = Player.Instance.transform.position;
        Vector2 camSize = new Vector2(cam.orthographicSize * cam.pixelWidth / cam.pixelHeight, cam.orthographicSize);

        if (!camIsMoving)
        {
            transform.position = new Vector3(
                Mathf.Clamp(playerPos.x + offset.x, min.x + camSize.x, max.x - camSize.x),
                Mathf.Clamp(playerPos.y + offset.y, min.y + camSize.y, max.y - camSize.y), z);
        }
        else
        {
            timer += Time.unscaledDeltaTime;
            transform.position = Vector3.Lerp(camMoveFrom, camMoveTo, timer / camMoveTime) + Vector3.forward * z;
            if (timer >= camMoveTime)
            {
                Time.timeScale = 1;
                camIsMoving = false;
                timer = 0;

                if (currentRoom)
                    currentRoom.gameObject.SetActive(false);

                currentRoom = nextRoom;
            }
        }

        if (playerPos.x > max.x + extra)
        {
            Time.timeScale = 0;
            if (!camIsMoving)
            {
                TransitionRoom(new Vector3(max.x - min.x, 0));
            }
        }
        if (playerPos.x < min.x - extra)
        {
            Time.timeScale = 0;
            if (!camIsMoving)
            {
                TransitionRoom(new Vector2(min.x - max.x, 0));
            }
        }
        if (playerPos.y > max.y + extra)
        {
            Time.timeScale = 0;
            if (!camIsMoving)
            {
                TransitionRoom(new Vector2(0, max.y - min.y));
            }
        }
        if (playerPos.y < min.y - extra)
        {
            Time.timeScale = 0;
            if (!camIsMoving)
            {
                TransitionRoom(new Vector2(0, min.y - max.y));
            }
        }
    }

    private void TransitionRoom(Vector2 delta)
    {
        camIsMoving = true;
        camMoveFrom = new Vector3(transform.position.x, transform.position.y);
        camMoveTo = new Vector3(transform.position.x + delta.x, transform.position.y + delta.y);
        min += delta;
        max += delta;

        if (RoomHandler.Instance)
        {
            try
            {
                nextRoom = RoomHandler.Instance[new LevelGenerator.Position(
                    Mathf.FloorToInt(camMoveTo.x/Room.RoomSize.x),
                    Mathf.FloorToInt(camMoveTo.y/Room.RoomSize.y))];
            }
            catch (IndexOutOfRangeException)
            {
                nextRoom = null;
            }

            if (nextRoom)
                nextRoom.gameObject.SetActive(true);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(min.x, min.y), new Vector3(min.x, max.y));
        Gizmos.DrawLine(new Vector3(max.x, min.y), new Vector3(max.x, max.y));
        Gizmos.DrawLine(new Vector3(min.x, min.y), new Vector3(max.x, min.y));
        Gizmos.DrawLine(new Vector3(min.x, max.y), new Vector3(max.x, max.y));

        cam = GetComponent<Camera>();
        Vector2 camSize = new Vector2(1*cam.aspect, 1) * (cam.orthographicSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(min.x + camSize.x, min.y + camSize.y), new Vector3(min.x + camSize.x, max.y - camSize.y));
        Gizmos.DrawLine(new Vector3(max.x - camSize.x, min.y + camSize.y), new Vector3(max.x - camSize.x, max.y - camSize.y));
        Gizmos.DrawLine(new Vector3(min.x + camSize.x, min.y + camSize.y), new Vector3(max.x - camSize.x, min.y + camSize.y));
        Gizmos.DrawLine(new Vector3(min.x + camSize.x, max.y - camSize.y), new Vector3(max.x - camSize.x, max.y - camSize.y));
    }
}
