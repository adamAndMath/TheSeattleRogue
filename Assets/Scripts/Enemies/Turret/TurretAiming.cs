using UnityEngine;
using System.Collections;

public class TurretAiming : StateMachineBehaviour {

    public float roationSpeed;
    private float playerAngle;
    Quaternion lookrotation;
    Vector3 relativePos;
    Enemy enemy;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        enemy = animator.GetComponent<Enemy>();
    }

	//OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        relativePos = Player.Instance.transform.position - enemy.transform.position;

        lookrotation = Quaternion.LookRotation(Vector3.forward, relativePos);
        enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, lookrotation, roationSpeed*Time.deltaTime);

        if (Quaternion.Angle(enemy.transform.rotation, lookrotation) == 0 && enemy.GetComponent<TurretBehaviour>().cooldownOn == false)
        {
            animator.SetBool("readyToShoot", true);
            enemy.GetComponent<TurretBehaviour>().ReloadTime = 0;
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
