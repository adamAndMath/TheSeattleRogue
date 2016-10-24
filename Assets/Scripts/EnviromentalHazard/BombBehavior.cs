using UnityEngine;
using System.Collections;

public class BombBehavior : Enemy
{
    public float bombRange;
    public float chaseSpeed;
    private float moveX;
    private float moveY;
    public float explosionTime;

    private bool activated;

	// Use this for initialization
	
	// Update is called once per frame
	protected void Update () 
    {

	    if (bombRange >= (Player.Instance.transform.position - transform.position).magnitude && Player.Instance != null)
	    {
	        activated = true;
	    }

	    if (activated)
	    {
            //Tikkende lyd skal indsættes
	        if (Mathf.Abs(Player.Instance.transform.position.x - transform.position.x) > 0.1f)
	        {
	            if ((Player.Instance.transform.position.x - transform.position.x) < 0)
	            {
	                moveX = -1*chaseSpeed*Time.deltaTime;
	            }
	            else
	            {
	                moveX = chaseSpeed*Time.deltaTime;
	            }
	            MoveHorizontal(moveX);
	        }

	        if (Mathf.Abs(Player.Instance.transform.position.y - transform.position.y) > 0.1f)
	        {
	            if ((Player.Instance.transform.position.y - transform.position.y) > 0.1f)
	            {
	                moveY = -1*chaseSpeed*Time.deltaTime;
	            }
	            else
	            {
	                moveY = chaseSpeed*Time.deltaTime;
	            }
	            MoveVertical(moveY);
	        }
	        explosionTime -= Time.deltaTime;
	    }

	    if (explosionTime <= 0)
	    {
	        //Afspil animaation og afspil explosionslyd
	    }
	}
}
