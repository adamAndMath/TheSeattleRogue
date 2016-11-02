using UnityEngine;
using UnityEngine.SceneManagement;

public class HubToWorld : MonoBehaviour
{
    public int scene;
    private Collider2D coll;

    void Start()
    {
        coll = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (coll.IsTouching(Player.Instance.GetComponent<Collider2D>()))
            SceneManager.LoadScene(scene);
    }
}
