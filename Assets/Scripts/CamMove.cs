using UnityEngine;

public class CamMove : MonoBehaviour
{
    public Vector2 min;
    public Vector2 max;
    public Vector2 offset;
    private float z;
    private Camera cam;
    public float camMoveSpeed;
    private Vector2 startingMax;
    private Vector2 startingMin;
    private Vector2 deltaMax;
    private Vector2 deltaMin;

    private bool camIsMoving;
    private Vector3 camMovingPoint;

    void Start()
    {
        z = transform.position.z;
        cam = GetComponent<Camera>();
        startingMax.x = max.x;
        startingMax.y = max.y;
        startingMin.x = min.x;
        startingMin.y = min.y;
        deltaMax.x = max.x;
        deltaMax.y = max.y;
        deltaMin.x = min.x;
        deltaMin.y = min.y;
    }

    void LateUpdate()
    {
        Vector3 playerPos = Player.Instance.transform.position;
        Vector2 camSize = new Vector2(1 * cam.aspect, 1) * (cam.orthographicSize);

        if (!camIsMoving)
        {
            transform.position = new Vector3(
                Mathf.Clamp(playerPos.x + offset.x, min.x + camSize.x, max.x - camSize.x),
                Mathf.Clamp(playerPos.y + offset.y, min.y + camSize.y, max.y - camSize.y), z);
        }
        if (Player.Instance.transform.position.x > camSize.x + (deltaMax.x - startingMax.x))
        {
            Time.timeScale = 0;
            if (!camIsMoving)
            {
                camMovingPoint = new Vector3(transform.position.x + 2*camSize.x, transform.position.y, 0);
                camIsMoving = true;
                float delta = max.x - min.x;
                min.x += delta;
                max.x += delta;
            }
            else
            {
                transform.Translate(camMoveSpeed * Time.unscaledDeltaTime, 0, 0);
            }
            if (transform.position.x >= camMovingPoint.x)
            {
                camIsMoving = false;
                Time.timeScale = 1;
                deltaMax.x = max.x;
                deltaMin.x = min.x;
            }
        }
        if (Player.Instance.transform.position.x < -camSize.x + (deltaMin.x - startingMin.x))
        {
            Time.timeScale = 0;
            if (!camIsMoving)
            {
                camMovingPoint = new Vector3(transform.position.x - 2*camSize.x, transform.position.y, 0);
                camIsMoving = true;
                float delta = max.x - min.x;
                min.x -= delta;
                max.x -= delta;
            }
            else
            {
                transform.Translate(-camMoveSpeed * Time.unscaledDeltaTime, 0, 0);
            }
            if (transform.position.x <= camMovingPoint.x)
            {
                camIsMoving = false;
                Time.timeScale = 1;
                deltaMin.x = min.x;
                deltaMax.x = max.x;
            }
        }
        if (Player.Instance.transform.position.y > camSize.y + (deltaMax.y - startingMax.y))
        {
            Time.timeScale = 0;
            if (!camIsMoving)
            {
                camMovingPoint = new Vector3(transform.position.x, transform.position.y + 2*camSize.y, 0);
                camIsMoving = true;
                float delta = max.y - min.y;
                min.y += delta;
                max.y += delta;
            }
            else
            {
                transform.Translate(0, camMoveSpeed * Time.unscaledDeltaTime, 0);
            }
            if (transform.position.y >= camMovingPoint.y)
            {
                camIsMoving = false;
                Time.timeScale = 1;
                deltaMax.y = max.y;
                deltaMin.y = min.y;
            }
        }
        if (Player.Instance.transform.position.y < -camSize.y + (deltaMin.y - startingMin.y))
        {
            Time.timeScale = 0;
            if (!camIsMoving)
            {
                camMovingPoint = new Vector3(transform.position.x, transform.position.y - 2*camSize.y, 0);
                camIsMoving = true;
                float delta = max.y - min.y;
                min.y -= delta;
                max.y -= delta;
            }
            else
            {
                transform.Translate(0, -camMoveSpeed*Time.unscaledDeltaTime,0);
            }
            if (transform.position.y <= camMovingPoint.y)
            {
                camIsMoving = false;
                Time.timeScale = 1;
                deltaMin.y = min.y;
                deltaMax.y = max.y;
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
