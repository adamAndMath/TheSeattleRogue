using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int health;
    public GameObject valuta;
    public bool isDamaged;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void Damaged(int damageAmount)
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
        Instantiate(valuta, gameObject.transform.position, Quaternion.identity);
        DestroyObject(gameObject);
    }
}
