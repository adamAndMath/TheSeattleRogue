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

    public float speedX;
    public float speedY;


    private float initialSpeedX;
    private float initalSpeedY;
    private TurretBehaviour turret;

    public float acceleration;

    private Vector2 dir;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        renderer = animator.GetComponent<SpriteRenderer>();
        renderer.color = Color.red;
        guardOnly = animator.GetComponent<GuardRobotBehaviour>();
        enemy = animator.GetComponent<Enemy>();
        initalSpeedY = speedY;
        initialSpeedX = speedX;
        turret = animator.GetComponent<TurretBehaviour>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (turret == null)
        {
            Debug.Log(Mathf.Abs(enemy.damageDirection.x));
            if (Mathf.Abs(enemy.damageDirection.x) > 0.01)
            {
                Debug.Log("IsThisHappening");
                if (
                    enemy.MoveHorizontal(PhysicsObject.ConstantAcceleration(acceleration, ref initialSpeedX)*enemy.damageDirection.x))
                {
                    initialSpeedX = 0;
                }
            }
            else
            {
                initialSpeedX = 0;
            }

            if (Mathf.Abs(enemy.damageDirection.y) > 0.01)
            {
                if (
                    enemy.MoveVertical(PhysicsObject.ConstantAcceleration(-acceleration, ref initalSpeedY)*enemy.damageDirection.y))
                {
                    initalSpeedY = 0;
                }
            }
            else
            {
                initalSpeedY = 0;
            }

            if (Mathf.Abs(initalSpeedY) < 0.2 || Mathf.Abs(initialSpeedX) < 0.2)
            {
                animator.SetBool("isTakingDamage", false);
            }
        }
        else
        {
            animator.SetBool("isTakingDamage", false);
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