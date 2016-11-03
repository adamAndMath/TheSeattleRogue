using UnityEngine;

public class PlayerAttack : StateMachineBehaviour
{
    private Player player;

    private readonly RaycastHit2D[] rayHits = new RaycastHit2D[16];

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.GetComponent<Player>();

        int size = player.weaponCollider2D.Cast(Vector2.up, rayHits, 0);

        if (size > 0)
        {
            for (int i = 0; i < size; i++)
            {
                RaycastHit2D rayHit = rayHits[i];
                Enemy enemy = rayHit.collider.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.Damaged(Player.Data.item.damage, player.transform.right);
                }

                BossBehaviour boss = rayHit.collider.GetComponent<BossBehaviour>();

                if (boss != null)
                {
                    boss.Damaged(Player.Data.item.damage);
                }
            }
        }
        
    }
}
