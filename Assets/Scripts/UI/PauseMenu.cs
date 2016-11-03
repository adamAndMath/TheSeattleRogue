using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuGameObject;
    public bool paused;
    public bool canPause = true;
    public int SceneToLoad;
	
	void Update ()
	{
	    if (canPause)
	    {
	        if (Input.GetKeyDown(KeyCode.Escape) && paused == false)
	        {
	            Time.timeScale = 0;
                PauseMenuGameObject.SetActive(true);
	            paused = true;
	        }
	        else if (Input.GetKeyDown(KeyCode.Escape) && paused)
	        {
	            Time.timeScale = 1;
                PauseMenuGameObject.SetActive(false);
	            paused = false;
	        }
	    }
    }

    public void Resume()
    {
        paused = false;
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(SceneToLoad);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
