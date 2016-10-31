using UnityEngine;
using System.Collections;

public class ProjectileBehaviour : PhysicsObject
{
    public float horizontalSpeed;
    public float acceleration;
    public float startingSpeed;
    private float offSet = 0.9f;

    public GameObject smokeCloud;

	void Update ()
	{
        if (MoveHorizontal(-horizontalSpeed * Time.deltaTime) || MoveVertical(ConstantAcceleration(-acceleration, ref startingSpeed)))
	    {
            Instantiate(smokeCloud, new Vector3(transform.position.x, transform.position.y + offSet, 0), Quaternion.identity);
            Destroy(gameObject);
	    }
	}

    protected override bool CanCollide(RaycastHit2D rayHit, Vector2 dir)
    {
        return rayHit.collider.GetComponent<Player>() || base.CanCollide(rayHit, dir);
    }
}
