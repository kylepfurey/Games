using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapDroneWander : LeapDroneStateTemplate
{
    public AIState suspicionState;
    public GameObject post;
    public Vector3 origPos;
    public float wanderRadius;

    public float timerCurrent = .5f;
    public float timerMax = 3f;
    public float timerMin = 1f;

    public GameObject testObject;
    public GameObject playerObject;
    public bool theTestButton;
    public float speed;
    protected override void Start()
    {
        base.Start();
        origPos = transform.position;
    }
    public override void OnStateEnter(AIStateMachine myAI)
    {
        base.OnStateEnter(myAI);

        if (navMeshAgent != null)
        {
            navMeshAgent.speed = speed;
        }
    }
    public override void OnStateUpdate(AIStateMachine myAI)
    {
        base.OnStateUpdate(myAI);

        if (Time.time > timerCurrent)
        {
            TimerActivated();
        }



        //This block will activate the suspicion state if a player has been spotted.
        if (playerObject != null)
        {
            myAI.SwitchToState(suspicionState);
            coordinator.playerTarget = playerObject;
            playerObject = null;
        }
    }

    public void TimerActivated()
    {
        timerCurrent = Time.time + Random.Range(timerMax, timerMin);
        Vector3 thePos = Vector3.zero;
        if (post != null)
        {
            thePos = new Vector3(post.transform.position.x + Random.Range(-wanderRadius, wanderRadius), post.transform.position.y, post.transform.position.z + Random.Range(-wanderRadius, wanderRadius));
        }
        else
        {
            thePos = new Vector3(origPos.x + Random.Range(-wanderRadius, wanderRadius), origPos.y, origPos.z + Random.Range(-wanderRadius, wanderRadius));

        }
        //navMeshAgent.SetDestination(thePos);
        navAgentController.targetObjectTemp.transform.position = thePos;
        //navAgentController.targetPosition = thePos;
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
}
