using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int health;


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
}
