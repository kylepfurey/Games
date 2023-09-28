using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CUSTOM NAV AGENT
//This is the script that is meant to be the equivalent of a builtin NavMeshAgent for my custom pathfinding system
//Has simple methods for moving through PathNode2s lists provided by the PathGraph
//WRITTEN BY LIAM SHELTON
public class CustomNavAgent : MonoBehaviour
{
    public float speed = 1f; //Speed of the agent. currently linear
    public float deadZone = .25f;

    public bool debugMode = true; //turn this on if you want the agent to draw a line to its current desired node

    public PathNode2 currentNode; //The node this agent is currently moving towards
    public List<PathNode2> currentPath = new List<PathNode2>(); //the list of nodes this agent is working through. It'll start with the lowest index and work its way through till there are none left

    public PathGraph currentGraph; //only purpose of this is to snap the agent to a nearby node when the game starts

    private void Awake()
    {
        currentGraph = PathGraph.pathGraph;
    }

    void Start()
    {
        if (currentNode == null)
        {
            SnapToNearestNode();
        }
    }

    //Does what it says. Snaps the agent to the nearest node.
    void SnapToNearestNode()
    {
        PathNode2 theNodeToSnapTo = null;
        float lowestDist = Mathf.Infinity;

        if (currentGraph != null)
        {
            if (currentGraph.nodes != null)
            {
                if (currentGraph.nodes.Count != 0)
                {
                    foreach (PathNode2 pathNode in currentGraph.nodes)
                    {
                        float theDist = Vector3.Distance(gameObject.transform.position, pathNode.gameObject.transform.position);
                        if (theDist < lowestDist)
                        {
                            lowestDist = theDist;
                            theNodeToSnapTo = pathNode;
                        }
                    }

                    if (theNodeToSnapTo != null)
                    {
                        //transform.position = theNodeToSnapTo.transform.position;
                        currentNode = theNodeToSnapTo;
                    }
                    else
                    {
                        Debug.LogError("Custom Nav Agent failed to snap to the nearest node");
                    }
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        //MoveFunction();
        NavFunction1();
        DebugVisuals();
    }

    //This is the method that is supposed to be called by other scripts, similarly to unity's builtin NavMeshAgent.SetDestination().

    //public delegate void SetDestinationCallback(Vector3 aPosition);
    //public SetDestinationCallback setDestinationCallback;
    public void SetDestination(Vector3 aPosition)
    {
        Debug.Log("NAVAGENT SETTING DESTINATION...");
        SnapToNearestNode();

        if (currentNode != null)
        {
            currentPath = PathGraph.pathGraph.FindPath(currentNode, aPosition, AlgorithmType.AStar);
        }
    }
    //This is an overload that allows scripts to specifiy the algorithm being used.
    public void SetDestination(Vector3 aPosition, AlgorithmType type)
    {
        currentPath = PathGraph.pathGraph.FindPath(currentNode, aPosition, type);

    }

    //Call this from update. Primarily here to keep the update section clean
    void DebugVisuals()
    {
        if (debugMode == true)
        {
            Debug.DrawLine(gameObject.transform.position + (Vector3.up * .5f), currentNode.transform.position);
        }
    }

    //A simple update method to make the agent move to its current waypoint. It should face the next directly, and move at a linear speed.
    //If acceleration and other such things are required in the future, this is where they'd be set up.
    void MoveFunction()
    {
        if (currentNode != null)
        {
            transform.LookAt(currentNode.transform.position);
            transform.position = Vector3.MoveTowards(gameObject.transform.position, currentNode.transform.position, speed * Time.deltaTime);
        }

    }

    //This is the update method that will iterate through the current path list until there are no nodes remaining.
    void NavFunction1()
    {
        if (currentPath != null)
        {
            if (currentPath.Count > 0)
            {
                currentNode = currentPath[0];
                if (Vector3.Distance(gameObject.transform.position, currentNode.gameObject.transform.position) < deadZone)
                {
                    PopCurrentNode();
                }
            }
        }
    }

    public void PopCurrentNode()
    {
        if (currentPath.Count > 0)
        {
            currentPath.Remove(currentNode);

        }

    }
}

//ALGORITM TYPE
//This probably should be in its own C# file. It provides an enum that all scripts can work with for distinguishing between algorithm logics.
//WRITTEN BY LIAM SHELTON
public enum AlgorithmType
{
    BreadthFirstSearch,
    Heuristic,
    Dijkstras,
    AStar,
}