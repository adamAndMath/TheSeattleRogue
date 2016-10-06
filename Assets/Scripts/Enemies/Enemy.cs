using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int health;
    public bool isDamaged;
    public ItemDrop[] drops;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

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
