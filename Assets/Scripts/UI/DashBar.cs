using UnityEngine;

public class DashBar : MonoBehaviour
{
    private RectTransform BackDrop;
    public RectTransform BarOfDashing;

	void Start ()
	{
	    BackDrop = GetComponent<RectTransform>();

        BackDrop.sizeDelta = new Vector2(Player.Instance.maxDash, BackDrop.sizeDelta.y);
    }
	
	void Update ()
    {
	    BarOfDashing.sizeDelta = new Vector2(Player.Instance.dashPower,BarOfDashing.sizeDelta.y);
	}
}
