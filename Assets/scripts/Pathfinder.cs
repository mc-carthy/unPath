using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinder : MonoBehaviour {

    private Node startNode, goalNode;
    private Graph graph;
    private GraphView graphView;

    private PriorityQueue<Node> frontierNodes;
    private List<Node> exploredNodes;
    private List<Node> pathNodes;

    public bool showIterations = true;
    public bool showColours = true;
    public bool showArrows = true;
    public bool exitOnComplete = true;

    public Color startColour = Color.green;
    public Color goalColour = Color.red;
    public Color frontierColour = Color.magenta;
    public Color exploredColour = Color.grey;
    public Color pathColour = Color.cyan;
    public Color arrowColour = new Color(0.85f, 0.85f, 0.85f, 1f);
    public Color highlightColour = new Color(1f, 1f, 0.5f, 1f);

    public bool isComplete = false;
    private int iterations = 0;

    public enum Mode
    {
        BreadthFirst = 0,
        Dijkstra = 1
    }

    public Mode mode = Mode.BreadthFirst;

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

        ShowColours(graphView, start, goal);

        frontierNodes = new PriorityQueue<Node>();
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

        isComplete = false;
        iterations = 0;
        startNode.distanceTravelled = 0;
    }

    private void ShowColours(bool lerpColour = false, float lerpValue = 0.5f)
    {
        ShowColours(graphView, startNode, goalNode, lerpColour, lerpValue);
    }

    private void ShowColours(GraphView graphView, Node start, Node goal, bool lerpColour = false, float lerpValue = 0.5f)
    {
        if (graphView == null || start == null || goal == null)
        {
            Debug.LogWarning("PATHFINDER - Show colours error, missing arguments");
            return;
        }

        if (frontierNodes != null)
        {
            graphView.ColourNodes(frontierNodes.ToList(), frontierColour, lerpColour, lerpValue);
        }

        if (exploredNodes != null)
        {
            graphView.ColourNodes(exploredNodes, exploredColour, lerpColour, lerpValue);
        }

        if (pathNodes != null && pathNodes.Count > 0)
        {
            graphView.ColourNodes(pathNodes, pathColour, lerpColour, lerpValue * 1.5f);
        }

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
    }

    public IEnumerator SearchRoutine(float timestep = 0.1f)
    {
        float timeStart = Time.time;

        yield return null;

        while (!isComplete)
        {
            if (frontierNodes.Count > 0)
            {
                Node currentNode = frontierNodes.Dequeue();
                iterations++;

                if (!exploredNodes.Contains(currentNode))
                {
                    exploredNodes.Add(currentNode);
                }

                if (mode == Mode.BreadthFirst)
                {
                    ExpandFrontierBreadthFirst(currentNode);
                }
                else if (mode == Mode.Dijkstra)
                {
                    ExpandFrontierDijkstra(currentNode);
                }

                if (frontierNodes.Contains(goalNode))
                {
                    pathNodes = GetPathNodes(goalNode);
                    if (exitOnComplete)
                    {
                        isComplete = true;
                    }
                }

                if (showIterations)
                {
                    ShowDiagnostics(true, 0.5f);

                    yield return new WaitForSeconds(timestep);
                }
            }
            else
            {
                isComplete = true;
            }
        }
        ShowDiagnostics(true, 0.5f);
        Debug.Log("PATHFINDER - SearchRoutine: Completed in " + (Time.time - timeStart).ToString() + " seconds.");
        Debug.Log("PATHFINDER mode: " + mode.ToString() + ". Path length: " + goalNode.distanceTravelled.ToString() + " units");
    }

    private void ShowDiagnostics(bool lerpColour = false, float lerpValue = 0.5f)
    {
        if (showColours)
        {
            ShowColours(lerpColour, lerpValue);
        }

        if (showArrows)
        {
            if (graphView != null)
            {
                graphView.ShowNodeArrows(frontierNodes.ToList(), arrowColour);

                if (frontierNodes.Contains(goalNode))
                {
                    graphView.ShowNodeArrows(pathNodes, highlightColour);
                }
            }
        }
    }

    private void ExpandFrontierBreadthFirst(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.neighbours.Count; i++)
            {
                if (
                    !exploredNodes.Contains(node.neighbours[i]) && 
                    !frontierNodes.Contains(node.neighbours[i])
                )
                {
                    float distanceToNeighbor = graph.GetNodeDistance(node, node.neighbours[i]);
                    float newDistanceTravelled = distanceToNeighbor + node.distanceTravelled + (int)node.nodeType;
                    node.neighbours[i].distanceTravelled = newDistanceTravelled;

                    node.neighbours[i].previous = node;

                    // This is a hack to make the priority queue behave like a
                    // regular queue in terms of FIFO
                    node.neighbours[i].priority = exploredNodes.Count;
                    
                    frontierNodes.Enqueue(node.neighbours[i]);
                }
            }
        }
    }

    private void ExpandFrontierDijkstra(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.neighbours.Count; i++)
            {
                if (!exploredNodes.Contains(node.neighbours[i]))
                {
                    float distanceToNeighbor = graph.GetNodeDistance(node, node.neighbours[i]);
                    float newDistanceTravelled = distanceToNeighbor + node.distanceTravelled + (int)node.nodeType;

                    if (
                        float.IsPositiveInfinity(node.neighbours[i].distanceTravelled) || 
                        newDistanceTravelled < node.neighbours[i].distanceTravelled
                    )
                    {
                        node.neighbours[i].previous = node;
                        node.neighbours[i].distanceTravelled = newDistanceTravelled;
                    }

                    if (!frontierNodes.Contains(node.neighbours[i]))
                    {
                        node.neighbours[i].priority = (int)node.neighbours[i].distanceTravelled;
                        frontierNodes.Enqueue(node.neighbours[i]);
                    }
                }
            }
        }
    }

    private List<Node> GetPathNodes(Node endNode)
    {
        List<Node> path = new List<Node>();

        if (endNode == null)
        {
            return path;
        }

        path.Add(endNode);

        Node currentNode = endNode.previous;

        while (currentNode != null)
        {
            path.Insert(0, currentNode);
            currentNode = currentNode.previous;
        }

        return path;
    }
}
