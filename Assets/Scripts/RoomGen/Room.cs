using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2 min;
    public Vector2 max;
    [EnumMask]
    public Direction entrences;

    [System.Flags]
    public enum Direction { Up = 1, Down = 2, Left = 4, Right = 8 }

    void OnDrawGismosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(min.x, min.y), new Vector3(min.x, max.y));
        Gizmos.DrawLine(new Vector3(max.x, min.y), new Vector3(max.x, max.y));
        Gizmos.DrawLine(new Vector3(min.x, min.y), new Vector3(max.x, min.y));
        Gizmos.DrawLine(new Vector3(min.x, max.y), new Vector3(max.x, max.y));
    }
}
