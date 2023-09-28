using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SEARCH ALGORITHM TEMPLATE
//This is an abstract class for the four pathfinding algorithms to inherit from.
//Contains functionality for drawing debug lines, recursive trace procedures, and heuristic list logic
//WRITTEN BY LIAM SHELTON

[System.Serializable]
public abstract class SearchAlgorithm_Template
{
    //For drawing the lines
    public float debugDuration = 5f;
    public Color myColor = Color.red;
    public Vector3 randomOffset;

    //Use this function to use Debug.DrawLine more easily in the inheriting classes.
    //Also prevents lines from being drawn when graph debug mode is disabled.
    protected void DrawSearchLine(Vector3 start, Vector3 end)
    {
        if (PathGraph.pathGraph.debugMode)
        {
            Color color = myColor;
            color.a = .175f;
            Debug.DrawLine(start + (randomOffset), end + (randomOffset), color, debugDuration);
        }
    }
    //Use this function to use Debug.DrawLine more easily, preferably within the recursive trace functins within this script.
    //Also prevents lines from being drawn when graph debug mode is disabled.
    void DrawPathLine(Vector3 start, Vector3 end)
    {
        if (PathGraph.pathGraph.debugMode)
        {
            Debug.DrawLine(start + (Vector3.up * .75f + randomOffset), end + (Vector3.up * .75f + randomOffset), myColor, debugDuration);
        }
    }

    //Override this method in the inheriting functions with search algorithm logic!
    public virtual List<PathNode2> GetPath(PathNode2 start, PathNode2 end)
    {
        return null;
    }

    //This function is the "normal" way to trace the path after an algorithm has finished a search. 
    //It will check to see which connected node is in the list of cleared (visited) nodes, and then cut off all nodes in the list that are higher than the current node's index.
    //Eventually, this cycle should result in a list of PathNode2's that leads back to the starting point, with all extra nodes cut out
    protected virtual List<PathNode2> RecursiveTrace(PathNode2 startNode, PathNode2 currentNode, List<PathNode2> Cleared)
    {
        List<PathNode2> thePath = new List<PathNode2>(); //The list of nodes that should make a path between start and end
        PathNode2 tracerNode = currentNode; //The tracer node is what the recursive trace uses to crawl the graph
        thePath.Add(currentNode);

        int emergencystopper = 0;//for safety purposes
        while (true) //Repeat this block until we arrive at the starting point
        {
            PathGraph.Log($"Tracing node {tracerNode.gameObject.name}");
            emergencystopper++;

            Cleared.Remove(tracerNode);//Remove the current traced node from Cleared, so that we don't touch it again
            //Loop through the connected nodes of the current tracer. If they are in the Cleared list and also connected to the current node, then by definition they are a valid part of the path back.
            foreach (PathNode2 node in tracerNode.connectedNodes)
            {
                if (Cleared.Contains(node))
                {
                    //If this part of the code runs, it means the node is valid as the next part of the path
                    DrawPathLine(tracerNode.transform.position, node.transform.position);
                    thePath.Add(node);
                    tracerNode = node;

                    //this block will cut off all nodes in Cleared that came after this node, guaranteeing that the next while cycle won't have anywhere to go except towards the starting point
                    int index = Cleared.IndexOf(node);
                    PathGraph.Log($"index of node {node.gameObject.name}: {Cleared.IndexOf(node)}");
                    Cleared.RemoveRange(index, Cleared.Count - index);
                    break;
                }
            }

            //If there are no more items in the Cleared list, we should be at the starting point!
            if (Cleared.Count == 0)
            {
                PathGraph.Log("OUT OF STACKED NODES");
                thePath.Reverse(); //reverse the list so that the path goes from start node to end node (it was originally built starting from end node)
                return thePath;
            }

            //Hit the emergency brakes if this process goes on for 10K iterations
            if (emergencystopper == 10000)
            {
                PathGraph.LogError("EMERGENCYSTOP RECURSIVE TRACE");
                return null;
            }
        }
    }

    //This function is a modified version of the recursive trace function for use with Dijkstra's and A*
    //Instead of using the stacked list of nodes, this version simply checks for the next connected node with the lowest dijkstraDistance.
    protected virtual List<PathNode2> RecursiveTraceDijkstra(PathNode2 startNode, PathNode2 currentNode)
    {
        List<PathNode2> thePath = new List<PathNode2>(); //The list of nodes that should make a path between start and end
        PathNode2 tracerNode = currentNode; //The tracer node is what the recursive trace uses to crawl the graph
        thePath.Add(currentNode);

        int emergencystopper = 0; //for safety purposes


        while (true) //Repeat this block until we arrive at the starting point
        {
            PathGraph.Log($"Dijkstra-Tracing node {tracerNode.gameObject.name}");
            emergencystopper++;

            //Cleared.Remove(tracerNode);

            //The next node of the list will be whatever node connected to the currently traced node has the lowest Dijkstra distance (distance from start point as calculated by the dijkstra algorithm). This should give us the node that will get us back fastest
            PathNode2 theNode = getNodeWithLowestDijkstraDistance(tracerNode);
            DrawPathLine(tracerNode.transform.position, theNode.transform.position);
            thePath.Add(theNode);
            tracerNode = theNode;

            //If the dijkstraDistance of the tracer node after the previous operations is 0, then we've arrived at our destination. Not sure why the loop doesn't need to happen for the last node, but whatever
            if (tracerNode.dijkstraDistance==0)
            {
                PathGraph.Log("DIJKSTRA DISTANCE 0 REACHED");
                thePath.Reverse();//reverse the list so that the path goes from start node to end node (it was originally built starting from end node)
                return thePath;
            }

            //Hit the emergency brakes if this process goes on for 10K iterations
            if (emergencystopper == 10000)
            {
                PathGraph.LogError("EMERGENCYSTOP RECURSIVE DIJKSTRA TRACE");
                return null;
            }
        }
    }

    //This method is used in the dijkstra recursive trace method to find the node with the shortest dijkstra distance, so that the returned path is the shortest possible one.
    PathNode2 getNodeWithLowestDijkstraDistance(PathNode2 tracerNode)
    {
        PathNode2 closestNode = tracerNode;
        float smallestDistance = Mathf.Infinity;

        foreach (PathNode2 node in tracerNode.connectedNodes)
        {
            if (node.dijkstraDistance < smallestDistance)
            {
                closestNode = node;
                smallestDistance = node.dijkstraDistance;
            }

        }
        PathGraph.Log($"NODE WITH SHORTEST DIJKSTRADISTANCE: {closestNode.gameObject.name}");
        return closestNode;
    }

    //This method conveniently allows algorithms to use distance-based heuristics when choosing which nodes to search next
    //It works by returning the connected node that has the closest proximity to the end node.
    protected PathNode2 HeuristicSelection(List<PathNode2> theSearchList, List<PathNode2> Cleared, PathNode2 endNode)
    {
        PathNode2 closestNode = null;
        float smallestDistance = Mathf.Infinity;

        //Typical technique for finding the item in the list that is closest (in this case, closest to endNode)
        foreach (PathNode2 node in theSearchList)
        {
            if (!Cleared.Contains(node))
            {
                float distance = Vector3.Distance(node.transform.position, endNode.transform.position);
                if (distance < smallestDistance)
                {
                    closestNode = node;
                    smallestDistance = distance;
                }
            }

        }
        return closestNode;
    }

}
