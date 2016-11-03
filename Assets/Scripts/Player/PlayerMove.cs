using UnityEngine;

public class PlayerMove : StateMachineBehaviour
{
    public float speed;
    private Player player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.GetComponent<Player>();
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1F && Time.timeScale != 0)
        {
            player.Direction = 0 > Input.GetAxisRaw("Horizontal");
        }

        float move = Input.GetAxis("Horizontal")*speed*Time.deltaTime;
        player.MoveHorizontalSloped(move);
    }
}
