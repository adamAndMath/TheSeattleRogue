using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class GuardRobotBehaviour : Enemy {
    public float threatRangeRight;
    public float threatRangeLeft;
    public float relocationSpeed;
    public float threatHeight;
    public float speed;
    private bool gizmosHasBeenDrawn;
    
    public float pointOfOrigin;
    public float snapThreshold;
    public float yPoint;

    private Rigidbody2D EnemyRigidbody;
    private Animator animator;

    public Vector3 rightRange;
    public Vector3 leftRange;

    private Vector2 gizmosrightRange;
    private Vector2 gizmosleftRange;

	// Use this for initialization
	protected override void Start ()
	{
        base.Start();
        pointOfOrigin = transform.position.x;

        rightRange = new Vector3(transform.position.x + threatRangeRight, transform.position.y,0);
        leftRange = new Vector3(transform.position.x - threatRangeLeft, transform.position.y, 0);

	    EnemyRigidbody = GetComponent<Rigidbody2D>();
	    animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (IsGrounded() == false)
        {
            float move = (gravitySpeed + gravity * Time.deltaTime / 2) * Time.deltaTime;
            gravitySpeed += gravity * Time.deltaTime;

            MoveVertical(-move);
            Debug.Log("This is not on the ground");
        }
        else
        {
            gravitySpeed = 0;
            Debug.Log("This is on the ground");
        }
        if (IsGrounded() && !gizmosHasBeenDrawn)
        {
            gizmosrightRange = new Vector3(transform.position.x + threatRangeRight, transform.position.y);
            gizmosleftRange = new Vector3(transform.position.x - threatRangeLeft, transform.position.y);
            yPoint = transform.position.y;
            gizmosHasBeenDrawn = true;
        }

        animator.SetBool("isAttacking", (Player.Instance.transform.position.x - transform.position.x < threatRangeRight && Player.Instance.transform.position.x - transform.position.x > 0 || Player.Instance.transform.position.x - transform.position.x > -threatRangeLeft)
        && Mathf.Abs(Player.Instance.transform.position.y - transform.position.y) <= threatHeight);

	}
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, gizmosleftRange);
        Gizmos.DrawLine(transform.position, gizmosrightRange);
        Gizmos.DrawWireSphere(new Vector2(pointOfOrigin, yPoint), 0.1f);
    }
}

