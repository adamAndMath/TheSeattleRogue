using UnityEngine;
using System.Collections;

public class SpawnNode : MonoBehaviour
{
    public GameObject[] Spawns = new GameObject[1];
    private int random;

	void Start ()
	{
	    random = Random.Range(0, Spawns.Length);
	    Instantiate(Spawns[random],transform.position,transform.rotation);
        Debug.Log(""+random);
	}
	
}
