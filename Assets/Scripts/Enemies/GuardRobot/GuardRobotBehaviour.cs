using UnityEngine;

public class GuardRobotBehaviour : Enemy
{
    public float threatRangeRight;
    public float threatRangeLeft;
    public float relocationSpeed;
    public float threatHeight;
    public float speed;
    private bool gizmosHasBeenDrawn;
    
    public float pointOfOrigin;
    public float snapThreshold;
    public float yPoint;

    private Animator animator;

    public Vector3 rightRange;
    public Vector3 leftRange;

    private Vector2 gizmosrightRange;
    private Vector2 gizmosleftRange;

    private Collider2D guardRobotCol;

	// Use this for initialization
	protected override void Start ()
	{
        base.Start();
        pointOfOrigin = transform.position.x;

        rightRange = new Vector3(transform.position.x + threatRangeRight, transform.position.y,0);
        leftRange = new Vector3(transform.position.x - threatRangeLeft, transform.position.y, 0);

	    animator = GetComponent<Animator>();

        guardRobotCol = this.GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (IsGrounded() == false)
        {
            float move = (gravitySpeed + gravity * Time.deltaTime / 2) * Time.deltaTime;
            gravitySpeed += gravity * Time.deltaTime;

            MoveVertical(-move);
        }
        else
        {
            gravitySpeed = 0;
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

        if (guardRobotCol.IsTouching(Player.Instance.GetComponent<Collider2D>()))
        {
            Player.Instance.Damaged(1);
        }
	}
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, gizmosleftRange);
        Gizmos.DrawLine(transform.position, gizmosrightRange);
        Gizmos.DrawWireSphere(new Vector2(pointOfOrigin, yPoint), 0.1f);
    }
    public override void Damaged(int damageAmount, Vector3 direction)
    {
        base.Damaged(damageAmount, direction);
        animator.SetBool("isTakingDamage", true);
    }
}

