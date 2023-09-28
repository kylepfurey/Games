using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//BREADTH FIRST SEARCH
//This is the search algorithm that uses straight-up BFS.
//Naturally, it's the least efficient performance-wise and usually returns a janky, inefficient path.
//Although the structure of these search algorithms might seem conducive to inheritance, the other search algorithms copy/paste a gread deal from this script.
//Much of the code here was prototyped in a different script called "BFA_Stack_Crawler", which has since been deleted. It's still available in previous fork commits
//WRITTEN BY LIAM SHELTON
public class BreadthFirstSearch : SearchAlgorithm_Template
{
    public override List<PathNode2> GetPath(PathNode2 start, PathNode2 end)
    {
        //quick-and-dirty line color initialization
        myColor = Color.red;
        randomOffset = new Vector3(Random.Range(-.15f, .15f), Random.Range(0, .15f), Random.Range(-.15f, .15f));

        //Initialize some list variables for the loops to work with
        List<PathNode2> theSearchList = new List<PathNode2>();
        List<PathNode2> Cleared = new List<PathNode2>();
        PathNode2 currentNode = start;


        int emergencyStopper = 0; //safety

        //This loop is where the search takes place. It should go on until either the end node is located, or until an emergency stop is triggered
        while (currentNode != end)
        {
            SearchCurrentNode(theSearchList, Cleared, ref currentNode, end); //Not sure why the 'ref' thingy was necessary for the currentNode when it wasn't for any of the other parameters, but whatev.

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


    //This is the largely copy/pasted SearchCurrentNode function from the BFA_Stack_Crawler prototype
    //It is essentially a single cycle of the search process. It will mark the current node as cleared, add any unchecked connections to the search list, and then assign the next node that will be check when this function is next called
    public void SearchCurrentNode(List<PathNode2> theSearchList, List<PathNode2> Cleared, ref PathNode2 currentNode, PathNode2 endNode)
    {
        PathGraph.Log($"Searching node {currentNode.gameObject.name}");
        Cleared.Add(currentNode); //Add the node we're about to search to the Cleared list. Because items get added to the end of a list, this will form the "stack" which will be used by the recursive trace

        theSearchList.Remove(currentNode);

        foreach (PathNode2 node in currentNode.connectedNodes)
        {
            PathGraph.Log($"SEARCH OF NODE {currentNode.gameObject.name}: Checking connected node of name {node.gameObject.name}...");

            //If the connected node is not in the cleared (aka visited) list and is not already in the search list, then add the node to the search list so that our script will know what options it has to search next.
            if (!Cleared.Contains(node) && !theSearchList.Contains(node))
            {
                PathGraph.Log($"node of name {node.gameObject.name} was added to the search list...");
                DrawSearchLine(currentNode.transform.position, node.transform.position);//Draw a line between the current node and the 
                theSearchList.Add(node);
            }
        }

        //VERY IMPORTANT: The next node to search will be chosen by this next line.
        //Because new items are added to the end of a standard c# list, this will choose the oldest available item to search next
        //This is how the "breadth first" search behaviour is implemented here. It looks like only 1 line, but changing this line will change how the search pattern behaves.
        currentNode = theSearchList[0];
    }
}
