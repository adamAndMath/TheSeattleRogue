using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class BossBehaviour : PhysicsObject
{
    public float shakeTime;
    public bool shakeIsReady;
    public float shakeAmount;

    private float remainingShakeTime;
    private float shakeX;
    private float shakeY;
    private float startingShakeAmount;
    private Vector3 startingPos;

    public int Health;

    public Camera cam;

    public Animator animator;

	// Use this for initialization
	protected override void Start ()
	{
        base.Start();
	    animator = GetComponent<Animator>();
	    cam = FindObjectOfType<Camera>();
	    remainingShakeTime = shakeTime;
	    startingShakeAmount = shakeAmount;
	    startingPos = cam.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (shakeTime <= 0)
        {
            shakeIsReady = true;
        }   
    }
    public bool CameraShake()
    {
        bool result = false;

        if (remainingShakeTime >= 0)
        {
            shakeX = Random.Range(-shakeAmount, shakeAmount);
            shakeY = Random.Range(-shakeAmount, shakeAmount);
            cam.transform.position = new Vector3(shakeX, shakeY, cam.transform.position.z);
            remainingShakeTime -= Time.deltaTime;
            shakeAmount -= (startingShakeAmount/shakeTime)*Time.deltaTime;
        }
        else
        {
            result = true;
            remainingShakeTime = shakeTime;
            cam.transform.position = startingPos;
            shakeAmount = startingShakeAmount;
        }

        return result;
    }
}
