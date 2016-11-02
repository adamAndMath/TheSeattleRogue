using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int finalScore;
	void Start ()
	{
	    GetComponent<Text>().text = "Score: " + finalScore;
	}

}
