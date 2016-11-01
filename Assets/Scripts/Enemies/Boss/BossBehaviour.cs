using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Random = UnityEngine.Random;

public class BossBehaviour : PhysicsObject
{
    public static BossBehaviour Instance { get; private set; }

    public bool shakeIsReady;
    public float shakeAmount;
    public float timeBetweenBoulders;
    public float boulderSpawnRange;
    
    [NonSerialized] public Vector2 originPos;
    [NonSerialized] public bool isGrounded;

    private float remainingShakeTime;
    private float shakeX;
    private float shakeY;
    private float startingShakeAmount;
    private Vector3 startingPos;
    private float timeLeftBetweenBoulders;
    private float fallSpeed = -10;
    private float speed;

    public int Health;

    public Camera cam;
    public List<GameObject> boulders;
    public List<GameObject> bouldersInScene;
    public Animator animator;
    private Collider2D coll;

    void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
	protected override void Start ()
	{
        base.Start();
	    animator = GetComponent<Animator>();
	    cam = FindObjectOfType<Camera>();
	    startingShakeAmount = shakeAmount;
	    startingPos = cam.transform.position;
	    timeLeftBetweenBoulders = timeBetweenBoulders;
	    coll = GetComponent<Collider2D>();
	    originPos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (coll.IsTouching(Player.Instance.GetComponent<Collider2D>()))
        {
            Player.Instance.Damaged(1);
        }

        if (!IsGrounded())
        {
            //MoveVertical(fallSpeed*Time.deltaTime);
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }
	    if (animator.GetBehaviour<StateHandler>())
	    {
	        animator.SetInteger("StateSet", Random.Range(1, 1));

	        animator.SetBool("RunFastMode", animator.GetInteger("StateSet") == 1);
	        animator.SetBool("GreatKickMode", animator.GetInteger("StateSet") == 2);
	        animator.SetBool("GrandSlamMode", animator.GetInteger("StateSet") == 3);
	    }
    }
    public bool CameraShake(float shakeTime)
    {
        bool result = false;
        bool hasSet = false;
        if (!hasSet)
        {
            remainingShakeTime = shakeTime*0.95f;
            hasSet = true;
        }
        remainingShakeTime -= Time.deltaTime;
        if (remainingShakeTime >= 0 && hasSet)
        {
            shakeX = Random.Range(-shakeAmount, shakeAmount);
            shakeY = Random.Range(-shakeAmount, shakeAmount);
            cam.transform.position = new Vector3(shakeX, shakeY, cam.transform.position.z);
            shakeAmount -= (startingShakeAmount/shakeTime)*Time.deltaTime;
            fallingBoulders();
        }
        else
        {
            result = true;
            cam.transform.position = startingPos;
            shakeAmount = startingShakeAmount;
        }
        return result;
    }

    public void fallingBoulders()
    {
        Debug.Log("Boulders Being Run");
        if (timeLeftBetweenBoulders <= 0)
        {
            Instantiate(boulders[Random.Range(0, boulders.Count)], new Vector2(Random.Range(-boulderSpawnRange, boulderSpawnRange), 10), Quaternion.identity);
            timeLeftBetweenBoulders = timeBetweenBoulders;
        }
        else
        {
            timeLeftBetweenBoulders -= Time.deltaTime;
        }
    }
}
