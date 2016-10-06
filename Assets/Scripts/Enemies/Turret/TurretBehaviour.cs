using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public class TurretBehaviour : Enemy
{
    private Animator turretAnimator;
    public float turretRange;
    public float cooldown;
    private float ReloadTime;
    public bool cooldownOn = false;

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

    public void RayCast()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, turretRange);
        Player player = hit.collider.GetComponent<Player>();

        if (player != null && cooldownOn == false)
        {
            turretAnimator.SetBool("readyToShoot", true);
            cooldownOn = true;
            ReloadTime = cooldown;
        }    
        
    }
}
