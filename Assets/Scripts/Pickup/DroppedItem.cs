using UnityEngine;

public class DroppedItem : Pickup
{
    Item item;

    protected override void PickedUp(Player player)
    {
        player.SetItem(item);
    }
}
