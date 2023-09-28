using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//AGENT MOVEMENT MODULE
//This is an abstract class to define how specific kinds of AI-controlled characters can move.
//The NavAgentController script expects one of these to be attached to its gameObject, and it will attempt to utilize it by setting its target position
//WRITTEN BY LIAM SHELTON
public abstract class AgentMovementModule : MonoBehaviour
{
    public float speed = 1f;

    [SerializeField]
    protected Vector3 targetPositionCurrent;

    protected virtual void Start()
    {
        targetPositionCurrent = gameObject.transform.position;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        MovementUpdateFunction();
    }

    protected virtual void MovementUpdateFunction()
    {

    }

    public virtual void SetTargetPosition(Vector3 thePos)
    {
        targetPositionCurrent = thePos;
    }
}
