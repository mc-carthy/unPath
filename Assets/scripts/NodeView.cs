using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeView : MonoBehaviour {

    [Range(0, 0.5f)]
    public float borderSize = 0.15f;

    public GameObject tile;
    public GameObject arrow;

    private Node node;

    public void Init(Node node)
    {
        if (tile != null)
        {
            gameObject.name = "Node (" + node.xIndex + "," + node.yIndex + ")";
            gameObject.transform.position = node.position;
            tile.transform.localScale = Vector3.one * (1 - borderSize);
            this.node = node;
            EnableObject(arrow, false);
        }
    }

    public void ColourNode(Color colour)
    {
        ColourNode(colour, tile);
    }

    public void ShowArrow(Color colour)
    {
        if (
            node != null && 
            node.previous != null &&
            arrow != null
        )
        {
            EnableObject(arrow, true);

            Vector3 dirToPrevious = (node.previous.position - node.position).normalized;
            float rotZ = Mathf.Atan2(dirToPrevious.y, dirToPrevious.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotZ - 90f));

            Renderer[] renderers = arrow.GetComponentsInChildren<Renderer>();
            if (renderers.Length > 0)
            {
                foreach (Renderer ren in renderers)
                {
                    ren.material.color = colour;
                }
            }
        }
    }

    private void ColourNode(Color colour, GameObject go)
    {
        if (go != null)
        {
            Renderer goRen = go.GetComponent<Renderer>();
            goRen.material.color = colour;
        }
    }

    private void EnableObject(GameObject go, bool state)
    {
        if (go != null)
        {
            go.SetActive(state);
        }
    }
}
