using UnityEngine;
using System.Collections;

public class LandSound : StateMachineBehaviour
{
    public AudioClip LandSoundClip;
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
	    animator.GetComponent<PlaySound>().Play(LandSoundClip);
	}

}
