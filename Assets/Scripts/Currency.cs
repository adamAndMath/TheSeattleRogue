using UnityEngine;

public class Currency : PhysicsObject
{
    public int val;
    public float gravity;
    public Vector2 speedMin;
    public Vector2 speedMax;
    private Vector2 speed;
    public AudioSource audioObject;
    public AudioClip coinSoundAudioClip;

    protected override void Start()
    {
        base.Start();
        speed = new Vector2(Random.Range(speedMin.x,speedMax.x), Random.Range(speedMin.y, speedMax.y));
    }

    void Update()
    {
        if (MoveHorizontal(speed.x*Time.deltaTime))
            speed.x = 0;

        if (MoveVertical((speed.y - gravity*Time.deltaTime/2) * Time.deltaTime))
            speed = Vector2.zero;
        else
            speed.y -= gravity*Time.deltaTime;
        

        int size = collider2D.Cast(Vector2.down, rayHits, 0);

        for (int i = 0; i < size; i++)
        {
            Player player = rayHits[i].collider.GetComponent<Player>();
            if (player)
            {
                player.money += val;
                Destroy(gameObject);

                AudioSource clone = Instantiate(audioObject);
                clone.clip = coinSoundAudioClip;
                clone.Play();
                clone.gameObject.hideFlags = HideFlags.HideInHierarchy;
                Destroy(clone.gameObject,3);
            }
        }
    }
}
