using UnityEngine;
using System.Collections;

public class ChestBehaviour : Enemy
{
    public GameObject openChest;

    public override void Killed()
    {
        base.Killed();
        GameObject open = Instantiate(openChest);
        open.transform.SetParent(transform.parent);
        open.transform.localPosition = transform.localPosition;
    }
}
