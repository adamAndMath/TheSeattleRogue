using UnityEngine;
using System.Collections;

public class GuardRobotPatrol : StateMachineBehaviour
{
    private GuardRobotBehaviour enemy;

    private bool hasPassedRightPoint;
    private bool hasPassedLeftPoint;
    private bool isFinishedPatroling;

    public float idleTimer;
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        enemy = animator.GetComponent<GuardRobotBehaviour>();
	    idleTimer = 5;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        idleTimer = idleTimer - Time.deltaTime;
	    if (idleTimer <= 0)
	    {
	        if (enemy.transform.position.x >= enemy.rightRange.x)
	        {
	            hasPassedRightPoint = true;
	            isFinishedPatroling = false;
	        }
	        if (hasPassedRightPoint != true)
	        {
	            enemy.MoveHorizontalSloped(0.01F*enemy.relocationSpeed);
	        }

	        if (hasPassedRightPoint)
	        {
	            enemy.MoveHorizontalSloped(-0.01F*enemy.relocationSpeed);
	            if (enemy.transform.position.x <= enemy.leftRange.x)
	            {
	                hasPassedLeftPoint = true;
	            }
	        }

	        if (hasPassedLeftPoint && hasPassedRightPoint)
	        {
                Debug.Log("This is the destination i currently am in "+enemy.transform.position.x);
	            Debug.Log("This is the place i want to be "+enemy.pointOfOrigin);
                Debug.Log("Can't get home Left");

	            enemy.MoveHorizontalSloped(0.02f * enemy.relocationSpeed);

	            if ((enemy.transform.position.x - enemy.pointOfOrigin < enemy.snapThreshold &&
	                enemy.transform.position.x - enemy.pointOfOrigin > 0 ||
	                enemy.transform.position.x - enemy.pointOfOrigin > -enemy.snapThreshold &&
	                enemy.transform.position.x - enemy.pointOfOrigin < 0) && isFinishedPatroling == false)
	            {
                    enemy.transform.position = new Vector3(enemy.pointOfOrigin, enemy.transform.position.y, enemy.transform.position.z);
	                idleTimer = 5;
	                Debug.Log(idleTimer);
	                isFinishedPatroling = true;
	            }
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
