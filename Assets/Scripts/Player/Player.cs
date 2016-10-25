using System;
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

    public float DashRegenerationRate = 1;
    public int MaxDash = 100;
    public int DashCost = 50;
    public float DashPower;

    public bool Direction { get { return 0 < transform.localScale.x; } set { transform.localScale = new Vector3(value ? -1 : 1, 1, 1); } }

    void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        DashPower = MaxDash;
        base.Start();
        hp = maxHP;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (DashPower < MaxDash)
        {
            DashPower += Time.deltaTime*DashRegenerationRate;
        }
        animator.SetBool("Moving", !Mathf.Approximately(0, Input.GetAxisRaw("Horizontal")));
        animator.SetBool("Looking", !Mathf.Approximately(0, Input.GetAxisRaw("Horizontal")) || !Mathf.Approximately(0, Input.GetAxisRaw("Vertical")));
        animator.SetBool("Jump", Input.GetButton("Jump"));
        animator.SetBool("Grounded", IsGrounded());
        animator.SetBool("Attacking", Input.GetButton("Attack"));
        animator.SetBool("Charge", Input.GetAxis("Charge") > 0.9F && DashPower >= DashCost);
    }

    public void Damaged(int damage)
    {
        if (animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex("Invincibility")).IsName("Invincibility"))
            return;

        hp -= damage;
        animator.SetTrigger("Hit");
    }

    protected override bool CanCollide(RaycastHit2D rayHit, Vector2 dir)
    {
        return rayHit.collider.gameObject.layer != Platform || (Input.GetAxisRaw("Vertical") >= 0 && base.CanCollide(rayHit, dir));
    }
}
