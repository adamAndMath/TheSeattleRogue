using UnityEngine;
using System.Collections;

public class RenderLayer : MonoBehaviour
{
    public string layer;
    public int orderInLayer;
    public Renderer renderer;

	void Start ()
	{
	    renderer = GetComponent<Renderer>();
	    renderer.sortingLayerName = layer;
	    renderer.sortingOrder = orderInLayer;

	}
}
