using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//DEATH ROOMBA STATE TEMPLATE
//This is an abstract class to allow DeathRoomba-specific functionality to be set up. It currently only assigns the coordinator script, but this is quite handy
//WRITTEN BY LIAM SHELTON
public abstract class DeathRoombaStateTemplate : AIState
{
    protected DeathRoombaCoordinator coordinator; //The coordinator stores DeathRoomba-specific references and functionalities, to minimize the number of GetComponent<> operations required.
    protected override void Start()
    {
        base.Start();
        coordinator = GetComponentInParent<DeathRoombaCoordinator>();
    }
}
