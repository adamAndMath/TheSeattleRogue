using UnityEngine;
using UnityEngine.UI;

public class MoneyGUI : MonoBehaviour
{

    private Text text;
	void Start ()
	{
	    text = GetComponent<Text>();
	}
	
	void Update ()
	{
	    text.text = "" + Player.money;
	}
}
