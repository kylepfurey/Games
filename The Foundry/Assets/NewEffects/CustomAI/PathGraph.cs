using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//PATH GRAPH
//This is an alternative to the Graph script we set up for a previous assignment
//Unlike the original graph, this one is fully designed to use monobehaviour-based nodes, exclusively for pathfinding. No bastardization of generic graphs with pathfinding graphs here.
//The pathfinding algorithms themselves exist in their own C# files, which this script references.
//WRITTEN BY LIAM SHELTON
public class PathGraph : MonoBehaviour
{
    public List<PathNode2> nodes = new List<PathNode2>(); //The list of nodes in this graph. Useful on occasions in which script needs to be applied to every node of the graph.

    public float distanceToConnect = 4f; //Proximity for connecting nodes

    public static PathGraph pathGraph; //singleton reference

    public bool debugMode = true; //Turn this off to stop pathnodes from displaying their connection lines
    public bool debugConsole = false; //WARNING: turning this on will cause massive performance losses in the Unity editor due to the vast amount of string messages. Keep off unless debugging the pathfinding system.

    //These next 4 are references to the algorithm scripts. Figured it would be cleaner than trying to cram everything into this one script
    public SearchAlgorithm_Template BFSAlgorithm;
    public SearchAlgorithm_Template HeuristicAlgorithm;
    public SearchAlgorithm_Template DijkstraAlgorith;
    public SearchAlgorithm_Template AStarAlgorithm;

    //Initialize important references here. Try to make sure this will work with the script execution order.
    void Awake()
    {
        pathGraph = this;
        BFSAlgorithm = new BreadthFirstSearch(); //there's probably a cleaner way to do this, but I'm not sure what it is
        HeuristicAlgorithm = new HeuristicSearch();
        DijkstraAlgorith = new DijkstraSearch();
        AStarAlgorithm = new AStarSearch();
    }

    #region required functions
    //These four functions are set up as required by the assignment.
    //However, they are not actually used by anything. It seemed more effective to use a single function that takes an algorithm type as an argument (see next)
    public List<PathNode2> BreadthFirstSearch(PathNode2 start, PathNode2 target)
    {
        return BFSAlgorithm.GetPath(start, target);
    }
    public List<PathNode2> HeuristicSearch(PathNode2 start, PathNode2 target)
    {
        return HeuristicAlgorithm.GetPath(start, target);
    }
    public List<PathNode2> DjikstraSearch(PathNode2 start, PathNode2 target)
    {
        return DijkstraAlgorith.GetPath(start, target);
    }
    public List<PathNode2> AStarSearch(PathNode2 start, PathNode2 target)
    {
        return AStarAlgorithm.GetPath(start, target);
    }
    #endregion

    //This a function other scripts may call to recieve paths between two nodes. This is the first version and is less frequently (ie never) used now that the vector3-based function is set up
    public List<PathNode2> FindPath(PathNode2 start, PathNode2 target, AlgorithmType type)
    {
        if (type == AlgorithmType.BreadthFirstSearch)
        {
            return BFSAlgorithm.GetPath(start, target);
        }
        else if (type == AlgorithmType.Heuristic)
        {
            return HeuristicAlgorithm.GetPath(start, target);
        }
        else if (type == AlgorithmType.Dijkstras)
        {
            return DijkstraAlgorith.GetPath(start, target);
        }
        else if (type == AlgorithmType.AStar)
        {
            return AStarAlgorithm.GetPath(start, target);
        }
        else
        {
            return null;
        }
    }
    //This is an overload that the CustomNavAgents call when they need pathfinding to a specific Vector3 position. 
    //It uses the FindClosestNode() method to determine the node that is closest to the desired position
    public List<PathNode2> FindPath(PathNode2 start, Vector3 aPosition, AlgorithmType type)
    {
        if (type == AlgorithmType.BreadthFirstSearch)
        {
            return BFSAlgorithm.GetPath(start, FindClosestNode(aPosition));
        }
        else if (type == AlgorithmType.Heuristic)
        {
            return HeuristicAlgorithm.GetPath(start, FindClosestNode(aPosition));
        }
        else if (type == AlgorithmType.Dijkstras)
        {
            return DijkstraAlgorith.GetPath(start, FindClosestNode(aPosition));
        }
        else if (type == AlgorithmType.AStar)
        {
            return AStarAlgorithm.GetPath(start, FindClosestNode(aPosition));
        }
        else
        {
            return null;
        }
    }

    //This method is used by the graph to find the node that is closest to a Vector3 coordinate.
    PathNode2 FindClosestNode(Vector3 aPosition)
    {
        PathNode2 closestNode = null;
        float smallestDistance = Mathf.Infinity;

        foreach (PathNode2 node in nodes)
        {
            float distance = Vector3.Distance(node.transform.position, aPosition);
            if (distance < smallestDistance)
            {
                closestNode = node;
                smallestDistance = distance;
            }

        }
        PathGraph.Log($"CLOSEST NODE TO COORDS {aPosition}:{closestNode.gameObject.name}");
        return closestNode;
    }

    //Call this from the PathNodes in Start() or Awake(). Unknown if new nodes can be added at runtime. Make sure the node is parented to the graph when it gets called!
    public void AddNode(PathNode2 newNode)
    {
        foreach (PathNode2 node in nodes)
        {
            //if close enough, make the connections
            if (Vector3.Distance(newNode.gameObject.transform.position, node.gameObject.transform.position) <= distanceToConnect)
            {
                node.connectedNodes.Add(newNode);
                newNode.connectedNodes.Add(node);
            }
        }
        nodes.Add(newNode);
    }


    //The following functions are quick-and-dirty ways to disable Debug.Log functionalities of the pathfinding system
    //Very important for performance, because otherwise the framerate will stutter every time pathfinding gets used
    //Also useful as a way to exaggerate the performance differences between the algorithms, for educational purposes.
    public static void Log(string message)
    {
        if (pathGraph.debugMode == true && pathGraph.debugConsole == true)
        {
            Debug.Log($"PATHFINDING: {message}");
        }
    }
    public static void LogWarning(string message)
    {
        if (pathGraph.debugMode == true && pathGraph.debugConsole == true)
        {
            Debug.LogWarning($"PATHFINDING: {message}");
        }
    }
    public static void LogError(string message)
    {
        //Debug.LogError($"PATHFINDING: {message}");
    }
}
