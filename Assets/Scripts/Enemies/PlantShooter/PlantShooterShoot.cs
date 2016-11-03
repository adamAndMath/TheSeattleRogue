using UnityEngine;

public class PlantShooterShoot : StateMachineBehaviour
{
    public ProjectileBehaviour projectile;

    public Vector2[] projectiles;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	    foreach (var velocity in projectiles)
	    {
	        ProjectileBehaviour clone = Instantiate(projectile);
            clone.transform.SetParent(animator.transform.parent);
	        clone.transform.position = animator.transform.Find("PointOfShooting").transform.position;
            clone.horizontalSpeed = velocity.x;
            clone.startingSpeed = velocity.y;
	    }

        //Instantiate(fastProjectileRight, enemy.transform.Find("PointOfShooting").transform.position, Quaternion.identity);
        //Instantiate(fastProjectileLeft, enemy.transform.Find("PointOfShooting").transform.position, Quaternion.identity);

        //Instantiate(projectilesRight, enemy.transform.Find("PointOfShooting").transform.position, Quaternion.identity);
        //Instantiate(projectilesLeft, enemy.transform.Find("PointOfShooting").transform.position, Quaternion.identity);

        animator.SetBool("isShooting", false);
	}
}
