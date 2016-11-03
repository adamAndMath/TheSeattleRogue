using UnityEngine;
using System.Collections;

public class PlantShooterBehaviour : Enemy
{
    private bool hasPassedRight;
    private bool hasPassedLeft;
    private bool hasBeenGrounded;
    private bool hasPassedMiddle;

    private Animator animator;
    private bool hasHitWall;
    private float stoppingPointRight;
    private float stoppingPointLeft;
    private float pointOfOrigin;
    
    public float relocationSpeed;
    public float timeOfWaitingSet;
    public float snappingThreshold;
    public float leftPoint;
    public float rightPoint;

    private float timeOfWaiting = 4;

    private float gizmosYPoint;

    private Collider2D plantCollider2D;

	// Use this for initialization
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        pointOfOrigin = transform.position.x;

        stoppingPointLeft = transform.position.x - leftPoint;
        stoppingPointRight = transform.position.x + rightPoint;

        plantCollider2D = GetComponent<Collider2D>();
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
        timeOfWaiting -= Time.deltaTime;
	    if (timeOfWaiting < 0)
	    {
	        
            if (hasPassedLeft && hasPassedRight && Mathf.Abs(transform.position.x - pointOfOrigin) > snappingThreshold)
	        {
                if (MoveHorizontalSloped(-relocationSpeed * Time.deltaTime))
                {
                    hasHitWall = true;
                }
	        }
            else if ((Mathf.Abs(transform.position.x - pointOfOrigin) < snappingThreshold || hasHitWall) && hasPassedLeft && hasPassedRight)
            {
	            hasPassedLeft = false;
	            hasPassedRight = false;
                animator.SetBool("isShooting",true);
	            timeOfWaiting = timeOfWaitingSet;
                hasHitWall = false;
            }
            if (!hasBeenGrounded && IsGrounded())
	        {
	            hasBeenGrounded = true;
	            animator.SetBool("isShooting", true);
	            timeOfWaiting = timeOfWaitingSet;
	            gizmosYPoint = transform.position.y;
	        }



	        if ((transform.position.x > stoppingPointLeft && !hasHitWall && !hasPassedLeft && hasBeenGrounded))
	        {
	            if (MoveHorizontalSloped(-relocationSpeed*Time.deltaTime))
	            {
	                hasHitWall = true;
	            }
	        }
	        else if (!hasPassedLeft && hasBeenGrounded)
	        {
	            hasPassedLeft = true;
	            animator.SetBool("isShooting", true);
	            timeOfWaiting = timeOfWaitingSet;
	            hasHitWall = false;
	        }


	        if ((transform.position.x < stoppingPointRight || !hasHitWall) && hasPassedLeft && hasBeenGrounded && !hasPassedRight)
	        {
	            if (MoveHorizontalSloped(relocationSpeed*Time.deltaTime))
	            {
	                hasHitWall = true;
	            }
	        }
            else if (transform.position.x > stoppingPointRight && !hasPassedRight)
            {
                hasPassedRight = true;
                animator.SetBool("isShooting", true);
                timeOfWaiting = timeOfWaitingSet;
                hasHitWall = false;
            }

	    }
	    
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector3(stoppingPointLeft, gizmosYPoint,0), 0.1f);
        Gizmos.DrawWireSphere(new Vector3(stoppingPointRight, gizmosYPoint, 0), 0.1f);
        Gizmos.DrawWireSphere(new Vector3(pointOfOrigin, gizmosYPoint, 0), 0.1f);
    }
    public override void Damaged(int damageAmount, Vector2 direction)
    {
        base.Damaged(damageAmount, direction);
        animator.SetBool("isTakingDamage", true);
    }
}
