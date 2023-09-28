using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//DEATH ROOMBA PATROL
//This is an AI state for patrolling the environment.
//This state machine is meant to have a trigger attached to use for detecting enemies
//WRITTEN BY LIAM SHELTON
public class DRPatrol : DeathRoombaStateTemplate
{
    public AIState suspicionState;

    public List<GameObject> wayPointCycle = new List<GameObject>(); //Put the desired patrol destinations here
    public int currentWaypointIndex;

    public GameObject playerObject;

    public Rigidbody rb;
    public float speedLookThreshhold = 2f;
    protected override void Start()
    {
        base.Start();
        rb = GetComponentInParent<Rigidbody>();
    }

    //While patrolling, this state will check for the player. If it sees them, then it will que the object for processing in OnStateUpdate()
    private void OnTriggerStay(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
        {
            if (navAgentController.MonitorLOS(other.gameObject) == true)
            {
                //que the player as a target!
                playerObject = other.gameObject;
            }
        }
    }
    public override void OnStateEnter(AIStateMachine myAI)
    {
        if (myAI != null)
        {
            base.OnStateEnter(myAI);

            if (navAgentController != null && navAgentController.targetObjectTemp != null && wayPointCycle[currentWaypointIndex] != null)
            {
                navAgentController.targetObjectTemp.transform.position = wayPointCycle[currentWaypointIndex].transform.position;
            }
        }
    }

    public override void OnStateUpdate(AIStateMachine myAI)
    {
        base.OnStateUpdate(myAI);

        //This block cycles through waypoints
        if (Vector3.Distance(navAgentController.transform.position, wayPointCycle[currentWaypointIndex].transform.position) < 1f)
        {
            if (currentWaypointIndex < wayPointCycle.Count - 1)
            {
                currentWaypointIndex++;
            }
            else
            {
                currentWaypointIndex = 0;
            }
        }
        navAgentController.targetObjectTemp.transform.position = wayPointCycle[currentWaypointIndex].transform.position;


        //This block makes the turret face the direction of movement, primarily for cosmetic purposes. Otherwise, the turret will stay completely still until provked
        if (rb.velocity.magnitude > speedLookThreshhold)
        {
            Vector3 lookPos = transform.position + rb.velocity;
            lookPos.y = coordinator.turretTransform.position.y;
            coordinator.turretLookTarget.position = lookPos;
        }

        //This block will activate the suspicion state if a player has been spotted.
        if (playerObject != null)
        {
            myAI.SwitchToState(suspicionState);
            coordinator.playerTarget = playerObject;
            playerObject = null;
        }
    }
}
