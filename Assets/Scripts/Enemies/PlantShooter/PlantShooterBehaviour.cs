using UnityEngine;
using System.Collections;

public class PlantShooterBehaviour : Enemy
{
    private bool hasPassedRight;
    private bool hasPassedLeft;
    private bool hasBeenGrounded;
    private bool hasPassedMiddle;

    private Animator animator;

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
        if (plantCollider2D.IsTouching(Player.Instance.GetComponent<Collider2D>()))
        {
            Player.Instance.Damaged(1);
        }
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
                MoveHorizontalSloped(-relocationSpeed * Time.deltaTime);
	        }
            else if (Mathf.Abs(transform.position.x - pointOfOrigin) < snappingThreshold && hasPassedLeft && hasPassedRight)
            {
	            hasPassedLeft = false;
	            hasPassedRight = false;
                animator.SetBool("isShooting",true);
	            timeOfWaiting = timeOfWaitingSet;
	        }
            if (!hasBeenGrounded && IsGrounded())
	        {
	            hasBeenGrounded = true;
	            animator.SetBool("isShooting", true);
	            timeOfWaiting = timeOfWaitingSet;
	            gizmosYPoint = transform.position.y;
	        }



	        if (transform.position.x > stoppingPointLeft && !hasPassedLeft && hasBeenGrounded)
	        {
	            MoveHorizontalSloped(-relocationSpeed*Time.deltaTime);
	        }
	        else if (!hasPassedLeft && hasBeenGrounded)
	        {
	            hasPassedLeft = true;
	            animator.SetBool("isShooting", true);
	            timeOfWaiting = timeOfWaitingSet;
	        }


	        if (transform.position.x < stoppingPointRight && hasPassedLeft && hasBeenGrounded && !hasPassedRight)
	        {
	            MoveHorizontalSloped(relocationSpeed*Time.deltaTime);
	        }
            else if (transform.position.x > stoppingPointRight && !hasPassedRight)
            {
                hasPassedRight = true;
                animator.SetBool("isShooting", true);
                timeOfWaiting = timeOfWaitingSet;
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
    public override void Damaged(int damageAmount)
    {
        base.Damaged(damageAmount);
        animator.SetBool("isTakingDamage", true);
    }
}
