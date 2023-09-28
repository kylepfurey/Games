using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//AISTATE
//This is an abstract class for AI state machine states to inherit from.
//Ended up using MonoBehaviors for this one. Attach them to an empty GameObject parented to an object that has the AI State Machine script
//WRITTEN BY LIAM SHELTON
public abstract class AIState : MonoBehaviour
{
    protected NavAgentController navAgentController;
    protected virtual void Start()
    {
        navAgentController = GetComponentInParent<NavAgentController>();
    }
    public virtual void OnStateEnter(AIStateMachine myAI)
    {

    }

    public virtual void OnStateUpdate(AIStateMachine myAI)
    {

    }

    public virtual void OnStateExit(AIStateMachine myAI)
    {

    }
}
