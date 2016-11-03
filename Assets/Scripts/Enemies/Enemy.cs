﻿using UnityEngine;

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
        damageDirection = direction;
        Debug.Log(damageDirection);
    }

    public virtual void Killed()
    {
        foreach (ItemDrop drop in drops)
        {
            drop.Drop(transform.parent, transform.position);
        }
        Player.Instance.enemyDeathSprites.Add(idleSprite);
        Player.Instance.score += scoreGiven;
        DestroyObject(gameObject);
    }
}
