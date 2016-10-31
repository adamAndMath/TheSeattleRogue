using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class BossBehaviour : PhysicsObject
{
    float shake = 0;
    float shakeAmount = 0.7f;
    float decreaseFactor = 1.0f;

    public int Health;

    public Camera cam;

    public Animator animator;

	// Use this for initialization
	protected override void Start ()
	{
        base.Start();
	    animator = GetComponent<Animator>();
	    cam = FindObjectOfType<Camera>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        CameraShake();
    }

    public void CameraShake()
    {
        {
            if (shake >= 0)
            {
                cam.transform.localPosition = Random.insideUnitSphere * shakeAmount;
                shake -= Time.deltaTime * decreaseFactor;

            }
            else
            {
                shake = 0.0f;
            }
        }
    }
}
