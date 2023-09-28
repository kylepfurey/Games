using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentControllerNavmesh : NavAgentController
{
    public NavMeshAgent agent;

    public float timerlol;
    public float delay = .1f;
    public override void MoveToTarget()
    {
        if (Time.time > timerlol)
        {
            timerlol = Time.time + delay;
            //base.MoveToTarget();
            MonitorTarget();
            movementModule.SetTargetPosition(targetPosition);
        }

    }

    public override void MonitorTarget()
    {
        targetLOS = MonitorLOS(targetObjectTemp);
        //base.MonitorTarget();
    }
}
