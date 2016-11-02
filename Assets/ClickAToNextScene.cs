using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ClickAToNextScene : MonoBehaviour
{
    public int sceneToLoad = 1;

	void Update ()
    {
	    if (Input.GetButtonDown("Jump"))
	    {
	        SceneManager.LoadScene(sceneToLoad);
	    }
	}
}
