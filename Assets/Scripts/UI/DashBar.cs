using UnityEngine;
using System.Collections;

public class DashBar : MonoBehaviour
{
    private RectTransform BackDrop;
    public RectTransform BarOfDashing;

	void Start ()
	{
	    BackDrop = GetComponent<RectTransform>();

        BackDrop.sizeDelta = new Vector2(Player.Instance.MaxDash, BackDrop.sizeDelta.y);
    }
	
	void Update ()
    {
	    BarOfDashing.sizeDelta = new Vector2(Player.Instance.DashPower,BarOfDashing.sizeDelta.y);
	}
}
