using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class Enemy : MonoBehaviour
{
    public int health;
    public GameObject valuta;

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
    }

    public void Killed()
    {
        Instantiate(valuta, gameObject.transform.position, Quaternion.identity);
        DestroyObject(gameObject);
    }
}
