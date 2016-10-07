using UnityEngine;
using System.Collections;

public class GuardRobotBehaviour : Enemy {
    public float threatRangeRight;
    public float threatRangeLeft;
    private Vector3 rightRange;
    private Vector3 leftRange;

	// Use this for initialization
	void Start () 
    {
        rightRange = new Vector3(threatRangeRight, 0);
        leftRange = new Vector3(threatRangeLeft, 0);
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position-leftRange);
        Gizmos.DrawLine(transform.position, transform.position+rightRange);
    }
}

