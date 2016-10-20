using UnityEngine;
using System.Collections;

public class GuardRobotAttacking : StateMachineBehaviour
{
    private Enemy enemy;
    public float attackingSpeed;

    //Movement
    public Vector2 speed;
    public float acceleration;
    private float initialSpeed;
    private float position;
	 
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	    enemy = animator.GetComponent<Enemy>();
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        //Debug.Log(Mathf.Pow(Mathf.Sqrt(Player.Instance.transform.position.y - enemy.transform.position.y), 2));
	    if (Player.Instance.transform.position.x > enemy.transform.position.x)
	    {
	        //enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(1*attackingSpeed, 0), ForceMode2D.Force);
           
	        position = enemy.transform.position.x + acceleration*Mathf.Pow(Time.deltaTime,2)*0.5f + initialSpeed*Time.deltaTime;
	        enemy.transform.position = new Vector2(position,enemy.transform.position.y);
            Debug.Log(position);

            //enemy.transform.Translate(enemy.transform.position.x + acceleration * Mathf.Pow(Time.deltaTime, 2) * 0.5f + initialSpeed * Time.deltaTime,0,0);
	    }

        if (Player.Instance.transform.position.x < enemy.transform.position.x)
        {
            //enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1 * attackingSpeed, 0), ForceMode2D.Force);
        }
	}

    public void CastRay()
    {
        
    }
	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
