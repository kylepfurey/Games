using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//PURSUING TARGET
//This AI state allows death roombas to chase their targets if line of sight is lost during combat.
//It is largely copied over from the suspicion scan state, with a key difference being that this state is informed of the player's exact location on every update
//WRITTEN BY LIAM SHELTON
public class PursuingTarget : DeathRoombaStateTemplate
{
    //Timer vars allow the roomba to stop pursuit if player successfully avoids contact for extended period of time
    public float defaultWaitTime = 10f;
    public float waitTimer;

    public SuspicionFollow SuspicionFollowState;
    public AIState AttackState;
    public override void OnStateEnter(AIStateMachine myAI)
    {
        base.OnStateEnter(myAI);
        navAgentController.Continue();
        waitTimer = Time.time + defaultWaitTime;
        navAgentController.targetObjectTemp.transform.position = coordinator.playerTarget.transform.position;
    }

    public override void OnStateUpdate(AIStateMachine myAI)
    {
        base.OnStateUpdate(myAI);
        navAgentController.targetObjectTemp.transform.position = coordinator.playerTarget.transform.position;
        coordinator.SetLookPosition(coordinator.playerTarget.transform.position);
        if (navAgentController.MonitorLOS(coordinator.playerTarget) == false)
        {
            if (waitTimer < Time.time)
            {
                myAI.SwitchToState(SuspicionFollowState);

                //It's slightly dirty to call this here, but for the sake of time I'll let it be. It's specific to Roombas. 
                SuspicionFollowState.suspicionScanState.hostileTimer = SuspicionFollowState.suspicionScanState.defaultTimeUntilHostile;
                coordinator.MoodLight.color = Color.yellow;
            }
        }
        else
        {
            myAI.SwitchToState(AttackState);
        }
    }

    public override void OnStateExit(AIStateMachine myAI)
    {
        base.OnStateExit(myAI);
    }
}
