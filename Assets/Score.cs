using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int score;
	void Start ()
	{
	    GetComponent<Text>().text = "Score: " + score;
	}

}
