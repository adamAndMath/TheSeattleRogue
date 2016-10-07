using UnityEngine;

public class Player : PhysicsObject
{
    public static Player Instance { get; private set; }

    public Collider2D weaponCollider2D;
    public int maxHP = 3;
    [HideInInspector]
    public int hp;
    public int money;
    private Animator animator;

    public bool Direction { get { return 0 < transform.localScale.x; } set { transform.localScale = new Vector3(value ? -1 : 1, 1, 1); } }

    void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        hp = maxHP;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("Moving", !Mathf.Approximately(0, Input.GetAxisRaw("Horizontal")));
        animator.SetBool("Looking", !Mathf.Approximately(0, Input.GetAxisRaw("Horizontal")) || !Mathf.Approximately(0, Input.GetAxisRaw("Vertical")));
        animator.SetBool("Jump", Input.GetButton("Jump"));
        animator.SetBool("Grounded", IsGrounded());
        animator.SetBool("Charge", Input.GetAxis("Charge") > 0.9F);
        animator.SetBool("Attacking", Input.GetButton("Attack"));
    }

    public void Damaged(int damage)
    {
        if (animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex("Invincibility")).IsName("Invincibility"))
            return;

        hp -= damage;
        animator.SetTrigger("Hit");
    }
}
