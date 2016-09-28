using UnityEngine;

public class PlayerMove : StateMachineBehaviour
{
    public float speed;
    private SpriteRenderer spriteRenderer;
    private Player player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        player = animator.GetComponent<Player>();
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        spriteRenderer.flipX = !Mathf.Approximately(0, Input.GetAxisRaw("Horizontal"));
        float move = Input.GetAxis("Horizontal")*speed*Time.deltaTime;
        player.MoveHorizontal(move);
    }
}
