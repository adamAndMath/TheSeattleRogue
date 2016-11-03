using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BeastModeIntro : MonoBehaviour
{
    public float timeTillFade = 0.5f;
    private Image image;
    public float fadeDuration = 2;

	void Start ()
	{
	    image = GetComponent<Image>();
	}
	
	void Update ()
	{
	    timeTillFade -= Time.deltaTime;
	    if (timeTillFade <= 0)
	    {
	        image.color = new Color(1,1,1,Mathf.Lerp(image.color.a, 0, Time.deltaTime*fadeDuration));
	    }

	    if (image.color.a <= 0.01f)
	    {
	        Destroy(gameObject);
	    }
	}
}
