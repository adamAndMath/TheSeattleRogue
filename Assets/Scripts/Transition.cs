using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
	public Sprite[] TransitionFrames=new Sprite[0];
    public int TimeBetweenFrames = 5;
    private int count;
    private Image renderer;
    private int currentFrame = 0;

    void Start()
    {
        renderer = GetComponent<Image>();
    }
	void Update ()
    {
	    if (Player.Data.hp <= 0)
	    {
	        if (currentFrame == TransitionFrames.Length)
	        {
                DeathScene.enemies = Player.Data.enemyDeathSprites;
                Score.finalScore = Player.Instance.score;
	            Player.Data.enemyDeathSprites = null;
	            Player.Data.hp = Player.Instance.maxHP;
	            Time.timeScale = 1;
                SceneManager.LoadScene(Player.Instance.deathScene);
	        }

            if (count == 0)
	        {
	            count = TimeBetweenFrames;
	            renderer.sprite = TransitionFrames[currentFrame];
	            currentFrame++;
	        }
            else
            {
                count--;
            }

	    }
    }
}
