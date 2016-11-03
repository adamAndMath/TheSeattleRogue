using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : PhysicsObject
{
    public static Player Instance { get; private set; }
    public static PlayerData Data { get; private set; }

    public Item item;
    public BoxCollider2D weaponCollider2D;
    public int maxHP = 3;
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
    
    public bool Direction { get { return 0 < transform.localScale.x; } set { transform.localScale = new Vector3(value ? -1 : 1, 1, 1); } }

    public class PlayerData
    {
        public int hp;
        public Item item;
        public List<Sprite> enemyDeathSprites = new List<Sprite>();
    }

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
        animator = GetComponent<Animator>();

        if (Data == null)
        {
            Data = new PlayerData();
            Data.hp = maxHP;
            SetItem(item);
            Data.enemyDeathSprites = new List<Sprite>();
        }
    }

    void Update()
    {
        if (Data.hp <= 0)
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
        if (Time.timeScale != 0 && Data.hp > 0 && !animator.GetCurrentAnimatorStateInfo(0).IsName("Fly"))
        {
            if (animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex("Invincibility")).IsName("Invincibility"))
                return;

            Data.hp -= damage;
            animator.SetTrigger("Hit");
        }
    }

    protected override bool CanCollide(RaycastHit2D rayHit, Vector2 dir)
    {
        return rayHit.collider.gameObject.layer != Platform || (Input.GetAxisRaw("Vertical") > -0.5F && base.CanCollide(rayHit, dir));
    }
}
