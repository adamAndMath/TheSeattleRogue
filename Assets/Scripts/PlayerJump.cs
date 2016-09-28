using UnityEngine;

public class PlayerJump : StateMachineBehaviour
{
    public float gravity;
    public float startSpeed;
    public string fallState;
    private float speed;
    private Player player;
    private readonly RaycastHit2D[] rayHits = new RaycastHit2D[16];

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.GetComponent<Player>();
        speed = startSpeed;
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float move = (speed - gravity * Time.deltaTime / 2) * Time.deltaTime;
        speed -= gravity * Time.deltaTime;

        if (player.MoveVertical(move))
            speed = 0;

        if (speed <= 0)
            animator.Play(fallState);
    }
}
