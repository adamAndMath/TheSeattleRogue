using UnityEngine;

public class CamMove : MonoBehaviour
{
    public Vector2 min;
    public Vector2 max;
    public Vector2 offset;
    private float z;
    private Camera cam;
    public float camMoveTime;

    private bool camIsMoving;
    private Vector3 camMoveFrom;
    private Vector3 camMoveTo;
    private float timer;

    void Start()
    {
        z = transform.position.z;
        cam = GetComponent<Camera>();
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
            }
        }

        if (playerPos.x > max.x)
        {
            Time.timeScale = 0;
            if (!camIsMoving)
            {
                camIsMoving = true;
                float delta = max.x - min.x;
                camMoveFrom = new Vector3(transform.position.x, transform.position.y, 0);
                camMoveTo = new Vector3(transform.position.x + delta, transform.position.y, 0);
                min.x += delta;
                max.x += delta;
            }
        }
        if (playerPos.x < min.x)
        {
            Time.timeScale = 0;
            if (!camIsMoving)
            {
                camIsMoving = true;
                float delta = max.x - min.x;
                camMoveFrom = new Vector3(transform.position.x, transform.position.y, 0);
                camMoveTo = new Vector3(transform.position.x - delta, transform.position.y, 0);
                min.x -= delta;
                max.x -= delta;
            }
        }
        if (playerPos.y > max.y)
        {
            Time.timeScale = 0;
            if (!camIsMoving)
            {
                camIsMoving = true;
                float delta = max.y - min.y;
                camMoveFrom = new Vector3(transform.position.x, transform.position.y, 0);
                camMoveTo = new Vector3(transform.position.x, transform.position.y + delta, 0);
                min.y += delta;
                max.y += delta;
            }
        }
        if (playerPos.y < min.y)
        {
            Time.timeScale = 0;
            if (!camIsMoving)
            {
                camIsMoving = true;
                float delta = max.y - min.y;
                camMoveFrom = new Vector3(transform.position.x, transform.position.y, 0);
                camMoveTo = new Vector3(transform.position.x, transform.position.y - delta, 0);
                min.y -= delta;
                max.y -= delta;
            }
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
