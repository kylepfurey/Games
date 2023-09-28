using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapDroneAggro : LeapDroneStateTemplate
{
    public AIState nextState;

    public float timer;
    public float delay = 2f;
    bool theCatch;
    public override void OnStateEnter(AIStateMachine myAI)
    {
        base.OnStateEnter(myAI);
        timer = Time.time + delay;
        myAI.gameObject.transform.LookAt(coordinator.playerTarget.transform.position);
        navAgentController.Stop();
    }
    public override void OnStateExit(AIStateMachine myAI)
    {
        base.OnStateExit(myAI);
        theCatch = false; ;
    }
    public override void OnStateUpdate(AIStateMachine myAI)
    {
        base.OnStateUpdate(myAI);
        if (theCatch == false)
        {
            theCatch = true;
            timer = Time.time + delay;

        }
        if (Time.time > timer)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                Debug.Log($"Normalized time: {animator.GetCurrentAnimatorStateInfo(0).normalizedTime}");
                myAI.SwitchToState(nextState);
            }
        }
    }
}
