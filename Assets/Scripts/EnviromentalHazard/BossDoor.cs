using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDoor : MonoBehaviour
{
    public int scene;
    Collider2D coll;

	void Start ()
    {
        coll = GetComponent<Collider2D>();
	}
	
	void Update ()
    {
        if (coll.IsTouching(Player.Instance.GetComponent<Collider2D>()))
            SceneManager.LoadScene(scene);
	}
}
