using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeView : MonoBehaviour {

    public GameObject tile;

    [Range(0, 0.5f)]
    public float borderSize = 0.15f;

    public void Init(Node node)
    {
        if (tile != null)
        {
            gameObject.name = "Node (" + node.xIndex + "," + node.yIndex + ")";
            gameObject.transform.position = node.position;
            tile.transform.localScale = Vector3.one * (1 - borderSize);
        }
    }

    public void ColourNode(Color colour)
    {
        ColourNode(colour, tile);
    }

    private void ColourNode(Color colour, GameObject go)
    {
        if (go != null)
        {
            Renderer goRen = go.GetComponent<Renderer>();
            goRen.material.color = colour;
        }
    }
}
