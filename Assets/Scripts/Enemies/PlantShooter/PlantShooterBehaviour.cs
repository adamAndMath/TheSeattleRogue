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

	// Use this for initialization
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        pointOfOrigin = transform.position.x;

        stoppingPointLeft = transform.position.x - leftPoint;
        stoppingPointRight = transform.position.x + rightPoint;
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
        timeOfWaiting -= Time.deltaTime;
	    if (timeOfWaiting < 0)
	    {
	        
            if (hasPassedLeft && hasPassedRight && Mathf.Abs(transform.position.x - pointOfOrigin) > snappingThreshold)
	        {
	            Debug.Log("Is Ready to go middle");
                MoveHorizontalSloped(-relocationSpeed * Time.deltaTime);
	        }
            else if (Mathf.Abs(transform.position.x - pointOfOrigin) < snappingThreshold && hasPassedLeft && hasPassedRight)
            {
                Debug.Log("Not this yet please");
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
                Debug.Log("It Has been grounded");
	            gizmosYPoint = transform.position.y;
	        }



	        if (transform.position.x > stoppingPointLeft && !hasPassedLeft && hasBeenGrounded)
	        {
	            MoveHorizontalSloped(-relocationSpeed*Time.deltaTime);
	            Debug.Log("What is going on?");
	        }
	        else if (!hasPassedLeft && hasBeenGrounded)
	        {
	            hasPassedLeft = true;
	            animator.SetBool("isShooting", true);
	            timeOfWaiting = timeOfWaitingSet;
                Debug.Log("It Has passed the left point");
	        }


	        if (transform.position.x < stoppingPointRight && hasPassedLeft && hasBeenGrounded && !hasPassedRight)
	        {
	            MoveHorizontalSloped(relocationSpeed*Time.deltaTime);
	            Debug.Log("This is really fucked up");
	        }
            else if (transform.position.x > stoppingPointRight && !hasPassedRight)
            {
                hasPassedRight = true;
                animator.SetBool("isShooting", true);
                timeOfWaiting = timeOfWaitingSet;
                Debug.Log("Has passed  right");
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
}
