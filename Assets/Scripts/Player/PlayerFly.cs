using UnityEngine;

public class PlayerFly : StateMachineBehaviour
{
    public float speed;
    private Vector2 dir;
    private Player player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.GetComponent<Player>();
        dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        player.DashPower -= player.DashCost;
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.MoveHorizontalSloped(dir.x*speed*Time.deltaTime);
        player.MoveVertical(dir.y*speed*Time.deltaTime);
    }
}
