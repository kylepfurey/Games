using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeapDroneMovementModule : AgentMovementModule
{
    public NavAgentController navController;
    public NavMeshAgent agent;


    public float timerlol;
    public float delay = .1f;
    protected override void MovementUpdateFunction()
    {
        base.MovementUpdateFunction();
        if (navController == null)
        {
            navController = GetComponent<NavAgentController>();
        }
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if(agent.enabled == true)
        {
            if (Time.time > timerlol)
            {
                timerlol = Time.time + delay;
                agent.SetDestination(targetPositionCurrent);

            }
        }
    }
}
