using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetZoneSensor : MonoBehaviour
{
    public string layername = "Player";
    private TargetZoneScript myZoneThingy;
    private void Start()
    {
        myZoneThingy = GetComponentInParent<TargetZoneScript>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == layername)
        {
            Debug.Log($"ISLAYER ");

        }
        else
        {
            Debug.Log($"NOTLAYER ");

        }
    }
}
