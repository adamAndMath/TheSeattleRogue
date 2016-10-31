using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class BossBehaviour : PhysicsObject
{
    public int Health;

    public Animator animator;

	// Use this for initialization
	protected override void Start ()
	{
        base.Start();
	    animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    public void CameraShake()
    {
        
    }
}
