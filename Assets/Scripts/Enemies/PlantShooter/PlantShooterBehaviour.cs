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

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector3(stoppingPointLeft, gizmosYPoint,0), 0.1f);
        Gizmos.DrawWireSphere(new Vector3(stoppingPointRight, gizmosYPoint, 0), 0.1f);
        Gizmos.DrawWireSphere(new Vector3(pointOfOrigin, gizmosYPoint, 0), 0.1f);
    }
    public override void Damaged(int damageAmount, Vector3 direction)
    {
        base.Damaged(damageAmount, direction);
        animator.SetBool("isTakingDamage", true);
    }
}
