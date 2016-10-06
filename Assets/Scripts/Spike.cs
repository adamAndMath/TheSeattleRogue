using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour {

    Collider2D spikeCol;

	void Start () {
        spikeCol = this.GetComponent<Collider2D>();
	}
	

	void Update () {
	    if (spikeCol.IsTouching(Player.Instance.GetComponent<Collider2D>()))
        {
            Player.Instance.Damaged(1);
        }
	}
}
