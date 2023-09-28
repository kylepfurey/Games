using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDroneState : FinalAIState
{
    //Yes, I know this is exactly what I was trying to avoid with the other state machine. However, I just need to get it done. Its the last one.
    protected CoordinatorBossdrone coordinatorBossdrone;
    protected FinalAIStatemachine myAI;
    protected DroneCombatA droneCombatA;

    public virtual void Start()
    {
        coordinatorBossdrone = GetComponentInParent<CoordinatorBossdrone>();
        myAI = GetComponentInParent<FinalAIStatemachine>();
        droneCombatA = GetComponentInParent<DroneCombatA>();
        //droneCombatA.
    }


    public void lookAtThePlayer()
    {
        coordinatorBossdrone.Turret.transform.LookAt(coordinatorBossdrone.targetReference.transform.position + Vector3.up*.5f);
    }

    protected bool hasLOS;

    public LayerMask layermask;
    public void LOSCheck()
    {
        hasLOS = (Physics.Linecast(coordinatorBossdrone.Turret.transform.position, coordinatorBossdrone.targetReference.transform.position, layermask, QueryTriggerInteraction.Ignore));

        //if (Physics.Linecast(turretTransform.position, target.transform.position, layermask, QueryTriggerInteraction.Ignore) == false)
        //{
        //    LineOfSightFunction();
        //}
    }
    public bool LOSCheckf()
    {
        return (Physics.Linecast(coordinatorBossdrone.Turret.transform.position, coordinatorBossdrone.targetReference.transform.position + Vector3.up * 0.5f, layermask, QueryTriggerInteraction.Ignore));

    }
}
