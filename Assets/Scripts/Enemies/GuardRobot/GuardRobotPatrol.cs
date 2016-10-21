using UnityEngine;
using System.Collections;

public class GuardRobotPatrol : StateMachineBehaviour
{
    private GuardRobotBehaviour enemy;

    private bool hasPassedRightPoint;
    private bool hasPassedLeftPoint;
    

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        enemy = animator.GetComponent<GuardRobotBehaviour>();
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
	    if (hasPassedLeftPoint && hasPassedRightPoint)
	    {
            if (enemy.transform.position.x > enemy.pointOfOrigin)
            {
                enemy.MoveHorizontalSloped(-0.01F * enemy.relocationSpeed);
            }
            else
            {
                enemy.MoveHorizontalSloped(0.01F * enemy.relocationSpeed);
            }

            if (enemy.transform.position.x - enemy.pointOfOrigin < enemy.snapThreshold && enemy.transform.position.x - enemy.pointOfOrigin > 0 || enemy.transform.position.x - enemy.pointOfOrigin > -enemy.snapThreshold && enemy.transform.position.x - enemy.pointOfOrigin < 0)
            {
            
            }
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
