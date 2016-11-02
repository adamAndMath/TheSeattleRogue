using UnityEngine;
using System.Collections;

public class ChestBehaviour : Enemy
{
    public GameObject orbs;
    public float spitSpeed;

    private Collider2D coll;
    private float timer;
    private int spitAmount;
    private bool isTriggered;
    private Animator animator;
    
	// Use this for initialization
    void Start ()
    {
        coll = GetComponent<Collider2D>();
        spitAmount = Random.Range(7, 11);
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () 
    {
	    if (coll.IsTouching(Player.Instance.GetComponent<Collider2D>()))
	    {
	        isTriggered = true;
            animator.SetBool("Opened", true);
	    }

	    if (isTriggered)
	    {
            if (spitAmount > 0 && timer <= 0)
            {
                Instantiate(orbs, transform.position, Quaternion.identity);
                spitAmount--;
                timer = spitSpeed;
            }
	        timer -= Time.deltaTime;
	    }
	}
}
