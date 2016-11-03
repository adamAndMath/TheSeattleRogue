using UnityEngine;
using System.Collections;

public class GuardRobotAttacking : StateMachineBehaviour
{
    private GuardRobotBehaviour enemy;
    public float attackingSpeed;

    //Movement
    public float acceleration;
    public float position;
    public float maxSpeed;
	 
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	    enemy = animator.GetComponent<GuardRobotBehaviour>();
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {

	    if (Player.Instance.transform.position.x > enemy.transform.position.x)
	    {
	        position = PhysicsObject.ConstantAcceleration(acceleration, ref enemy.speed);
	    }
        else
	    {
            position = PhysicsObject.ConstantAcceleration(-acceleration, ref enemy.speed);
        }
        enemy.speed = Mathf.Clamp(enemy.speed, -maxSpeed, maxSpeed);
	    enemy.MoveHorizontalSloped(position);
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
