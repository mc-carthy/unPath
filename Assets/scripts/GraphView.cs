using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Graph))]
public class GraphView : MonoBehaviour {

    public GameObject nodeViewPrefab;
    public NodeView[,] nodeViews;

    public void Init(Graph graph)
    {
        if (graph == null)
        {
            Debug.LogWarning("GRAPHVIEW - No graph to initialise!");
            return;
        }

        nodeViews = new NodeView[graph.Width, graph.Height];

        foreach(Node n in graph.nodes)
        {
            GameObject instance = Instantiate(nodeViewPrefab, Vector3.zero, Quaternion.identity);
            NodeView nodeView = instance.GetComponent<NodeView>();

            if (nodeView != null)
            {
                nodeView.Init(n);
                nodeViews[n.xIndex, n.yIndex] = nodeView;

                Color originalColour = MapData.GetColourFromNodeType(n.nodeType);
                nodeView.ColourNode(originalColour);
            }
        }
    }

    public void ColourNodes(List<Node> nodes, Color colour, bool lerpColour = false, float lerpValue = 0.5f)
    {
        foreach (Node n in nodes)
        {
            if (n != null)
            {
                NodeView nodeView = nodeViews[n.xIndex, n.yIndex];
                Color newColour = colour;

                if (lerpColour)
                {
                    Color originalColour = MapData.GetColourFromNodeType(n.nodeType);
                    newColour = Color.Lerp(originalColour, newColour, lerpValue);
                }

                if (nodeView != null)
                {
                    nodeView.ColourNode(newColour);
                }
            }
        }
    }

    public void ShowNodeArrows(Node node, Color colour)
    {
        if (node != null)
        {
            NodeView nodeView = nodeViews[node.xIndex, node.yIndex];
            if (nodeView != null) {
                nodeView.ShowArrow(colour);
            }
        }
    }

    public void ShowNodeArrows(List<Node> nodes, Color colour)
    {
        foreach(Node n in nodes)
        {
            ShowNodeArrows(n, colour);
        }
    }
}
