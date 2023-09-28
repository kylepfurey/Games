using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//PATHNODE 2
//This is similar to the original PathNode class, except this one is meant to serve as both Node and Pathnode simultaneously
//The script that draws the lines between connected nodes is here instead of the PathGraph. Seemed like a reasonable place considering the connections are stored in the nodes.
//WRITTEN BY LIAM SHELTON
public class PathNode2 : MonoBehaviour
{
    public List<PathNode2> connectedNodes = new List<PathNode2>();
    public float weight = 0f;
    public float dijkstraDistance = Mathf.Infinity; //this variable is used for distance by Djikstra's algorithm. Make sure it gets initialized to mathf.Infinity when such algorthms begin.

    public PathGraph myGraph;
    public MeshRenderer nodeVisual; //This is the sphere mesh used to make the nodes visible for debugging purposes. I used this instead of Gizmos because one cannot click on a gizmo sphere to select the object in the editor. Also looks nicer
    public Color graphColor;

    // Make sure this happens after the PathGraph awakes in the Script Execution order
    protected virtual void Awake()
    {
        JoinParentGraph();
    }

    void Update()
    {
        DebugFunction(myGraph.debugMode);
    }

    //Method to draw lines between connected nodes. I ended up using Debug.DrawLine instead of Gizmos as they're easier to use and serve the same purpose
    void DebugFunction(bool debugMode)
    {
        nodeVisual.enabled=debugMode; //Toggle debug mesh renderer. 
        //draw lines
        if (debugMode == true)
        {
            foreach (PathNode2 node in connectedNodes)
            {
                Debug.DrawLine(gameObject.transform.position, node.transform.position, graphColor, Time.deltaTime);
            }
        }
    }

    //This function will attempt to get the graph object it's parented to, and then call the AddNode() function
    private void JoinParentGraph()
    {
        myGraph = PathGraph.pathGraph;
        myGraph.AddNode(this);
    }
}
