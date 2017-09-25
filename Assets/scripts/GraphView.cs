using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Graph))]
public class GraphView : MonoBehaviour {

    public GameObject nodeViewPrefab;
    public NodeView[,] nodeViews;

    public Color baseColour = Color.white;
    public Color wallColour = Color.black;

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

                if (n.nodeType == NodeType.Blocked)
                {
                    nodeView.ColourNode(wallColour);
                }
                else
                {
                    nodeView.ColourNode(baseColour);
                }
            }
        }
    }

    public void ColourNodes(List<Node> nodes, Color colour)
    {
        foreach (Node n in nodes)
        {
            if (n != null)
            {
                NodeView nodeView = nodeViews[n.xIndex, n.yIndex];
                if (nodeView != null)
                {
                    nodeView.ColourNode(colour);
                }
            }
        }
    }
}
