using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//HEURISTIC SEARCH
//This is largey the same as the BreadthFirstSearch algorithm, but uses distance from the endNode as a heuristic
//The only functional difference in the code (other than color) is the part that chooses which node to search next
//By directing the search towards the end node, this search algorithm should be significantly less wasteful of resources and should return a more direct path.
//It will also cause paths to go all the way into crevice-shaped obstacles before navigating around them.
//WRITTEN BY LIAM SHELTON
public class HeuristicSearch : SearchAlgorithm_Template
{
    public override List<PathNode2> GetPath(PathNode2 start, PathNode2 end)
    {
        //quick-and-dirty line color initialization
        myColor = Color.yellow;
        randomOffset = new Vector3(Random.Range(-.15f, .15f), Random.Range(0, .15f), Random.Range(-.15f, .15f));

        //Initialize some list variables for the loops to work with
        List<PathNode2> theSearchList = new List<PathNode2>();
        List<PathNode2> Cleared = new List<PathNode2>();
        PathNode2 currentNode = start;


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

        //If we reach this point successfully, then the Cleared list is now effectively the stack and can be treated as such for recursive trace purposes.
        return RecursiveTrace(start, end, Cleared);
    }


    //This is the largely copy/pasted SearchCurrentNode function from the BreadthFirstSearch script
    //It is essentially a single cycle of the search process. It will mark the current node as cleared, add any unchecked connections to the search list, and then assign the next node that will be check when this function is next called
    public void SearchCurrentNode(List<PathNode2> theSearchList, List<PathNode2> Cleared, ref PathNode2 currentNode, PathNode2 endNode)
    {
        PathGraph.Log($"HEURISTIC Searching node {currentNode.gameObject.name}");
        Cleared.Add(currentNode);

        theSearchList.Remove(currentNode);
        //I split this part off to make it easier to think about what was happening. The only difference was the very last line, which is not affected by its existence in a separate function
        SplitThisPartOff(theSearchList, Cleared, ref currentNode, endNode);
    }

    //This method is just the foreach part of the original SearchCurrentNode method, split off in hopes of thinking about what was happening more easily. 
    //The only thing that has changed is the very last line, which uses the HeuristicSelection method instead of simply choosing the oldest item in theSearchList
    public void SplitThisPartOff(List<PathNode2> theSearchList, List<PathNode2> Cleared, ref PathNode2 currentNode, PathNode2 endNode)
    {
        //original stuff below this point. If the connected node is not in the cleared (aka visited) list and is not already in the search list, then add the node to the search list so that our script will know what options it has to search next.
        foreach (PathNode2 node in currentNode.connectedNodes)
        {
            PathGraph.Log($"SEARCH OF NODE {currentNode.gameObject.name}: Checking connected node of name {node.gameObject.name}...");
            if (!Cleared.Contains(node) && !theSearchList.Contains(node))
            {
                PathGraph.Log($"node of name {node.gameObject.name} was added to the search list...");
                DrawSearchLine(currentNode.transform.position, node.transform.position);
                theSearchList.Add(node);
            }
        }

        //This is the only part of this entire script that is functionally different from the BreadthFirstSearch.
        //By using the HeuristicSelection method instead of the first index of theSearchList, the search pattern will move in the direction of the endpoint rather than propogate in a circle around the start point
        //In practice, this also appears to have the side effect of returning a straighter, less zigzagging path.
        currentNode = HeuristicSelection(theSearchList, Cleared, endNode); 
    }
}
