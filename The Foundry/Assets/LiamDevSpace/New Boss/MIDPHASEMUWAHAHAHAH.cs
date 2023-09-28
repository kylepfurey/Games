using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPSGame;

public class MIDPHASEMUWAHAHAHAH : BossDroneState
{
    public BossPhasesLOL phasesLOL;
    public GameObject theShield;
    public Health theHealth;


    public BossChargeMiniguns lsllsdk;
    public BossFireMiniguns cwhbrc;
    public BossTargetMissiles ddddd;
    public FinalAIState theNextState;

    public override void Start()
    {
        base.Start();
        theHealth = GetComponentInParent<Health>();
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        phasesLOL = GetComponentInParent<BossPhasesLOL>();
        theShield.SetActive(true);
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if (phasesLOL.spawnedObjectsLOL.Count == 0)
        {
            Debug.Log("NOMOREADDSLOLOLOL");
            myAI.SwitchToState(theNextState);
            lsllsdk.MISSILESDIRTYLOLOL = true;
            cwhbrc.MISSILESDIRTYLOLOL = true;
            ddddd.theFINLALSLS = true;
        }
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        theShield.SetActive(false);
    }
}
