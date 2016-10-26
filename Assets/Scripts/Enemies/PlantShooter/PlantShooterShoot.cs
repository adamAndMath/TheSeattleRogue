﻿using UnityEngine;
using System.Collections;

public class PlantShooterShoot : StateMachineBehaviour
{
    public GameObject projectilesRight;
    public GameObject projectilesLeft;
    public GameObject fastProjectileLeft;
    public GameObject fastProjectileRight;

    public Enemy enemy;

    public Vector2 angleVector;
    public float timer;

    private Quaternion startingRotation = Quaternion.Euler(0, 45, 0);
    private Quaternion negativeStartingRotation = Quaternion.Euler(0, -45, 0);
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	    enemy = animator.GetComponent<Enemy>();


        Instantiate(fastProjectileRight, enemy.transform.Find("PointOfShooting").transform.position, Quaternion.identity);
        Instantiate(fastProjectileLeft, enemy.transform.Find("PointOfShooting").transform.position, Quaternion.identity);

        Instantiate(projectilesRight, enemy.transform.Find("PointOfShooting").transform.position, Quaternion.identity);
        Instantiate(projectilesLeft, enemy.transform.Find("PointOfShooting").transform.position, Quaternion.identity);

        animator.SetBool("isShooting", false);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	/*{
	    timer -= timer - Time.deltaTime;
	    if (timer <= 0)
	    {
	        animator.SetBool("isShooting",false);
	    }
	}*/

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
   //{
        
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