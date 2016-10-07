using UnityEngine;
using System.Collections;

public class PhysicsObject : MonoBehaviour
{
    protected Collider2D collider2D;
    private readonly RaycastHit2D[] rayHits = new RaycastHit2D[16];

    protected virtual void Start()
    {
        collider2D = GetComponent<Collider2D>();
    }

    protected bool IsGrounded()
    {
        int size = collider2D.Cast(Vector2.down, rayHits, 0.01F);

        if (size > 0)
        {
            for (int i = 0; i < size; i++)
            {
                RaycastHit2D rayHit = rayHits[i];
                if (!rayHit.collider.isTrigger && rayHit.point.y - transform.position.y < 0 && Mathf.Abs(rayHit.normal.y) / rayHit.normal.magnitude > 0.99F)
                    return true;
            }
        }

        return false;
    }

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
                if (!rayHit.collider.isTrigger && move > rayHit.distance && Mathf.Abs(Vector2.Dot(rayHit.normal.normalized, dir)) > 0.99F && Vector2.Dot(rayHit.point - (Vector2)transform.position, dir) > 0)
                {
                    move = rayHit.distance;
                    re = true;
                }
            }
        }

        transform.Translate(move * dir, Space.World);
        return re;
    }

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
                if (!rayHit.collider.isTrigger && move > rayHit.distance && Mathf.Abs(rayHit.normal.y) / rayHit.normal.magnitude > 0.99F && Vector2.Dot(rayHit.point - (Vector2)transform.position, dir) > 0)
                {
                    move = rayHit.distance;
                    re = true;
                }
            }
        }

        transform.Translate(move * dir, Space.World);
        return re;
    }
}
