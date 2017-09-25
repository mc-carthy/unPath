﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
    public MapData mapData;
    public Graph graph;
    public Pathfinder pathfinder;
    
    public float timestep = 0.05f;

    private int startX = 0;
    private int startY = 3;
    private int goalX = 39;
    private int goalY = 1;

    private void Start()
    {
        if (mapData != null && graph != null)
        {
            int[,] mapInstance = mapData.MakeMap();
            graph.Init(mapInstance);

            GraphView graphView = graph.gameObject.GetComponent<GraphView>();

            if (graphView != null)
            {
                graphView.Init(graph);
            }

            if (
                graph.IsWithinBounds(startX, startY) && 
                graph.IsWithinBounds(goalX, goalY) &&
                pathfinder != null
            )
            {
                Node startNode = graph.nodes[startX, startY];
                Node goalNode = graph.nodes[goalX, goalY];
                pathfinder.Init(graph, graphView, startNode, goalNode);
                StartCoroutine(pathfinder.SearchRoutine(timestep));
            }
        }
    }

}
