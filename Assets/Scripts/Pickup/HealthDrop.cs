using UnityEngine;

public class HealthDrop : Pickup
{
    public int health = 1;

    protected override void PickedUp(Player player)
    {
        Player.data.hp = Mathf.Min(Player.data.hp + health, player.maxHP);
    }
}
