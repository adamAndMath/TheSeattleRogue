using UnityEngine;
using System.Collections;

public class GuardRobotBehaviour : Enemy {
    public float threatRangeRight;
    public float threatRangeLeft;
    private Vector3 rightRange;
    private Vector3 leftRange;
    public float relocationSpeed;
    public float threatHeight;
    
    public float pointOfOrigin;
    
    public float snapThreshold;
    public float yPoint;

    private Rigidbody2D EnemyRigidbody;
    private Animator animator;

    public float speed;

	// Use this for initialization
	protected override void Start ()
	{
        base.Start();
        pointOfOrigin = transform.position.x;
	    yPoint = transform.position.y;

        rightRange = new Vector3(threatRangeRight, transform.position.y);
        leftRange = new Vector3(threatRangeLeft, transform.position.y);

	    EnemyRigidbody = GetComponent<Rigidbody2D>();
	    animator = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () 
    {
        

        animator.SetBool("isAttacking", (Player.Instance.transform.position.x - transform.position.x < threatRangeRight && Player.Instance.transform.position.x - transform.position.x > 0 || Player.Instance.transform.position.x - transform.position.x > -threatRangeLeft)
        && Mathf.Abs(Player.Instance.transform.position.y - transform.position.y) <= threatHeight);

	}
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position-leftRange);
        Gizmos.DrawLine(transform.position, transform.position+rightRange);
        Gizmos.DrawWireSphere(new Vector2(pointOfOrigin, yPoint), 0.1f);
    }
}

