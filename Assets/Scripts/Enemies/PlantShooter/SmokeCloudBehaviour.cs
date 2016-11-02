using UnityEngine;

public class SmokeCloudBehaviour : MonoBehaviour
{
    private Collider2D cloudCollider2D;
    public float disappearTimer;

    void OnDisable()
    {
        Destroy(gameObject);
    }

	// Use this for initialization
    void Start()
    {
        cloudCollider2D = GetComponent<Collider2D>();
    }

	// Update is called once per frame
	void Update ()
	{
	    disappearTimer -= Time.deltaTime;
	    if (disappearTimer <= 0)
	    {
	        Destroy(gameObject);
	    }
        if (cloudCollider2D.IsTouching(Player.Instance.GetComponent<Collider2D>()))
        {
            Player.Instance.Damaged(1);
        }
        
	}
}
