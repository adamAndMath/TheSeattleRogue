using UnityEngine;

public class DroppedItem : Pickup
{
    public Item item;

    protected override void PickedUp(Player player)
    {
        player.SetItem(item);
    }
}
