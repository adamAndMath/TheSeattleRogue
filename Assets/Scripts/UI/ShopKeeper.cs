using UnityEngine;
using System.Collections;

public class ShopKeeper : MonoBehaviour
{
    public GameObject ShopGameObject;
    public float ShopRange = 4F;
    private bool ShopActive;
	
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.W) && Vector2.Distance(Player.Instance.transform.position,transform.position) <=  ShopRange)
	    {
	        ShopActive =! ShopActive;
        }

	    if (ShopActive)
	    {
	        ShopGameObject.SetActive(true);
	        Time.timeScale = 0;
	    }
	    else if(Input.GetKeyDown(KeyCode.W) && ShopActive)
	    {            
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
