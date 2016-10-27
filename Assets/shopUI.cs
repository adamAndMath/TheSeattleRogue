using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class shopUI : MonoBehaviour
{
    public GameObject InfoScreenGameObject;
    public GameObject WeaponBoxGameObject;
    public GameObject WeaponDisplayAreaGameObject;
    public GameObject[] WeaponsList = new GameObject[1];

	void Start ()
    {
	    for (int i = 0; i < WeaponsList.Length; i++)
	    {
	        GameObject webBox = Instantiate(WeaponBoxGameObject, WeaponDisplayAreaGameObject.transform)as GameObject;


	        Instantiate(WeaponsList[i], webBox.transform);

            Debug.Log(webBox.transform.position);
	    }
	}

}
