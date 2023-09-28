using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWait : BossDroneState
{
    public FinalAIState nextState;
    public bool triggerStopper = false;
    public GameObject shieldObject;
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        triggerStopper = false;
    }
    public string layername = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == layername && triggerStopper == false)
        {
            coordinatorBossdrone.targetReference = other.gameObject;
        }
    }

    public void StartTheFight()
    {
        triggerStopper = true;
        Debug.Log("NOMOREWAIT");
        myAI.SwitchToState(nextState);
        shieldObject.SetActive(false);
        //coordinatorBossdrone.targetReference = other.gameObject;
    }

    public override void Start()
    {
        base.Start();
        shieldObject.transform.parent = null; //unparent the shield to prevent it from acting as a hurtbox
    }
}
