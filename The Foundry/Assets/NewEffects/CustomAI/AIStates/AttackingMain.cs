using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPSGame;
//ATTACKING (MAIN)
//This is a state that allows the DeathRoombas to shoot at their targets, as well as begin pursuit when line of sight is lost
//Note that it somewhat haphazardly puppeteers the DroneCombatA script. It works by calling its callback function, which should cause weapon modules to fire if they're able to.
//It is intended for the DroneCombatA script to be disabled when this script starts. Make sure the weapon modules are enabled, however.
//WRITTEN BY LIAM SHELTON
public class AttackingMain : DeathRoombaStateTemplate
{
    public DroneCombatA myCombatA;

    public AIState pursueTargetState;
    protected override void Start()
    {
        base.Start();
        myCombatA = GetComponentInParent<DroneCombatA>();
    }
    public override void OnStateUpdate(AIStateMachine myAI)
    {
        base.OnStateUpdate(myAI);

        coordinator.SetLookPosition(coordinator.playerTarget.transform.position);
        if (navAgentController.MonitorLOS(coordinator.playerTarget))
        {
            myCombatA.fireCommand(coordinator.playerTarget.transform);
        }
        else
        {
            myAI.SwitchToState(pursueTargetState);
        }
    }
}
