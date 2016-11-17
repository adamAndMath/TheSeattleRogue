using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : PhysicsObject
{
    public int health;
    public Sprite idleSprite;
    public ItemDrop[] drops;
    public float gravity;
    public float gravitySpeed;
    public int scoreGiven;
    public Vector2 damageDirection;

    public virtual void Damaged(int damageAmount, Vector3 direction)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Killed();
        }
        if (Player.Instance.Direction)
        {
            damageDirection = -direction;
            Debug.Log(damageDirection);
        }
        else
        {
            damageDirection = direction;
            Debug.Log(damageDirection);
        }
    }

    public virtual void Killed()
    {
        foreach (ItemDrop drop in drops)
        {
            drop.Drop(transform.parent, transform.position);
        }

        if (Player.data.kills != null)
        {
            Player.data.kills.Add(idleSprite);
        }
        else
        {
            Player.data.kills = new List<Sprite> { idleSprite };
        }

        Player.data.score += scoreGiven;
        DestroyObject(gameObject);
    }
}
