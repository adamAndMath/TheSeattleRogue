using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("Moving", !Mathf.Approximately(0, Input.GetAxisRaw("Horizontal")));

        if (animator.GetComponent<Collider2D>().Cast(Vector2.left, new RaycastHit2D[8], 10) > 0)
        {
            Debug.Log("E");
        }
    }
}
