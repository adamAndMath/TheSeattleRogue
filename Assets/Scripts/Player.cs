using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Collider2D collider2D;
    private readonly RaycastHit2D[] rayHits = new RaycastHit2D[16];

    void Start()
    {
        animator = GetComponent<Animator>();
        collider2D = animator.GetComponent<Collider2D>();
    }

    void Update()
    {
        animator.SetBool("Moving", !Mathf.Approximately(0, Input.GetAxisRaw("Horizontal")));
        animator.SetBool("Jump", Input.GetButton("Jump"));
        animator.SetBool("Grounded", IsGrounded());
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
}
