using UnityEngine;
using System.Collections;

public class ShopKeeper : MonoBehaviour
{
    public GameObject ShopGameObject;
    public GameObject SpeechBubble;
    public float ShopRange = 4F;
    private bool ShopActive;
    private bool inRange;
	
	void Update ()
    {

	    if (Vector2.Distance(Player.Instance.transform.position, transform.position) <= ShopRange)
	    {
	        SpeechBubble.SetActive(true);
	        inRange = true;
	    }
	    else
	    {
	        SpeechBubble.SetActive(false);
	        inRange = false;
	    }

	    if (Input.GetButtonDown("Cancel") && inRange)
	    {
	        ShopActive =! ShopActive;
        }

        if (ShopActive && inRange)
	    {
	        ShopGameObject.SetActive(true);
	        Time.timeScale = 0;
	    }

	    if(Input.GetButtonDown("Cancel") && ShopActive)
	    {
	        ShopActive = false;
            ShopGameObject.SetActive(false);
            Time.timeScale = 1;
        }
	    //if(!inRange)
	    //{
     //       ShopGameObject.SetActive(false);
     //       Time.timeScale = 1;
     //   }

	}
}
