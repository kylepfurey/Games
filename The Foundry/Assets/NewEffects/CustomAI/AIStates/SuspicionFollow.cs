using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//SUSPICION FOLLOW
//This AI State will make roombas "search" for the player if they have been spotted and break LOS before identification.
//It will go to the player's last seen location and wait until timer expires. If timer expires, roomba should resume patrol.
//If it manages to re-locate the player, the roomba will switch back to the scan state.
//WRITTEN BY LIAM SHELTON
public class SuspicionFollow : DeathRoombaStateTemplate
{
    public float defaultWaitTime = 5f;
    public float waitTimer;

    public AIState patrolState;
    public SuspicionScan suspicionScanState;
    public override void OnStateEnter(AIStateMachine myAI)
    {
        base.OnStateEnter(myAI);
        navAgentController.Continue();
        waitTimer = Time.time + defaultWaitTime;
    }

    public override void OnStateUpdate(AIStateMachine myAI)
    {
        base.OnStateUpdate(myAI);

        if (navAgentController.MonitorLOS(coordinator.playerTarget) == false)
        {
            if (waitTimer < Time.time)
            {
                myAI.SwitchToState(patrolState);
                suspicionScanState.hostileTimer = suspicionScanState.defaultTimeUntilHostile;
                coordinator.MoodLight.color = Color.yellow;
            }
        }
        else
        {
            myAI.SwitchToState(suspicionScanState);
        }
    }
}
