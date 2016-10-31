using UnityEngine;
using System.Collections;

public class RunFast : StateMachineBehaviour
{
    public bool hasHitRightWall;
    public float bossSpeed;
    public float bossTimer;
    private float realBossTimer;

    private bool readyToRun;

    public BossBehaviour boss;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	    boss = animator.GetComponent<BossBehaviour>();
	    realBossTimer = bossTimer;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	    if (readyToRun)
	    {
	        //------------------------------------Not working--------------------------------------------------//
	        if (hasHitRightWall)
	        {
	            if (boss.MoveHorizontal(-bossSpeed*Time.deltaTime))
	            {
	                hasHitRightWall = false;
	                realBossTimer = bossTimer;
	                readyToRun = false;
	                Debug.Log("So The Boss Has Hit");
	            }
	        }
	        else
	        {
	            if (boss.MoveHorizontal(bossSpeed*Time.deltaTime))
	            {
                    hasHitRightWall = true;
                    realBossTimer = bossTimer;
                    readyToRun = false;
	            }
	        }
	    }
	    //---------------------------------------Works fine--------------------------------------------------//

	    if (realBossTimer <= 0)
	    {
	        readyToRun = true;
	    }
	    realBossTimer -= Time.deltaTime;
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
