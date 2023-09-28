using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossTargetMissiles : BossDroneState
{
    public float targetTimer;
    public float targetThreshold = 1f;
    public float successfulLaunchResetVal = -2f;
    public GameObject missilePrefab;
    public FinalAIState sightRecovered;

    public GameObject leftTrans;
    public GameObject rightTrans;

    public GameObject missileTargetPrefab;
    public GameObject missileVisualPrefab;

    public GameObject MissileLaunchVFX;

    public BossChargeMiniguns chargeRefOverrideLolSoDirty;

    public bool theFINLALSLS;

    //public 

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        //targetTimer = Time.time + targetDuration;
        if (targetTimer > targetThreshold)
        {
            targetTimer = 0f;
        }
        //FireAMissile(leftTrans);
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        targetTimer += Time.deltaTime;
        if (LOSCheckf() == false && theFINLALSLS == false)
        {
            myAI.SwitchToState(sightRecovered);
        }
        if (targetTimer > targetThreshold)
        {
            //fire the missiles
            targetTimer = successfulLaunchResetVal;
            FireAMissile(leftTrans);
            FireAMissile(rightTrans);
        }


        if (chargeRefOverrideLolSoDirty.chargeTimer < chargeRefOverrideLolSoDirty.chargeThreshold)
        {
            chargeRefOverrideLolSoDirty.chargeTimer += Time.deltaTime;
            chargeRefOverrideLolSoDirty.OpeningLerp();
        }
    }

    public void FireAMissile(GameObject leftOrRight)
    {
        TargetZoneScript targetZone = GameObject.Instantiate(missileTargetPrefab, coordinatorBossdrone.targetReference.transform.position, Quaternion.identity).GetComponent<TargetZoneScript>();
        MissileScript missileVisual = GameObject.Instantiate(missileVisualPrefab, transform.position, Quaternion.identity).GetComponent<MissileScript>();
        GameObject.Instantiate(MissileLaunchVFX, transform.position, transform.rotation);
        missileVisual.target = targetZone.gameObject;
        missileVisual.LeftOrRightObject = leftOrRight;
        missileVisual.speedMultiplier += missileVisual.speedMultiplier * Random.Range(0f, .1f);
    }
}
