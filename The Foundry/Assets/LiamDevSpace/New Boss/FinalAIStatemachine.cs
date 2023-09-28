using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalAIStatemachine : MonoBehaviour
{
    public FinalAIState currentState; //The state that the state machine is currently using
    public FinalAIState defaultState; //The state the machine defaults to upon start

    protected virtual void Start()
    {
        currentState = defaultState;
        currentState.OnStateEnter();
    }

    protected virtual void Update()
    {
        currentState.OnStateUpdate(); //run the current state machcine's update function!
    }

    //Call this from anything, especially AIStates, to change the current state.
    public virtual void SwitchToState(FinalAIState nextState)
    {
        currentState.OnStateExit();
        currentState = nextState;
        currentState.OnStateEnter();
    }
}
