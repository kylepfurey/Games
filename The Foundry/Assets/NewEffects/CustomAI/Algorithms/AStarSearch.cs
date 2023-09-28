using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//A* SEARCH
//This is the A* search algorithm, which combines Dijkstra's algorithm with heuristics.
//It should generally return a path that is just shy of being the shortest possible, without wasting nearly as much reasources as Dijkstra's on a radial search pattern.
//It was largely copy/pasted from the DijkstraSearch, with just one single line being changed.
//WRITTEN BY LIAM SHELTON
public class AStarSearch : SearchAlgorithm_Template
{
    public override List<PathNode2> GetPath(PathNode2 start, PathNode2 end)
    {
        //quick-and-dirty line color initialization
        myColor = Color.cyan;
        randomOffset = new Vector3(Random.Range(-.15f, .15f), Random.Range(0, .15f), Random.Range(-.15f, .15f));

        //Initialize some list variables for the loops to work with
        List<PathNode2> theSearchList = new List<PathNode2>();
        List<PathNode2> Cleared = new List<PathNode2>();
        PathNode2 currentNode = start;

        //Initialize the pathnodes by setting their dijkstraDistances to infinity
        foreach (PathNode2 node in PathGraph.pathGraph.nodes)
        {
            node.dijkstraDistance = Mathf.Infinity;
        }
        start.dijkstraDistance = 0f;//except for the very first one, which should be set to 0


        int emergencyStopper = 0; //safety

        //This loop is where the search takes place. It should go on until either the end node is located, or until an emergency stop is triggered
        while (currentNode != end)
        {
            SearchCurrentNode(theSearchList, Cleared, ref currentNode, end);

            emergencyStopper++;
            if (emergencyStopper > 10000)
            {
                PathGraph.LogError("EMERGENCY STOP: LOOPED NODE SEARCH");
                break;
            }
        }

        //If we reach this point successfully, then all nodes that could potentially be part of the path will have had their dijkstraDistance values calculated and can be used to trace the shortest path.
        return RecursiveTraceDijkstra(start, end);
    }


    //This is the largely copy/pasted SearchCurrentNode function from the DijkstraSearch script
    //It is essentially a single cycle of the search process. It will mark the current node as cleared, add any unchecked connections to the search list, and then assign the next node that will be check when this function is next called
    //A major difference is the block of code that performs the distance-assigning process.
    //As any Dijkstra's algorithm, if the current distance is less than the distance we could have from the current node, then update the distance.
    public void SearchCurrentNode(List<PathNode2> theSearchList, List<PathNode2> Cleared, ref PathNode2 currentNode, PathNode2 endNode)
    {
        if (currentNode != null)
        {
            PathGraph.Log($"A* Searching node {currentNode.gameObject.name}");
            Cleared.Add(currentNode);

            theSearchList.Remove(currentNode);

            foreach (PathNode2 node in currentNode.connectedNodes)
            {
                PathGraph.Log($"SEARCH OF NODE {currentNode.gameObject.name}: Checking connected node of name {node.gameObject.name}...");

                //do the distance assignment thingy for any connected node that hasn't been cleared. Make sure to do this even for nodes that are already in theSearchList!
                //This should happen for every node until we find the end node, and will therefore make it easy to figure out the shortest path once we find the end node 
                {
                    PathGraph.Log($"running distance functionality on node {node.gameObject.name}...");
                    float dist = currentNode.dijkstraDistance + Vector3.Distance(currentNode.transform.position, node.transform.position);
                    //if the distance from current node to the connected node is lower than what it currently says, update it
                    if (dist < node.dijkstraDistance)
                    {
                        PathGraph.Log($"looks like a shorter path to {node.gameObject.name} was found (original: {node.dijkstraDistance}, new: {dist})...");
                        node.dijkstraDistance = dist;
                    }
                    else //otherwise, leave it be
                    {
                        PathGraph.Log($"original djikstra distance of node {node.gameObject.name} not shorter than new path. (original: {node.dijkstraDistance}, new: {dist})");
                    }
                }
                //original stuff below this point. If the connected node is not in the cleared (aka visited) list and is not already in the search list, then add the node to the search list so that our script will know what options it has to search next.
                if (!Cleared.Contains(node) && !theSearchList.Contains(node))
                {
                    PathGraph.Log($"node of name {node.gameObject.name} was added to the search list...");
                    DrawSearchLine(currentNode.transform.position, node.transform.position);
                    theSearchList.Add(node);
                }
            }
        }

        //This is the single line that has been changed. Instead of searching the first index of the search list, search the node in the list that has the closest proximity to the end node!
        //This will change the search pattern. Instead of searching in a radius, this will search in the direction of the end node as much as possible.
        currentNode = HeuristicSelection(theSearchList, Cleared, endNode);
    }
}
