using UnityEngine;

public class StubleAround : StateMachineBehaviour
{
    public float relocationSpeed;
    public int direction;
    public float timeToShoot;

    private Enemy enemy;
    private float time;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	    direction = Random.Range(-1, 2);
	    enemy = animator.GetComponent<Enemy>();
	    time = timeToShoot;
	}

	public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	    time -= Time.deltaTime;
	    if (enemy.MoveHorizontal(relocationSpeed*direction*Time.deltaTime))
	    {
	        direction = -direction;
	    }

	    if (time < 0)
	    {
	        animator.SetBool("isShooting", true);
	    }
	}
}
