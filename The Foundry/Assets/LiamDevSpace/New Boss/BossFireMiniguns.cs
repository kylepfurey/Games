using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireMiniguns : BossDroneState
{
    public float minigunDuration = 3f;
    private float minigunTimer;
    public FinalAIState nextState;


    public BossTargetMissiles targetMissiles;
    public bool MISSILESDIRTYLOLOL;

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        minigunTimer = Time.time + minigunDuration;
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();

        lookAtThePlayer();
        droneCombatA.fireCommand(coordinatorBossdrone.targetReference.transform);
        if (Time.time > minigunTimer)
        {
            myAI.SwitchToState(nextState);
        }


        if (MISSILESDIRTYLOLOL == true)
        {
            targetMissiles.OnStateUpdate();

        }
    }
}
