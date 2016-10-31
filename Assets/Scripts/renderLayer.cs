using UnityEngine;
using System.Collections;

public class renderLayer : MonoBehaviour
{
    public string Layer;
    public int OrderInLayer;
    public Renderer renderer;

	void Start ()
	{
	    renderer = GetComponent<Renderer>();
	    renderer.sortingLayerName = Layer;
	    renderer.sortingOrder = OrderInLayer;

	}
}
