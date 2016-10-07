public class Enemy : PhysicsObject
{
    public int health;
    public bool isDamaged;
    public ItemDrop[] drops;

    public virtual void Damaged(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Killed();
        }
        isDamaged = true;
    }

    public void Killed()
    {
        foreach (ItemDrop drop in drops)
        {
            drop.Drop(transform.position);
        }

        DestroyObject(gameObject);
    }
}
