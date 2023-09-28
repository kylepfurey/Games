using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class BossChargeMiniguns : BossDroneState
{
    public float chargeTime = 3f;
    public float chargeTimer;
    public float chargeThreshold = 3f;
    public FinalAIState fireState;
    public FinalAIState MissileState;
    public AudioSource audioSource;
    public float speedMultiplier = 1f;
    public bool hasEngagedFiring;

    public BossTargetMissiles targetMissiles;
    public bool MISSILESDIRTYLOLOL;

    //public float blindTimer;
    //public float 

    //public 
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        //chargeTimer = Time.time + chargeTime;
        
        if (hasEngagedFiring)
        {
            hasEngagedFiring = false;
            scaleVal = 0f;
            chargeTimer = 0f;
        }
        
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        lookAtThePlayer();
        OpeningLerp();

        chargeTimer += Time.deltaTime;
        if (chargeTimer >= chargeThreshold)//(chargeTimer < Time.time)
        {
            Debug.Log("READYTOFIRE");
            myAI.SwitchToState(fireState);
            hasEngagedFiring = true;
        }
        if (LOSCheckf() == true && MISSILESDIRTYLOLOL == false)
        {
            myAI.SwitchToState(MissileState);
        }
        Debug.Log($"LOS: {LOSCheckf()}");

        if (MISSILESDIRTYLOLOL == true)
        {
            targetMissiles.OnStateUpdate();
            
        }
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
    }



    private float scaleVal;
    public void OpeningLerp()
    {
        if (scaleVal < 1)
        {
            scaleVal += Time.deltaTime * speedMultiplier;
            audioSource.pitch = scaleVal;
        }
    }
}
