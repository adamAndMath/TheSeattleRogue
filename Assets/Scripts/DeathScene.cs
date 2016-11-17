using System;
using UnityEngine;
using UnityEngine.UI;

public class DeathScene : MonoBehaviour
{
    public static Player.PlayerData finalScore;
    public Image imagePrefab;
    public Text scoreText;
    public Text timeText;
    public float time;
    private float timer;
    private int id;

    void Start()
    {
        scoreText.text = "Score: " + finalScore.score;
        timeText.text = String.Format("Time: {0} sec", finalScore.time.ToString("####.##"));
    }

	void Update()
    {
        if (finalScore.kills == null || id >= finalScore.kills.Count) return;
        
	    timer += Time.deltaTime;

        if (timer < time) return;

	    timer -= time;
        Image image = Instantiate(imagePrefab);
        image.transform.SetParent(transform);
        image.sprite = finalScore.kills[id++];
    }
}
