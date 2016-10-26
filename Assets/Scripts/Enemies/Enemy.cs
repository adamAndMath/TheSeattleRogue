using UnityEngine;

public class Enemy : PhysicsObject
{
    public int health;
    public Sprite idleSprite;
    public ItemDrop[] drops;
    public float gravity;
    public float gravitySpeed;

    public virtual void Damaged(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Killed();
        }
    }

    public void Killed()
    {
        foreach (ItemDrop drop in drops)
        {
            drop.Drop(transform.position);
        }
        Player.Instance.enemyDeathSprites.Add(idleSprite);

        DestroyObject(gameObject);
    }
}
