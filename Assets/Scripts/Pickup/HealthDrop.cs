using UnityEngine;

public class HealthDrop : Pickup
{
    public int health = 1;

    protected override void PickedUp(Player player)
    {
        Player.Data.hp = Mathf.Min(Player.Data.hp + health, player.maxHP);
    }
}
