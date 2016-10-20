using UnityEngine;
using System.Collections;

public class SpawnNode : MonoBehaviour
{
    public GameObject[] Spawns = new GameObject[1];
    private int random;
    private GameObject SpawnedEnemy;

	void Start ()
	{
	    random = Random.Range(0, Spawns.Length);
	    SpawnedEnemy = Instantiate(Spawns[random],transform.position,transform.rotation) as GameObject;
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !SpawnedEnemy)
        {
            Instantiate(Spawns[random], transform.position, transform.rotation);
        }
    }
	
}
