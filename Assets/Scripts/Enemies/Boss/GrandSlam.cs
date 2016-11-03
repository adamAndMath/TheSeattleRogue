using UnityEngine;
using System.Collections;

public class GrandSlam : StateMachineBehaviour
{
    public float relocationSpeed;
    public float jumpSpeed;
    public float jumpDeaccelerationSpeed;
    public float snapThreshold;

    private BossBehaviour boss;
    private float speed;
    private float remainingShakeTime;
    private float move;
    private SlamDown slam;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	    boss = animator.GetComponent<BossBehaviour>();
	    slam = animator.GetBehaviour<SlamDown>();
	    speed = 0;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
	    if (!(Mathf.Abs(boss.transform.position.x - boss.originPos.x) < snapThreshold))
	    {
	        if (boss.transform.position.x > boss.originPos.x)
	        {
	            boss.MoveHorizontal(-relocationSpeed*Time.deltaTime);
	            BossBehaviour.Instance.GetComponent<SpriteRenderer>().flipX = true;
	        }
	        else
	        {
	            boss.MoveHorizontal(relocationSpeed*Time.deltaTime);
	            BossBehaviour.Instance.GetComponent<SpriteRenderer>().flipX = false;
	        }
	    }
	    else
	    {
            Debug.Log(jumpSpeed - move);
            //speed += Time.deltaTime * jumpDeaccelerationSpeed;
            //move = (speed + Time.deltaTime * jumpDeaccelerationSpeed / 2) * Time.deltaTime;

            move = jumpDeaccelerationSpeed * Mathf.Pow(Time.deltaTime, 2) * 0.5f + speed * Time.deltaTime;
            speed += jumpDeaccelerationSpeed * Time.deltaTime;

            boss.MoveVertical((jumpSpeed - move)*Time.deltaTime);

            if (jumpSpeed - move < 0)
            {
                animator.SetInteger("StateSet", 0);
                slam.slamDeaccelerationSpeed = jumpDeaccelerationSpeed;
                slam.slamSpeed = speed;
                slam.jumpSpeedReference = jumpSpeed;
                animator.SetBool("SlamDown", true);
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
