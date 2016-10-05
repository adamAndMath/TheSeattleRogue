using UnityEngine;
using System.Collections;

public class StandardTurretBehavior : Enemy
{
    public Animator TurretAnimator;

	// Use this for initialization
	void Start ()
	{
	    TurretAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
