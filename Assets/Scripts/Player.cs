using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public int maxHP = 3;
    [HideInInspector]
    public int hp;
    private Animator animator;
    private Collider2D collider2D;
    private readonly RaycastHit2D[] rayHits = new RaycastHit2D[16];

    public bool Direction { get { return 0 < transform.localScale.x; } set { transform.localScale = new Vector3(value ? -1 : 1, 1, 1); } }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        hp = maxHP;
        animator = GetComponent<Animator>();
        collider2D = animator.GetComponent<Collider2D>();
    }

    void Update()
    {
        animator.SetBool("Moving", !Mathf.Approximately(0, Input.GetAxisRaw("Horizontal")));
        animator.SetBool("Looking", !Mathf.Approximately(0, Input.GetAxisRaw("Horizontal")) || !Mathf.Approximately(0, Input.GetAxisRaw("Vertical")));
        animator.SetBool("Jump", Input.GetButton("Jump"));
        animator.SetBool("Grounded", IsGrounded());
        animator.SetBool("Charge", Input.GetButton("Charge"));
    }

    private bool IsGrounded()
    {
        int size = collider2D.Cast(Vector2.down, rayHits, 0.01F);

        if (size > 0)
        {
            for (int i = 0; i < size; i++)
            {
                RaycastHit2D rayHit = rayHits[i];
                if (rayHit.point.y - animator.transform.position.y < 0 && Mathf.Abs(rayHit.normal.y) / rayHit.normal.magnitude > 0.99F)
                    return true;
            }
        }

        return false;
    }

    public bool MoveHorizontal(float move)
    {
        Vector2 dir = Vector2.right * Mathf.Sign(move);
        move = Mathf.Abs(move);
        bool re = false;

        int size = collider2D.Cast(dir, rayHits, move);

        if (size > 0)
        {
            for (int i = 0; i < size; i++)
            {
                RaycastHit2D rayHit = rayHits[i];
                if (move > rayHit.distance && Mathf.Abs(Vector2.Dot(rayHit.normal.normalized, dir)) > 0.99F && Vector2.Dot(rayHit.point - (Vector2)transform.position, dir) > 0)
                {
                    move = rayHit.distance;
                    re = true;
                }
            }
        }

        animator.transform.Translate(move * dir);
        return re;
    }

    public bool MoveVertical(float move)
    {
        Vector2 dir = Vector2.up * Mathf.Sign(move);
        move = Mathf.Abs(move);
        bool re = false;

        int size = collider2D.Cast(dir, rayHits, move);

        if (size > 0)
        {
            for (int i = 0; i < size; i++)
            {
                RaycastHit2D rayHit = rayHits[i];
                if (move > rayHit.distance && Mathf.Abs(rayHit.normal.y) / rayHit.normal.magnitude > 0.99F && Vector2.Dot(rayHit.point - (Vector2)transform.position, dir) > 0)
                {
                    move = rayHit.distance;
                    re = true;
                }
            }
        }

        animator.transform.Translate(move * dir);
        return re;
    }
}
