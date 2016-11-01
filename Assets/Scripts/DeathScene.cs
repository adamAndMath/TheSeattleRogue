using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DeathScene : MonoBehaviour
{
    public static List<Sprite> enemies;
    public Image imagePrefab;
    public float time;
    private float timer;
    private int id;

	void Update ()
    {
        if (id < enemies.Count) return;

	    timer += Time.deltaTime;

        if (timer >= time) return;

	    timer -= time;
        Image image = Instantiate(imagePrefab);
        image.transform.SetParent(transform);
        image.sprite = enemies[id++];
	}
}
