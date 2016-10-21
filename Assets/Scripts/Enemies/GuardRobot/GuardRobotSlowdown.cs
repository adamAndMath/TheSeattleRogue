using UnityEngine;
using System.Collections;

public class GuardRobotSlowdown : StateMachineBehaviour
{
    public GuardRobotBehaviour enemy;
    public float stoopingThreshold;
    public float accelerationSlow;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        enemy = animator.GetComponent<GuardRobotBehaviour>();
        animator.SetBool("isReturningHome", false);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        if (Mathf.Abs(enemy.speed) > stoopingThreshold)
        {
            float position;

            if (enemy.speed > 0)
	        {
                position = -accelerationSlow * Mathf.Pow(Time.deltaTime, 2) * 0.5f + enemy.speed * Time.deltaTime;
                enemy.speed -= accelerationSlow * Time.deltaTime;
	        }
            else
            {
                position = accelerationSlow * Mathf.Pow(Time.deltaTime, 2) * 0.5f + enemy.speed * Time.deltaTime;
                enemy.speed += accelerationSlow * Time.deltaTime;
            }

            enemy.MoveHorizontalSloped(position);
        }
        else
        {
            animator.SetBool("isReturningHome", true);
            Debug.Log("Why?!?");
        }
        
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
