using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPSGame;
using UnityEngine.Events;

public class BOSSHEALTHMONITOR : MonoBehaviour
{
    private Health theHealthLol;
    public BossPhaseEvent[] theEvents;

    private void Start()
    {
        theHealthLol = GetComponent<Health>();
    }

    private void Update()
    {
        foreach (BossPhaseEvent anEvent in theEvents)
        {
            if (anEvent.hasTriggered == false && theHealthLol.currentHealth/theHealthLol.maxHealth < anEvent.healthToTriggerAt)
            {
                anEvent.hasTriggered = true;
                anEvent.theEvent.Invoke();
            }
        }
    }

}
[System.Serializable]
public class BossPhaseEvent 
{
    public float healthToTriggerAt;
    public UnityEvent theEvent;
    public bool hasTriggered;
}
