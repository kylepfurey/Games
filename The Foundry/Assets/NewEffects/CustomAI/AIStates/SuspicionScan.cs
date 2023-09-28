using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//SUSPICION SCAN
//This is an AI state in which a Roomba will attempt to identify a player that it can currently see. It should stop and look at the player. Progress indicated by the color of its spotlight 
//If the player remains in line of sight for a specified time, then the roomba will enter combat state.
//Will switch to search modde if player leaves LOS.
//It also has a brief time buffer before switching to search, during which it will hold position.
//WRITTEN BY LIAM SHELTON
public class SuspicionScan : DeathRoombaStateTemplate
{
    public float defaultTimeUntilHostile = 2f;

    public float hostileTimer;

    public AIState HostileStateAttacking;
    public SuspicionFollow SuspicionStateFollow;

    private Color colorlow = Color.yellow;
    private Color colorhigh = Color.red;

    public bool followWaitStarter = true;
    public float followWaitTimer;
    public float followWaitDelay = 1.5f;

    protected override void Start()
    {
        base.Start();
        hostileTimer = defaultTimeUntilHostile;
    }
    public override void OnStateEnter(AIStateMachine myAI)
    {
        base.OnStateEnter(myAI);
        navAgentController.Stop();
    }

    public override void OnStateUpdate(AIStateMachine myAI)
    {
        base.OnStateUpdate(myAI);

        if (navAgentController.MonitorLOS(coordinator.playerTarget))
        {
            followWaitStarter = true;
            coordinator.SetLookPosition(coordinator.playerTarget.transform.position);

            coordinator.MoodLight.color = Color.Lerp(colorhigh, colorlow, hostileTimer / defaultTimeUntilHostile);

            hostileTimer = hostileTimer - Time.deltaTime;
            if (hostileTimer < 0)
            {
                myAI.SwitchToState(HostileStateAttacking);
            }
        }
        else
        {
            if (followWaitStarter == true)
            {
                followWaitStarter = false;
                followWaitTimer = Time.time + followWaitDelay;
                navAgentController.targetObjectTemp.transform.position = coordinator.playerTarget.transform.position;


            }
            if (followWaitTimer < Time.time)
            {
                myAI.SwitchToState(SuspicionStateFollow);
            }
        }

    }
}
