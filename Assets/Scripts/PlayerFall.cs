using UnityEngine;

public class PlayerFall : StateMachineBehaviour
{
    public float gravity;
    public string idleState;
    private float speed;
    private Collider2D collider2D;
    private readonly RaycastHit2D[] rayHits = new RaycastHit2D[16];

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        collider2D = animator.GetComponent<Collider2D>();
        speed = 0;
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float move = (speed + gravity * Time.deltaTime / 2) * Time.deltaTime;
        speed += gravity * Time.deltaTime;

        int size = collider2D.Cast(Vector2.down, rayHits, move);

        if (size > 0)
        {
            for (int i = 0; i < size; i++)
            {
                RaycastHit2D rayHit = rayHits[i];
                if (move > rayHit.distance && Mathf.Abs(rayHit.normal.y) / rayHit.normal.magnitude > 0.99F && (rayHit.point.y - animator.transform.position.y) < 0)
                {
                    move = rayHit.distance;
                    animator.Play(idleState);
                }
            }
        }

        animator.transform.Translate(move * Vector2.down);
    }
}
