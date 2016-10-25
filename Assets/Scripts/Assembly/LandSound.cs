using UnityEngine;
using System.Collections;

public class LandSound : StateMachineBehaviour
{
    public GameObject LandSoundObject;
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
	    animator.GetComponent<PlaySound>().Play(LandSoundObject);
	}

}
