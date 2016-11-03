using UnityEngine;
using System.Collections;

public class EnemyTakingDamage : StateMachineBehaviour
{
    private SpriteRenderer renderer;
    private Color color = Color.white;
    public GuardRobotBehaviour guardOnly;
    public Enemy enemy;

    private float moveY;
    private float moveX;

    public float initialSpeedX;
    public float initalSpeedY;

    public float acceleration;

    private Vector2 dir;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        renderer = animator.GetComponent<SpriteRenderer>();
        renderer.color = Color.red;
        guardOnly = animator.GetComponent<GuardRobotBehaviour>();
        enemy = animator.GetComponent<Enemy>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemy.MoveHorizontal(PhysicsObject.ConstantAcceleration(-acceleration*enemy.damageDirection.x, ref initialSpeedX)))
        {
            initialSpeedX = 0;
        }
        if (enemy.MoveVertical(PhysicsObject.ConstantAcceleration(-acceleration * enemy.damageDirection.y, ref initalSpeedY)))
        {
            initalSpeedY = 0;
        }

        if (Mathf.Abs(initalSpeedY) < 0.2 || Mathf.Abs(initialSpeedX) < 0.2)
        {
            animator.SetBool("IsTakingDamage", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        renderer.color = color;;
        if (guardOnly != null)
        {
            guardOnly.speed = 0;
        }
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
