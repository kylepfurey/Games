using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapDroneCoordinator : DeathRoombaCoordinator
{
    protected override void Update()
    {
        //base.Update();
        //Some quick-n-dirty logic for smoothly aiming the turrets
        turretLerpTarget.position = Vector3.Lerp(turretLerpTarget.position, turretLookTarget.position, Time.deltaTime * lerpFactor);
        turretTransform.LookAt(turretLerpTarget);
    }
}
