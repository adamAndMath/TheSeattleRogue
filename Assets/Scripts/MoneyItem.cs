using UnityEngine;
using System.Collections;

public class MoneyItem : PhysicsObject
{
    public Vector2 speed;

    void Update()
    {
        if (IsGrounded())
            speed = Vector2.zero;
        else
        {
            if (MoveHorizontal(speed.x * Time.deltaTime))
                speed.x = 0;
            if (MoveVertical(speed.y * Time.deltaTime))
                speed.y = 0;
        }
    }
}
