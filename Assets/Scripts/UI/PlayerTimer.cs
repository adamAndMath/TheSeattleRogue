using UnityEngine;
using UnityEngine.UI;

public class PlayerTimer : MonoBehaviour
{
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        if (Player.data != null)
        {
            Player.data.time += Time.deltaTime;
            text.text = Mathf.FloorToInt(Player.data.time).ToString("###00");
        }
    }
}
