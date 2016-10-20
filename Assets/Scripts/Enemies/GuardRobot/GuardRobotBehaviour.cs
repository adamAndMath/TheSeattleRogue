using UnityEngine;
using System.Collections;

public class GuardRobotBehaviour : Enemy {
    public float threatRangeRight;
    public float threatRangeLeft;
    private Vector3 rightRange;
    private Vector3 leftRange;
    private float pointOfOrigin;
    public float relocationSpeed;
    private float snapThreshold = 0.01F;
    public float threatHeight;

    private Rigidbody2D EnemyRigidbody;
    private Animator animator;


	// Use this for initialization
	void Start ()
	{
	    pointOfOrigin = transform.position.x;
        rightRange = new Vector3(threatRangeRight, 0);
        leftRange = new Vector3(threatRangeLeft, 0);

	    EnemyRigidbody = GetComponent<Rigidbody2D>();
	    animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (transform.position.x - pointOfOrigin < snapThreshold && transform.position.x - pointOfOrigin > 0 || transform.position.x - pointOfOrigin > -snapThreshold && transform.position.x - pointOfOrigin < 0)
        {
            
            transform.position = new Vector3(pointOfOrigin,transform.position.y,transform.position.z);
        }
	    
        if (transform.position.x > pointOfOrigin)
	    {
	        transform.Translate(-0.01F*relocationSpeed,0,0);
	    }
	    else
	    {
            transform.Translate(0.01F * relocationSpeed, 0, 0);
	    }

        animator.SetBool("isAttacking", (Player.Instance.transform.position.x - transform.position.x < threatRangeRight && Player.Instance.transform.position.x - transform.position.x > 0 || Player.Instance.transform.position.x - transform.position.x > -threatRangeLeft) 
        /*&& Mathf.Pow(Mathf.Sqrt(Player.Instance.transform.position.y - transform.position.y),2) < threatHeight*/);

	}
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position-leftRange);
        Gizmos.DrawLine(transform.position, transform.position+rightRange);
    }
}

