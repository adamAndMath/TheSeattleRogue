using UnityEngine;

public class PlayerMove : StateMachineBehaviour
{
    public float speed;
    private SpriteRenderer spriteRenderer;
    private Collider2D collider2D;
    private readonly RaycastHit2D[] rayHits = new RaycastHit2D[16];

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        collider2D = animator.GetComponent<Collider2D>();
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        spriteRenderer.flipX = !Mathf.Approximately(0, Input.GetAxisRaw("Horizontal"));
        float move = Mathf.Abs(Input.GetAxis("Horizontal"))*speed*Time.deltaTime;
        Vector2 dir = Vector2.right*Input.GetAxisRaw("Horizontal");

        int size = collider2D.Cast(dir, rayHits, move);

        if (size > 0)
        {
            for (int i = 0; i < size; i++)
            {
                RaycastHit2D rayHit = rayHits[i];
                if (move > rayHit.distance && Vector2.Dot((rayHit.point - (Vector2)animator.transform.position), dir)/dir.magnitude > 0)
                    move = rayHit.distance;
            }
        }

        animator.transform.Translate(move * dir);
    }
}
