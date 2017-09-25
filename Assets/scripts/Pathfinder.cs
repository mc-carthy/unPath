using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour {

    private Node startNode, goalNode;
    private Graph graph;
    private GraphView graphView;

    private Queue<Node> frontierNodes;
    private List<Node> exploredNodes;
    private List<Node> pathNodes;

    public Color startColour = Color.green;
    public Color goalColour = Color.red;
    public Color frontierColour = Color.magenta;
    public Color exploredColour = Color.grey;
    public Color pathColour = Color.cyan;

    public void Init(Graph graph, GraphView graphView, Node start, Node goal)
    {
        if (start == null || goal == null || graph == null || graphView == null)
        {
            Debug.LogWarning("PATHFINDER Init error: Missing component(s)!");
            return;
        }

        if (start.nodeType == NodeType.Blocked || goal.nodeType == NodeType.Blocked)
        {
            Debug.LogWarning("PATHFINDER Init error: Start/Goal nodes must be open");
            return;
        }

        this.graph = graph;
        this.graphView = graphView;
        this.startNode = start;
        this.goalNode = goal;

        NodeView startNodeView = graphView.nodeViews[start.xIndex, start.yIndex];
        if (startNodeView != null)
        {
            startNodeView.ColourNode(startColour);
        }

        NodeView goalNodeView = graphView.nodeViews[goal.xIndex, goal.yIndex];
        if (goalNodeView != null)
        {
            goalNodeView.ColourNode(goalColour);
        }

        frontierNodes = new Queue<Node>();
        frontierNodes.Enqueue(start);
        exploredNodes = new List<Node>();
        pathNodes = new List<Node>();

        for (int y = 0; y < graph.Height; y++)
        {
            for (int x = 0; x < graph.Width; x++)
            {
                graph.nodes[x, y].Reset();
            }
        }
    }
}
