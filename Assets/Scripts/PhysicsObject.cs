﻿using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    private static int platform;

    private static int Platform
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
        int size = collider2D.Cast(Vector2.down, rayHits, 0.01F);

        if (size > 0)
        {
            for (int i = 0; i < size; i++)
            {
                RaycastHit2D rayHit = rayHits[i];
                if (!rayHit.collider.isTrigger && rayHit.point.y - transform.position.y < 0 && Mathf.Abs(rayHit.normal.y) > 0 && CanCollide(rayHit, Vector2.down))
                    return true;
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
        float move = moveMax = Mathf.Abs(moveMax);
        bool re = false;

        int size = collider2D.Cast(dir, rayHits, move);
        float vertical = 0;

        if (size > 0)
        {
            for (int i = 0; i < size; i++)
            {
                RaycastHit2D rayHit = rayHits[i];
                if (!rayHit.collider.isTrigger && move > rayHit.distance && Mathf.Abs(Vector2.Dot(rayHit.normal.normalized, dir)) > 0 && Vector2.Dot(rayHit.point - (Vector2)transform.position, dir) > 0 && CanCollide(rayHit, dir))
                {
                    if (Mathf.Abs(Vector2.Dot(rayHit.normal.normalized, dir)) < 0.71F)
                    {
                        vertical = -dir.x*(moveMax - rayHit.distance)*rayHit.normal.x/rayHit.normal.y;
                    }
                    else
                    {
                        vertical = 0;
                        move = rayHit.distance;
                    }

                    re = true;
                }
            }
        }

        transform.Translate(move * dir + vertical * Vector2.up, Space.World);
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

    protected bool CanCollide(RaycastHit2D rayHit, Vector2 dir)
    {
        return rayHit.collider.gameObject.layer != Platform || (rayHit.normal.y > 0 && Vector2.Dot(dir, rayHit.normal) < 0);
    }
}
