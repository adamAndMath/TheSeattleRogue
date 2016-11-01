using UnityEngine;

public class Currency : Pickup
{
    public int val;
    public GameObject audioObject;

    protected override void PickedUp(Player player)
    {
        player.money += val;

        GameObject clone = Instantiate(audioObject);
        clone.hideFlags = HideFlags.HideInHierarchy;
        Destroy(clone, 3);
    }
}
