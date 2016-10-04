using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lifemanager : MonoBehaviour
{
    public Sprite emptyHeart;
    public Sprite fullHeart;

    public GameObject healthParent;
    //to get the prefab that the image is on
    public Image heartPrefab;

    List<Image> healthBar = new List<Image>();

	void Start ()
    {
        for (int i = 0; i < Player.Instance.maxHP; i++)
        {
            Image clone = Instantiate(heartPrefab);
            clone.transform.SetParent(transform);
            healthBar.Add(clone);
        }
	}


    void Update () {
        
	}
}
