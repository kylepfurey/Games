using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SIMPLE MOVEMENT MODULE
//This is the first movement module developed for the new system. It uses simple LookAt() and MoveTowards() functions to move the character towards the current target position
//While no longer in use, it is worth keeping for reference & example
//WRITTEN BY LIAM SHELTON
public class SimpleMovementModule : AgentMovementModule
{    
    protected override void MovementUpdateFunction()
    {
        base.MovementUpdateFunction();
        transform.LookAt(targetPositionCurrent);
        transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPositionCurrent, speed * Time.deltaTime);
    }
}
