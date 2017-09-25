﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public bool isComplete = false;
    private int iterations = 0;

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

        isComplete = false;
        iterations = 0;
    }

    private void ShowColours()
    {
        ShowColours(graphView, startNode, goalNode);
    }

    private void ShowColours(GraphView graphView, Node start, Node goal)
    {
        if (graphView == null || start == null || goal == null)
        {
            Debug.LogWarning("PATHFINDER - Show colours error, missing arguments");
            return;
        }

        if (frontierNodes != null)
        {
            graphView.ColourNodes(frontierNodes.ToList(), frontierColour);
        }

        if (exploredNodes != null)
        {
            graphView.ColourNodes(exploredNodes, exploredColour);
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

                ExpandFrontier(currentNode);
                ShowColours();

                yield return new WaitForSeconds(timestep);
            }
            else
            {
                isComplete = true;
            }
        }
    }

    private void ExpandFrontier(Node node)
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
                    node.neighbours[i].previous = node;
                    frontierNodes.Enqueue(node.neighbours[i]);
                }
            }
        }
    }
}