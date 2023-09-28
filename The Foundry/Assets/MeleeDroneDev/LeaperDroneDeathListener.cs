using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPSGame;
using UnityEngine.AI;

//This is a quick-and-dirty script to handle the two things not covered by DroneDeathLogic
public class LeaperDroneDeathListener : MonoBehaviour
{
    Health droneHealth;
    // Start is called before the first frame update
    void Start()
    {
        droneHealth = GetComponent<Health>();
        droneHealth.onDead += OnDeath;
    }
    public void OnDeath()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;

        AIStateMachine stateMachine = GetComponent<AIStateMachine>();
        stateMachine.SwitchToState(GetComponentInChildren<LeapDroneDead>());
    }
}
