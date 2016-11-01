using UnityEngine;
using System.Collections;

public class GrandSlam : StateMachineBehaviour
{
    public float relocationSpeed;
    public float jumpSpeed;
    public float jumpDeaccelerationSpeed;
    public float snapThreshold;
    public float shakeTime;

    private BossBehaviour boss;
    private float speed;
    private float remainingShakeTime;
    private float move;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	    boss = animator.GetComponent<BossBehaviour>();
	    remainingShakeTime = shakeTime;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
	    if (!(boss.transform.position.x - boss.originPos.x < snapThreshold))
	    {
	        if (boss.transform.position.x > boss.originPos.x)
	        {
	            boss.MoveHorizontal(-relocationSpeed);
	        }
	        else
	        {
	            boss.MoveHorizontal(relocationSpeed*Time.deltaTime);
	        }
	    }
	    else
	    {
            speed += Time.deltaTime * jumpDeaccelerationSpeed;
            move = (speed + Time.deltaTime * jumpDeaccelerationSpeed / 2) * Time.deltaTime;

            boss.MoveVertical((jumpSpeed-move) * Time.deltaTime);

            if (jumpSpeed - move < 0)
            {
                jumpDeaccelerationSpeed = jumpDeaccelerationSpeed*2;
            }


	        if (boss.isGrounded && remainingShakeTime > 0)
	        {
	            boss.CameraShake(shakeTime);
                remainingShakeTime -= Time.deltaTime;
	        }
	        else if (boss.isGrounded)
	        {
	            Debug.Log("What?");
	            animator.SetInteger("StateSet", 0);
	            speed = 0;
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
