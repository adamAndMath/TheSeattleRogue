using UnityEngine;

public abstract class PhysicsObject : MonoBehaviour
{
    private static int platform;

    protected static int Platform
    {
        get
        {
            if (platform == 0)
                platform = LayerMask.NameToLayer("Platform");

            return platform;
        }
    }

    protected readonly RaycastHit2D[] rayHits = new RaycastHit2D[16];
    protected Collider2D collider2D;

    protected virtual void Start()
    {
        collider2D = GetComponent<Collider2D>();
    }

    protected bool IsGrounded()
    {
        int size = collider2D.Cast(Vector2.down, rayHits, 0.1F);

        if (size > 0)
        {
            for (int i = 0; i < size; i++)
            {
                RaycastHit2D rayHit = rayHits[i];
                if (!rayHit.collider.isTrigger && rayHit.point.y - transform.position.y < 0 && Mathf.Abs(rayHit.normal.y) > 0 && CanCollide(rayHit, Vector2.down))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Moves the object along the x axis.
    /// </summary>
    /// <param name="move">displacement</param>
    /// <returns>true if collision occured</returns>
    public bool MoveHorizontal(float move)
    {
        Vector2 dir = Vector2.right * Mathf.Sign(move);
        move = Mathf.Abs(move);
        bool re = false;

        int size = collider2D.Cast(dir, rayHits, move);

        if (size > 0)
        {
            for (int i = 0; i < size; i++)
            {
                RaycastHit2D rayHit = rayHits[i];
                if (!rayHit.collider.isTrigger && move > rayHit.distance && Mathf.Abs(Vector2.Dot(rayHit.normal.normalized, dir)) > 0 && Vector2.Dot(rayHit.point - (Vector2)transform.position, dir) > 0 && CanCollide(rayHit, dir))
                {
                    move = rayHit.distance;
                    re = true;
                }
            }
        }

        transform.Translate(move * dir, Space.World);
        return re;
    }

    /// <summary>
    /// Moves the object along the x axis.
    /// </summary>
    /// <param name="moveMax">displacement</param>
    /// <param name="vertical">returns the vertical movement when moving across slopes</param>
    /// <returns>true if collision occured</returns>
    public bool MoveHorizontalSloped(float moveMax)
    {
        Vector2 dir = Vector2.right * Mathf.Sign(moveMax);
        moveMax = Mathf.Abs(moveMax);
        bool re = false;

        int size = collider2D.Cast(dir, rayHits, moveMax);

        if (size > 0)
        {
            float dist = moveMax;
            Vector2 normal = Vector2.zero;

            for (int i = 0; i < size; i++)
            {
                RaycastHit2D rayHit = rayHits[i];
                if (!rayHit.collider.isTrigger && dist > rayHit.distance &&
                    Mathf.Abs(Vector2.Dot(rayHit.normal.normalized, dir)) > 0 &&
                    Vector2.Dot(rayHit.point - (Vector2) transform.position, dir) > 0 && CanCollide(rayHit, dir))
                {
                    dist = rayHit.distance;
                    normal = rayHit.normal;
                    re = true;
                }
            }

            if (re)
            {
                transform.Translate((moveMax - dist)*normal.y/Mathf.Abs(normal.x)*Vector3.up, Space.World);
                MoveHorizontal(moveMax*dir.x);
            }
            else
            {
                transform.Translate(dist * dir, Space.World);
            }
        }
        else
        {
            transform.Translate(moveMax*dir, Space.World);
        }

        return re;
    }

    /// <summary>
    /// Moves the object along the y axis.
    /// </summary>
    /// <param name="move">displacement</param>
    /// <returns>true if collision occured</returns>
    public bool MoveVertical(float move)
    {
        Vector2 dir = Vector2.up * Mathf.Sign(move);
        move = Mathf.Abs(move);
        bool re = false;

        int size = collider2D.Cast(dir, rayHits, move);

        if (size > 0)
        {
            for (int i = 0; i < size; i++)
            {
                RaycastHit2D rayHit = rayHits[i];
                if (!rayHit.collider.isTrigger && move > rayHit.distance && Mathf.Abs(rayHit.normal.y) > 0 && Vector2.Dot(rayHit.point - (Vector2)transform.position, dir) > 0 && CanCollide(rayHit, dir))
                {
                    move = rayHit.distance;
                    re = true;
                }
            }
        }

        transform.Translate(move * dir, Space.World);
        return re;
    }

    /// <summary>
    /// Wrapper for canstant acceleration movement
    /// </summary>
    /// <param name="acceleration">the acceleration of the object</param>
    /// <param name="speed">the speed of the object. (Will be updated to the objects new speed)</param>
    /// <returns>delta position</returns>
    public static float ConstantAcceleration(float acceleration, ref float speed)
    {
        speed += Time.deltaTime*acceleration;
        return (speed - Time.deltaTime*acceleration/2)*Time.deltaTime;
    }

    protected virtual bool CanCollide(RaycastHit2D rayHit, Vector2 dir)
    {
        return rayHit.collider.gameObject.layer != Platform || (rayHit.normal.y > 0 && Vector2.Dot(dir, rayHit.normal) < 0 && !collider2D.OverlapPoint(rayHit.point));
    }
}
