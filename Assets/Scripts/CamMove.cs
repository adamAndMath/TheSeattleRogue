using UnityEngine;

public class CamMove : MonoBehaviour
{
    public Vector2 min;
    public Vector2 max;
    public Vector2 offset;
    private float z;

    void Start()
    {
        z = transform.position.z;
    }

    void Update()
    {
        Vector3 playerPos = Player.Instance.transform.position;
        transform.position = new Vector3(Mathf.Clamp(playerPos.x + offset.x, min.x, max.x), Mathf.Clamp(playerPos.y + offset.y, min.y, max.y), z);
    }
}
