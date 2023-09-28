using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NAV AGENT CONTROLLER
//This script is meant to serve as a pseudo-extension of the CustomNavAgent class
//It's primary function is to switch between moving directly to targets and using pathfinding, based on line of sight
//If the character has a direct line of sight to its destination, then it is best to move directly towards it. Otherwise, use the pathfinding algorithms until we have direct LOS.
//It also has functionality to avoid falling into holes when using direct navigation. NO LONGER NECESSARY
//WRITTEN BY LIAM SHELTON
public class NavAgentController : MonoBehaviour
{
    public CustomNavAgent m_Agent;
    public RoomTrackerModule roomTracker;
    protected AgentMovementModule movementModule;

    public Transform spherecastOrigin;
    public float spherecastRadius = 0.25f; //This controls how big the spherecast used to determine LOS will be. Increase this if agent tends to get stuck on corners


    public Vector3 targetPosition; //Semi-obsolete variable for tracking the agent's current target position. 

    public GameObject targetObjectTemp; //This system requires the use of a GameObject instead of a Vector3 position, to avoid issues relating to spherecasts being blocked by objects occupying the destination

    public bool targetLOS; //If this is true, then we probably have LOS
    public bool targetInRoom; //If this is true, then our target destination is inside our 'walkable' space. Part of the pitfall avoidance mechanism.

    protected bool keepTheFirst; //This is part of a somewhat dodgy mechanism to prevent agents from halting when updating paths during pursuit
    protected float pathRequestTimer;//Use this to put a leash on how frequently agents will make pathfinding requests
    protected float pathRequestDelay = 2f;//Use this to put a leash on how frequently agents will make pathfinding requests

    protected Vector3 lastPos; //Use this to determine wether or not we need to update our current path

    protected bool isStopped; //This is used to stop updates from causing movement when other scripts want us to stay in place

    protected void Awake()
    {
        m_Agent = GetComponent<CustomNavAgent>();
        roomTracker = GetComponent<RoomTrackerModule>();
        movementModule = GetComponent<AgentMovementModule>();

    }
    // Start is called before the first frame update
    void Start()
    {
        targetObjectTemp.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        targetPosition = targetObjectTemp.transform.position; //Possibly obsolete. Set the target position to the GameObject we're using as an endpoint for spherecasts

        
    }

    protected void FixedUpdate()
    {
        if (isStopped)
        {

        }
        else
        {
            MoveToTarget();
        }
    }

    //Call this from other scripts to make the agent hold its position
    public void Stop()
    {
        isStopped = true;
        movementModule.SetTargetPosition(transform.position); //Gotta do this to stop it from continuing to last node
    }

    //Call this from other scripts to make the agent continue moving towards its target
    public void Continue()
    {
        isStopped = false;
    }

    public virtual void MoveToTarget()
    {
        MonitorTarget();

        if (targetLOS)
        {
            //If we have clear line of sight then we can move straight towards them!
            movementModule.SetTargetPosition(targetPosition);
            keepTheFirst = true;
            if (m_Agent.currentPath.Count > 0)
            {
                m_Agent.currentPath.Clear();
            }
            pathRequestTimer = 0.25f;
        }
        else
        {
            //Watch the timer to prevent wasteful pathfinding calls
            if (pathRequestTimer < Time.time)
            {
                pathRequestTimer = Time.time + pathRequestDelay;
                if (targetPosition != lastPos) //Block pathfinding requests if navigation target position hasn't changed
                {
                    lastPos = targetPosition;
                    m_Agent.SetDestination(targetPosition);
                    if (keepTheFirst == true)
                    {
                        keepTheFirst = false;
                    }
                    else //I believe this check allows the agent to continue their momentum when a new path is generated
                    {
                        m_Agent.currentPath.RemoveAt(0);
                        m_Agent.currentNode = m_Agent.currentPath[0];
                    }
                }
            }

            //If we don't have direct LOS and have a path, then we should try to follow it.
            if (m_Agent.currentPath.Count > 0)
            {
                movementModule.SetTargetPosition(m_Agent.currentPath[0].transform.position);
            }
        }
    }

    //Call this in Update to keep our information about the current destination up-to-date
    public virtual void MonitorTarget()
    {
        targetLOS = MonitorLOS(targetObjectTemp);
        targetInRoom = MonitorRoomPresence();
    }

    //Call this to check if the navigation target is within our walkable area. Used to avoid pitfalls. Occasionally seems to break due to errors with bounds-checking
    public bool MonitorRoomPresence()
    {
        if (roomTracker.currentRoom.bounds.Contains(targetPosition))//spsps
        {
            //Debug.Log("Target is in our walkable area");
            return true;
        }
        else
        {
            //Debug.Log("Target NOT in our walkable area");
            return false;
        }
    }

    //Very important function. Call this to see if we have unobstructed line of sight to a GameObject
    //This function is very handy as a general-purpose visibility-checking function, so it gets used by quite a few other scripts. It seems very reliable.
    public bool MonitorLOS(GameObject targetObject)
    {

        //Perform a spherecast to our target
        RaycastHit hitInfo;
        Vector3 theDir = GetDirectionVector(targetObject);
        Debug.DrawRay(spherecastOrigin.position, theDir, Color.blue);
        bool spherecastHitSomething = Physics.SphereCast(spherecastOrigin.position, spherecastRadius, theDir, out hitInfo, theDir.magnitude, LayerMask.GetMask("Default", "Environmental"), QueryTriggerInteraction.Ignore);
        if (spherecastHitSomething)
        {
            //If spherecast hits something, then check if it's hit our target GameObject
            if (checkIfHitIsTarget(hitInfo.collider, targetObject) == true)
            {
                //if spherecast hits our target object, then we probably have LOS
                Debug.DrawLine(spherecastOrigin.position, hitInfo.point, Color.green);
                return true;
            }
            else
            {
                //if spherecast hit something other than our object, then we dont have LOS
                Debug.DrawLine(spherecastOrigin.position, hitInfo.point, Color.red);
                return false;
            }
        }
        else
        {
            //If our spherecast hit nothing at all, then it should mean we have a clear path to our target
            Debug.DrawLine(spherecastOrigin.position, targetObject.transform.position, Color.green);
            return true;
        }
    }

    //Short function for getting direction vector
    Vector3 GetDirectionVector(GameObject target)
    {
        return target.transform.position - spherecastOrigin.position;
    }

    //Scans a collider to see if it is our target object. Split-off from the MonitorLOS function for organization purposes
    //WARNING: May not work if collider-containing gameobject is a sibling of the target
    bool checkIfHitIsTarget(Collider collider, GameObject objectToCheck)
    {
        //first check if the collider is the target
        if (collider.gameObject == objectToCheck)
        {
            return true;
        }
        else
        {
            //next check if the collider is a child of our target object
            Transform[] transforms = collider.GetComponentsInParent<Transform>();
            for (int i = 0; i < transforms.Length; i++)
            {
                if (transform.gameObject == objectToCheck)
                {
                    return true;
                }
            }

            //if we make it here then the collider probably isn't part of our target
            return false;
        }
    }
}
