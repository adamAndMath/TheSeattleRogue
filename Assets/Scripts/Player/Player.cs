using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : PhysicsObject
{
    public static Player Instance { get; private set; }

    public Item item;
    public BoxCollider2D weaponCollider2D;
    public int maxHP = 3;
    [HideInInspector]
    public int hp;
    public static int money;
    private Animator animator;
    public int score;

    public float dashRegenerationRate = 1;
    public int maxDash = 100;
    public int dashCost = 50;
    public float dashPower;

    public Animator weapon;

    public int deathScene = 0;
    public GameObject deathTransitionObject;

    public List<Sprite> enemyDeathSprites;
    public bool Direction { get { return 0 < transform.localScale.x; } set { transform.localScale = new Vector3(value ? -1 : 1, 1, 1); } }

    public void SetItem(Item item)
    {
        this.item = item;
        weapon.runtimeAnimatorController = item.animator;
        weaponCollider2D.offset = item.collisionOffset;
        weaponCollider2D.size = item.collisionSize;
    }

    void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        dashPower = maxDash;
        hp = maxHP;
        animator = GetComponent<Animator>();
        SetItem(item);
    }

    void Update()
    {
        if (hp <= 0)
        {
            deathTransitionObject.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        if (dashPower < maxDash)
        {
            dashPower += Time.deltaTime*dashRegenerationRate;
        }

        if (Time.timeScale != 0)
        {
            animator.SetBool("Moving", !Mathf.Approximately(0, Input.GetAxisRaw("Horizontal")));
            animator.SetBool("Looking", !Mathf.Approximately(0, Input.GetAxisRaw("Horizontal")) || !Mathf.Approximately(0, Input.GetAxisRaw("Vertical")));
            animator.SetBool("Jump", Input.GetButton("Jump"));
            animator.SetBool("Grounded", IsGrounded());
            animator.SetBool("Attacking", Input.GetButtonDown("Attack"));
            weapon.SetBool("Attacking", Input.GetButtonDown("Attack"));
            animator.SetBool("Charge", Input.GetAxis("Charge") > 0.9F && dashPower >= dashCost);
        }

    }

    public void Damaged(int damage)
    {
        if (hp > 0 && !animator.GetBool("Charge"))
        {
            if (animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex("Invincibility")).IsName("Invincibility"))
                return;

            hp -= damage;
            animator.SetTrigger("Hit");
        }
    }

    protected override bool CanCollide(RaycastHit2D rayHit, Vector2 dir)
    {
        return rayHit.collider.gameObject.layer != Platform || (Input.GetAxisRaw("Vertical") > -0.5F && base.CanCollide(rayHit, dir));
    }
}
