using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//AI CHARACTER LOGIC
//This is the script that actually implements the custom nav agent script, same as one would when working with the builtin NavMeshAgent
//WRITTEN BY LIAM SHELTON
public class AI_CharacterLogicReal : MonoBehaviour
{
    public AlgorithmType algorithm;
    public CustomNavAgent agent;

    public void setDestinationThingy(Vector3 thePosition)
    {
        agent.SetDestination(thePosition, algorithm);
    }
}
