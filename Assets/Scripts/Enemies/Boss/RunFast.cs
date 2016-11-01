using UnityEngine;
using System.Collections;

public class RunFast : StateMachineBehaviour
{
    public bool hasHitRightWall;
    public float bossSpeed;
    public float bossTimer;
    public float stateTime;
    public float shakeTime;

    private float realBossTimer;
    private float remaningShakeTime;
    private bool shakeIsReady;
    private float stateTimeRemaining;

    private bool readyToRun = true;

    public BossBehaviour boss;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	    boss = animator.GetComponent<BossBehaviour>();
	    realBossTimer = bossTimer;
	    stateTimeRemaining = stateTime;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	    if (readyToRun)
	    {
	        if (hasHitRightWall)
	        {
	            if (boss.MoveHorizontal(-bossSpeed*Time.deltaTime))
	            {
	                hasHitRightWall = false;
	                realBossTimer = bossTimer;
	                readyToRun = false;
	                BossBehaviour.Instance.GetComponent<SpriteRenderer>().flipX = false;
	            }

	        }
	        else
	        {
	            if (boss.MoveHorizontal(bossSpeed*Time.deltaTime))
	            {
                    hasHitRightWall = true;
                    realBossTimer = bossTimer;
                    readyToRun = false;
                    BossBehaviour.Instance.GetComponent<SpriteRenderer>().flipX = true;
	            }
	        }
	    }

	    if (readyToRun == false)
	    {
            if (boss.CameraShake(shakeTime))
	        {
	            readyToRun = true;
	        }
	    }
	    stateTimeRemaining -= Time.deltaTime;


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
