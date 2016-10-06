using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public class TurretBehaviour : Enemy
{
    private Animator turretAnimator;
    public float turretRange;
    public float cooldown;
    public float ReloadTime;
    public bool cooldownOn = false;
    private bool IsAiming;

	// Use this for initialization
	void Start ()
	{
	    turretAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (ReloadTime > 0)
	    {
	        ReloadTime = ReloadTime - Time.deltaTime;
	        cooldownOn = true;
	    }
	    else
	    {
	        cooldownOn = false;
	    }

	    turretAnimator.SetBool("isAiming",turretRange >= (Player.Instance.transform.position - transform.position).magnitude);
	
    }

    public override void Damaged(int damageAmount)
    {
        base.Damaged(damageAmount);
        turretAnimator.SetBool("isTakingDamage", true);
    }

}
