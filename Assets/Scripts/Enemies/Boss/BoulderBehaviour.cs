using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class BoulderBehaviour : PhysicsObject
{
    private Collider2D coll;

    public float fallSpeed;
    public float flySpeed;

    private float directionX;
    private float directionY;
    private bool hasBeenTouched;
    private bool hasHitRightState;
	// Use this for initialization
    protected override void Start()
    {
        base.Start();
        BossBehaviour.Instance.bouldersInScene.Add(gameObject);
        coll = GetComponent<Collider2D>();
        directionX = Random.Range(0.2f, 1.0f);
        directionY = Random.Range(0.2f, 1.0f);
    }
	// Update is called once per frame
	void Update () 
    {
	    if (!IsGrounded() && !hasBeenTouched)
	    {
	        MoveVertical(fallSpeed*Time.deltaTime);

	        if (coll.IsTouching(Player.Instance.GetComponent<Collider2D>()))
	        {
	            Player.Instance.Damaged(1);
	        }
	    }
	    else
	    {
	        if (coll.IsTouching(BossBehaviour.Instance.GetComponent<Collider2D>()))
	        {
	            hasBeenTouched = true;
	            hasHitRightState = BossBehaviour.Instance.animator.GetBehaviour<RunFast>().hasHitRightWall;
	        }
	        if (hasBeenTouched)
	        {
                if (hasHitRightState)
                {
                    if (MoveHorizontal(flySpeed * directionX * Time.deltaTime) ||
                    MoveVertical(flySpeed * directionY * Time.deltaTime))
                    {
                        Destroy(gameObject);
                    }
                    if (coll.IsTouching(Player.Instance.GetComponent<Collider2D>()))
                    {
                        Player.Instance.Damaged(1);
                    }
                }
                else
                {
                    if (MoveHorizontal(-flySpeed*directionX*Time.deltaTime) ||
                        MoveVertical(flySpeed*directionY*Time.deltaTime))
                    {
                        Destroy(gameObject);
                    }
                    if (coll.IsTouching(Player.Instance.GetComponent<Collider2D>()))
                    {
                        Player.Instance.Damaged(1);
                    }
                    
                }
                
	        }
	    }
    }
}
