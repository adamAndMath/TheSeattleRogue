﻿using UnityEngine;
using System.Collections;

public class StandardTurretBehavior : Enemy
{
    public Animator turretAnimator;

	// Use this for initialization
	void Start ()
	{
	    turretAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
