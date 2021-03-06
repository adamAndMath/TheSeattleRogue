﻿using UnityEngine;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class ItemDrop
{
    public GameObject obj;
    public float chance;

    public void Drop(Transform parent, Vector2 position)
    {
        if (Random.value > chance) return;

        GameObject clone = GameObject.Instantiate(obj);
        clone.transform.position = position;
        clone.transform.SetParent(parent);
    }
}
