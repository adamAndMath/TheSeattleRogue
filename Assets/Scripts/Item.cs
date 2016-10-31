using UnityEngine;

public class Item : ScriptableObject
{
    public Sprite sprite;
    public AnimatorOverrideController animator;
    public Vector2 collisionOffset;
    public Vector2 collisionSize;
    public int damage;
    public int price;
}
