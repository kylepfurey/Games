using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPSGame;
//AI STATE MACHINE
//This is a finite state machine implemented for the final.
//WRITTEN BY LIAM SHELTON
public class AIStateMachine : MonoBehaviour
{
    public AIState currentState; //The state that the state machine is currently using
    public AIState defaultState; //The state the machine defaults to upon start

    protected virtual void Start()
    {
        currentState = defaultState;
        currentState.OnStateEnter(this);
    }

    protected virtual void Update()
    {
        currentState.OnStateUpdate(this); //run the current state machcine's update function!
    }

    //Call this from anything, especially AIStates, to change the current state.
    public virtual void SwitchToState(AIState nextState)
    {
        currentState.OnStateExit(this);
        currentState = nextState;
        currentState.OnStateEnter(this);
    }
}
