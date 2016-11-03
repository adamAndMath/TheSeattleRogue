using UnityEngine;
using System.Collections;

public class SlamDown : StateMachineBehaviour {
    public float shakeTime;
    public float slamDeaccelerationSpeed;
    public float slamSpeed;
    public float jumpSpeedReference;

    private bool ground;
    private float move; 
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
	        move = slamDeaccelerationSpeed*Mathf.Pow(Time.deltaTime, 2) * 0.5f + slamSpeed*Time.deltaTime;
	        slamSpeed += slamDeaccelerationSpeed*Time.deltaTime;

	        BossBehaviour.Instance.MoveVertical(-move);

	    if (animator.GetBool("IsGrounded"))
	    {
            animator.SetBool("ShakeTime", true);
	        animator.SetBool("SlamDown", false);
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
