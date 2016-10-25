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

    private float timeOfWaiting;


	// Use this for initialization
    protected void Start()
    {
        animator = GetComponent<Animator>();
        pointOfOrigin = transform.position.x;
    }
	
	// Update is called once per frame
	void Update () 
    {
	    if (timeOfWaiting < 0)
	    {
	        if ((hasPassedLeft && !hasPassedMiddle || hasPassedLeft && hasPassedRight) &&
	            Mathf.Abs(transform.position.x) > snappingThreshold)
	        {
	            
	        }
            else if ((!hasPassedLeft && !hasPassedMiddle || hasPassedLeft && hasPassedRight) && Mathf.Abs(transform.position.x) < snappingThreshold)
            {
	            if (hasPassedMiddle)
	            {
	                hasPassedMiddle = false;
	                hasPassedLeft = false;
	                hasPassedRight = false;
	            }
                animator.SetBool("isShooting",true);
	            timeOfWaiting = timeOfWaitingSet;
	        }

	        if (!IsGrounded() && !hasBeenGrounded)
	        {
	            MoveVertical(gravitySpeed);
	        }
	        else
	        {
	            hasBeenGrounded = true;
	            animator.SetBool("isShooting", true);
	            timeOfWaiting = timeOfWaitingSet;
	        }

	        if (transform.position.x > -stoppingPointLeft && !hasPassedLeft && hasBeenGrounded)
	        {
	            MoveHorizontalSloped(-relocationSpeed*Time.deltaTime);
	        }
	        else if (!hasPassedLeft)
	        {
	            hasPassedLeft = true;
	            animator.SetBool("isShooting", true);
	            timeOfWaiting = timeOfWaitingSet;
	        }
	        if (transform.position.x < stoppingPointRight && hasPassedLeft && hasBeenGrounded)
	        {
	            MoveHorizontalSloped(relocationSpeed*Time.deltaTime);
	        }
	        else if (!hasPassedRight)
	        {
	            hasPassedRight = true;
	        }
	    }
	    timeOfWaiting -= Time.deltaTime;
    }
}
