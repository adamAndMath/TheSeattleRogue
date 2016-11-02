using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour
{
    public float flySpeed;
    private Collider2D collider2D;
    private readonly RaycastHit2D[] hits = new RaycastHit2D[16];
    public int damage = 2;

    void OnDisable()
    {
        Destroy(gameObject);
    }

	// Use this for initialization
	void Start ()
	{
	    collider2D = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    float dist = flySpeed*Time.deltaTime;

	    int size = collider2D.Cast(transform.up, hits, dist);

        if (size > 0)
        {
            Collider2D hit = null;

            for (int i = 0; i < size; i++)
            {
                RaycastHit2D rayHit = hits[i];

                if ((!rayHit.collider.isTrigger || rayHit.collider.GetComponent<Player>()) && dist > rayHit.distance)
                {
                    dist = rayHit.distance;
                    hit = rayHit.collider;
                }
            }

            if (hit != null)
            {
                Destroy(gameObject);

                if (hit.GetComponent<Player>() != null)
                    hit.GetComponent<Player>().Damaged(damage);
            }
        }

        transform.Translate(Vector3.up * dist, Space.Self);
	}
}
