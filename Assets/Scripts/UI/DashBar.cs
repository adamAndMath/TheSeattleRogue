using UnityEngine;

public class DashBar : MonoBehaviour
{
    private RectTransform BackDrop;
    public RectTransform BarOfDashing;

	void Start ()
	{
	    BackDrop = GetComponent<RectTransform>();

        BackDrop.sizeDelta = new Vector2(Player.Instance.maxDash*2, BackDrop.sizeDelta.y);
    }
	
	void Update ()
    {
	    BarOfDashing.sizeDelta = new Vector2(Player.Instance.dashPower*2,BarOfDashing.sizeDelta.y);
	}
}
