using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lifemanager : MonoBehaviour
{
    public Sprite emptyHeart;
    public Sprite fullHeart;
    
    //to get the prefab that the image is on
    public Image heartPrefab;

    List<Image> healthBar = new List<Image>();

	void Start ()
    {
        
	}


    void Update ()
    {
        while (Player.Instance.maxHP > healthBar.Count)
        {
            Image clone = Instantiate(heartPrefab);
            clone.transform.SetParent(transform);
            healthBar.Add(clone);
        }
        
        while (Player.Instance.maxHP < healthBar.Count)
        {
            Image clone = healthBar[healthBar.Count - 1];
            healthBar.Remove(clone);
            Destroy(clone);
        }
        
        for (int i = 0; i < healthBar.Count; i++)
        {
            Image clone = healthBar[i];

            if (Player.Data.hp > i)
            {
                clone.sprite = fullHeart;
            }
            else
            {
                clone.sprite = emptyHeart;
            }
        }
    }
}
