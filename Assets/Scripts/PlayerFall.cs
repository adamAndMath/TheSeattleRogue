using UnityEngine;

public class PlayerFall : StateMachineBehaviour
{
    public float gravity;
    private float speed;
    private Player player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.GetComponent<Player>();
        speed = 0;
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float move = (speed + gravity * Time.deltaTime / 2) * Time.deltaTime;
        speed += gravity * Time.deltaTime;

        player.MoveVertical(-move);
    }
}
