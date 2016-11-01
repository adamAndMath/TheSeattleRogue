using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : PhysicsObject
{
    public static Player Instance { get; private set; }

    public Collider2D weaponCollider2D;
    public int maxHP = 3;
    [HideInInspector]
    public int hp;
    public int money;
    private Animator animator;

    public float dashRegenerationRate = 1;
    public int maxDash = 100;
    public int dashCost = 50;
    public float dashPower;

    public Animator weapon;

    public int deathScene = 0;
    public Animator deathTransitionAnimator;

    public List<Sprite> enemyDeathSprites;
    public bool Direction { get { return 0 < transform.localScale.x; } set { transform.localScale = new Vector3(value ? -1 : 1, 1, 1); } }

    void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        dashPower = maxDash;
        base.Start();
        hp = maxHP;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (dashPower < maxDash)
        {
            dashPower += Time.deltaTime*dashRegenerationRate;
        }
        animator.SetBool("Moving", !Mathf.Approximately(0, Input.GetAxisRaw("Horizontal")));
        animator.SetBool("Looking", !Mathf.Approximately(0, Input.GetAxisRaw("Horizontal")) || !Mathf.Approximately(0, Input.GetAxisRaw("Vertical")));
        animator.SetBool("Jump", Input.GetButton("Jump"));
        animator.SetBool("Grounded", IsGrounded());
        animator.SetBool("Attacking", Input.GetButtonDown("Attack"));
        weapon.SetBool("Attacking", Input.GetButtonDown("Attack"));
        animator.SetBool("Charge", Input.GetAxis("Charge") > 0.9F && dashPower >= dashCost);

        if (hp <= 0)
        {
            deathTransitionAnimator.SetBool("Transitioning", true);
            DeathScene.enemies = enemyDeathSprites;
            SceneManager.LoadScene(deathScene);
        }

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
