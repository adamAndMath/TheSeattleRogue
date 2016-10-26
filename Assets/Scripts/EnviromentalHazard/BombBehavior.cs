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

    private Collider2D bombCollider;

	// Use this for initialization
    protected override void Start()
    {
        base.Start();
        bombCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
	protected void Update () 
    {

	    if (bombRange >= (Player.Instance.transform.position - transform.position).magnitude && Player.Instance != null)
	    {
	        activated = true;
	    }

	    if (activated)
	    {
            Quaternion rotation = Quaternion.LookRotation(Player.Instance.transform.position - transform.position, transform.TransformDirection(Vector3.up));
            transform.rotation = new Quaternion(0,0,rotation.z,rotation.w);
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
	                moveY = chaseSpeed*Time.deltaTime;
	            }
	            else
	            {
	                moveY = -chaseSpeed*Time.deltaTime;
	            }
	            MoveVertical(moveY);
	        }
	        explosionTime -= Time.deltaTime;
	    }

	    if (explosionTime <= 0)
	    {
	        //Afspil animaation og afspil explosionslyd
            if (bombCollider.IsTouching(Player.Instance.GetComponent<Collider2D>()))
            {
                Player.Instance.Damaged(1);
            }
            Destroy(gameObject);
	    }
	}
}
