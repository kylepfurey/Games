using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeapDroneAI : MonoBehaviour
{
    public NavMeshAgent m_agent;

    public enum leapDroneState
    {
        wandering,
        aggroing,
        chasing,
        jumpsquat,
        airborne,
        recovering

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
