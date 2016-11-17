using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class BossBehaviour : PhysicsObject
{
    public static BossBehaviour Instance { get; private set; }

    public bool shakeIsReady;
    public float shakeAmount;
    public float timeBetweenBoulders;
    public float boulderSpawnRange;
    public int health;
    public float hitInvulTime;

    [HideInInspector]
    public Vector2 originPos;
    public bool isGrounded;
    public GameObject boulderGameObject;
    public bool stateHasBeenSet;

    private bool hasSet;
    private float remainingShakeTime;
    private float shakeX;
    private float shakeY;
    private float startingShakeAmount;
    [NonSerialized] public Vector3 startingPos;
    private float timeLeftBetweenBoulders;
    private float fallSpeed = -10;
    private float speed;
    private float invulTimeLeft;
    private bool isHit;

    public int Health;

    public Camera cam;
    public List<GameObject> boulders;
    public List<GameObject> bouldersInScene;
    public Animator animator;
    private Collider2D coll;

    private Color startColor;

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
	    startColor = GetComponent<SpriteRenderer>().color;
	    invulTimeLeft = hitInvulTime;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (coll.IsTouching(Player.Instance.GetComponent<Collider2D>()))
        {
            Player.Instance.Damaged(1);
        }

        animator.SetBool("IsGrounded", IsGrounded());

	    if (!stateHasBeenSet)
	    {
	        animator.SetInteger("StateSet", Random.Range(1, 3));
	        stateHasBeenSet = true;
	    }

        animator.SetBool("RunFastMode", animator.GetInteger("StateSet") == 1);
        animator.SetBool("GrandSlamMode", animator.GetInteger("StateSet") == 2);
        animator.SetBool("GreatKickMode", animator.GetInteger("StateSet") == 3);

	    if (invulTimeLeft <= 0)
	    {
	        isHit = false;
	    }

	    if (isHit)
	    {
	        GetComponent<SpriteRenderer>().color = Color.red;
	        invulTimeLeft -= Time.deltaTime;
	    }
	    else
	    {
            Debug.Log("Returned to my good state");
	        invulTimeLeft = hitInvulTime;
	        GetComponent<SpriteRenderer>().color = startColor;
	        isHit = false;
	    }

	    if (health <= 0)
	    {
	        SceneManager.LoadScene("Winner");
	        Player.data = null;
        }
    }
    public bool CameraShake(float shakeTime)
    {
        bool result;
        if (!hasSet)
        {
            remainingShakeTime = shakeTime;
            hasSet = true;
        }
        if (remainingShakeTime >= 0 && hasSet)
        {
            fallingBoulders();
            cam.transform.position = new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), cam.transform.position.z);
            shakeAmount -= startingShakeAmount*Time.deltaTime/shakeTime;
            result = false;
        }
        else
        {
            hasSet = false;
            cam.transform.position = startingPos;
            shakeAmount = startingShakeAmount;
            result = true;
        }
        remainingShakeTime -= Time.deltaTime;
        return result;
    }

    public void fallingBoulders()
    {
        GameObject boulderGameObject;
        if (timeLeftBetweenBoulders <= 0)
        {
            boulderGameObject = (GameObject)Instantiate(boulders[Random.Range(0, boulders.Count)], new Vector2(Random.Range(-boulderSpawnRange, boulderSpawnRange), 10), Quaternion.identity);
            bouldersInScene.Add(boulderGameObject);
            timeLeftBetweenBoulders = timeBetweenBoulders;
        }
        else
        {
            timeLeftBetweenBoulders -= Time.deltaTime;
        }
    }

    public void Damaged(int damageAmount)
    {
        Debug.Log("HitConnected");
        if (!isHit)
        {
            health -= damageAmount;
            isHit = true;
        }
    }

}
