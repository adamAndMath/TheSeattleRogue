using UnityEngine;

public class HealthDrop : Pickup
{
    public int health = 1;

    protected override void PickedUp(Player player)
    {
        player.hp = Mathf.Min(player.hp + health, player.maxHP);
    }
}
