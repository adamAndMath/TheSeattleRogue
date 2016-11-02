using UnityEngine;
using System.Collections;

public class GreatKick : StateMachineBehaviour
{
    private float boulderDistance;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        animator.SetInteger("StateSet", 0);
	    foreach (GameObject boulder in BossBehaviour.Instance.bouldersInScene)
	    {
	        float min = 0;
	        GameObject closetsBoulder;
	        if (Mathf.Abs(BossBehaviour.Instance.transform.position.x - boulder.transform.position.x) < min || min == 0)
	        {
	            min = boulder.transform.position.x;
	            closetsBoulder = boulder;
	        }
	        boulderDistance = min;
	    }
        if (Mathf.Abs(BossBehaviour.Instance.transform.position.x - boulderDistance) < 0.2)
	    if (BossBehaviour.Instance.transform.position.x > boulderDistance)
	    {
	        BossBehaviour.Instance.MoveVertical(3);
	    }
	    else
	    {
            BossBehaviour.Instance.MoveVertical(-3);
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
