using UnityEngine;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class ItemDrop
{
    public GameObject obj;
    [Range(0, 1)]
    public float chance;

    public void Drop(Vector2 position)
    {
        if (Random.value > chance) return;

        GameObject clone = GameObject.Instantiate(obj);
        clone.transform.position = position;
    }
}
