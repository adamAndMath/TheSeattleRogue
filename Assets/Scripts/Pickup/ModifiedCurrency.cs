using UnityEngine;
using System.Collections;

public class ModifiedCurrency : Pickup
{
    public int val;
    public GameObject audioObject;

    private float horizontalSpeed;
    public float acceleration;
    public float startingSpeed;

    protected override void PickedUp(Player player)
    {
        if (IsGrounded())
        {
            player.money += val;

            GameObject clone = Instantiate(audioObject);
            clone.hideFlags = HideFlags.HideInHierarchy;
            Destroy(clone, 3);
        }
    }
	void Update ()
	{
	    if (!IsGrounded())
	    {
	        horizontalSpeed = Random.Range(-1.0f, 1.1f);
	        MoveHorizontal(-horizontalSpeed*Time.deltaTime);
	        MoveVertical(ConstantAcceleration(-acceleration, ref startingSpeed));
	    }
	}
}
