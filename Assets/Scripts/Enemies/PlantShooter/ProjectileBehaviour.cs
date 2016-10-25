using UnityEngine;
using System.Collections;

public class ProjectileBehaviour : PhysicsObject
{
    public float horizontalSpeed;
    public float verticalSpeed;
    public float acceleration;
    private float startingSpeed;

    private GameObject smokeCloud;

	void Update ()
	{

	    MoveVertical(ConstantAcceleration(-acceleration, ref startingSpeed));
        
        MoveHorizontal(-horizontalSpeed*Time.deltaTime);
	    MoveVertical(verticalSpeed*Time.deltaTime);

	    if (IsGrounded())
	    {
	        Instantiate(smokeCloud, transform.position, Quaternion.identity);
            Destroy(gameObject);
	    }
	}
}
