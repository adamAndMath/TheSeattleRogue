﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SmokeCloudBehaviour : MonoBehaviour
{
    private Collider2D cloudCollider2D;

	// Use this for initialization
    void Start()
    {
        cloudCollider2D = GetComponent<Collider2D>();
    }

	// Update is called once per frame
	void Update () 
    {
        if (cloudCollider2D.IsTouching(Player.Instance.GetComponent<Collider2D>()))
        {
            Player.Instance.Damaged(1);
        }
	}
}
