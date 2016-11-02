using System;
using System.Xml;
using UnityEngine;

public class TurretBehaviour : Enemy
{
    private Animator turretAnimator;
    public float turretRange;
    public float cooldown;
    /*[NonSerialized]*/public float ReloadTime;
    public float TESTPLSIGNORE;
    public bool cooldownOn;
    private bool IsAiming;

    private SpriteRenderer renderer;
    public Sprite[] ChargingSprites = new Sprite[0];

    public GameObject turretPlatform;
	// Use this for initialization
	protected override void Start ()
	{
	    renderer = GetComponent<SpriteRenderer>();
	    turretAnimator = GetComponent<Animator>();

	    GameObject platform = (GameObject)Instantiate(turretPlatform, transform.position, Quaternion.identity);
        platform.transform.SetParent(transform.parent);
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (ReloadTime <= cooldown)
	    {
	        ReloadTime = ReloadTime + Time.deltaTime;
	        cooldownOn = true;
	    }
	    else
	    {
	        cooldownOn = false;
	    }

	    turretAnimator.SetBool("isAiming",turretRange >= (Player.Instance.transform.position - transform.position).magnitude);

	    for(int i = 0; i < ChargingSprites.Length; i++)
	    {
	    TESTPLSIGNORE = cooldown/ChargingSprites.Length*i;
	        if (ReloadTime >= TESTPLSIGNORE)
	        {
	            renderer.sprite = ChargingSprites[i];
	        }
	    }

	}

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //UnityEditor.Handles.color = Color.green;
        //UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, turretRange);
    }
#endif

    public override void Damaged(int damageAmount)
    {
        base.Damaged(damageAmount);
        turretAnimator.SetBool("isTakingDamage", true);
    }

}
