﻿using UnityEngine;
using System.Collections;

public class ShopKeeper : MonoBehaviour
{
    public GameObject ShopGameObject;
    public GameObject SpeechBubble;
    public float ShopRange = 4F;
    private bool ShopActive;
	
	void Update ()
    {
	    if (Vector2.Distance(Player.Instance.transform.position, transform.position) <= ShopRange)
	    {
	        SpeechBubble.SetActive(true);
	    }
	    else
	    {
	        SpeechBubble.SetActive(false);
	    }

	    if (Input.GetKeyDown(KeyCode.W) && Vector2.Distance(Player.Instance.transform.position,transform.position) <=  ShopRange)
	    {
	        ShopActive =! ShopActive;
        }

	    if (Input.GetKeyDown(KeyCode.Escape) && ShopActive)
	    {
	        ShopActive = false;
	    }

            if (ShopActive)
	    {
	        ShopGameObject.SetActive(true);
	        Time.timeScale = 0;
	    }
	    else if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Escape)) && ShopActive)
	    {
            ShopGameObject.SetActive(false);
            Time.timeScale = 1;
        }
	    else
	    {
            ShopGameObject.SetActive(false);
            Time.timeScale = 1;
        }

	    if (Vector2.Distance(Player.Instance.transform.position, transform.position) > ShopRange)
	    {
	        ShopActive = false;
	    }

	}
}
