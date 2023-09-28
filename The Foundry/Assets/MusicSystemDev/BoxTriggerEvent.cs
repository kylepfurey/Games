using FPSGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class BoxTriggerEvent : MonoBehaviour
{
    public UnityEvent boxTriggeredEvent;
    public UnityEvent boxExitEvent;
    public bool deleteOnEntry = false;
    public string layername;


    private void OnTriggerEnter(Collider collider)
    {
        if (LayerMask.LayerToName(collider.gameObject.layer) == layername)
        {
            boxTriggeredEvent.Invoke();
            if (deleteOnEntry)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (LayerMask.LayerToName(collider.gameObject.layer) == layername)
        {
            boxExitEvent.Invoke();
        }
    }
}
