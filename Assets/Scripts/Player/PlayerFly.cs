using UnityEngine;
using XInputDotNetPure;

public class PlayerFly : StateMachineBehaviour
{
    public float speed;
    private Vector2 dir;
    private Player player;

    private bool playerIndexSet;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.GetComponent<Player>();
        dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected ans use it
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

            GamePad.SetVibration(playerIndex, Mathf.Clamp01(1.1F - dir.x), Mathf.Clamp01(1.1F + dir.x));

    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.MoveHorizontal(dir.x*speed*Time.deltaTime);
        player.MoveVertical(dir.y*speed*Time.deltaTime);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GamePad.SetVibration(playerIndex, 0, 0);
    }
}
