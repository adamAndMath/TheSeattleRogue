using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuGameObject;
    public bool paused;
    public int SceneToLoad;
	
	void Update ()
	{
	    if (Input.GetKeyDown(KeyCode.Escape))
	    {
	        paused =! paused;
	    }

	    if (paused)
	    {
	        Time.timeScale = 0;
            PauseMenuGameObject.SetActive(true);

	    }
	    else
	    {
	        Time.timeScale = 1;
            PauseMenuGameObject.SetActive(false);
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
