using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapDronePursuit : LeapDroneStateTemplate
{
    public float speed;
    public float attackDistance = 1f;
    public AIState leapState;

    public override void OnStateEnter(AIStateMachine myAI)
    {
        base.OnStateEnter(myAI);
        navMeshAgent.speed = speed;
        coordinator.rb.isKinematic = true;
        navMeshAgent.enabled = true;
    }
    public override void OnStateUpdate(AIStateMachine myAI)
    {
        base.OnStateUpdate(myAI);
        //navMeshAgent.SetDestination(coordinator.playerTarget.transform.position);
        navAgentController.targetObjectTemp.transform.position = coordinator.playerTarget.transform.position;
        navAgentController.Continue();

        if (Vector3.Distance(myAI.gameObject.transform.position, coordinator.playerTarget.transform.position) < attackDistance)
        {
            myAI.SwitchToState(leapState);
            
        }
    }
}
