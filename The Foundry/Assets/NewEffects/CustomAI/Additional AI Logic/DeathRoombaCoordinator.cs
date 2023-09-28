using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//DEATH ROOMBA COORDINATOR
//This is a location to dump references and functions that don't fit neatly into state machine logic. 
//WRITTEN BY LIAM SHELTON

public class DeathRoombaCoordinator : MonoBehaviour
{
    public Transform turretLookTarget;
    public Transform turretLerpTarget;
    public Transform turretTransform;
    public float lerpFactor = 10f;
    public Light MoodLight;
    public GameObject playerTarget;
    public Rigidbody rb;

    public GameObject baseModel;
    public float rotationMultiplier = 10f;

    protected virtual void Update()
    {

        //Some quick-n-dirty logic for smoothly aiming the turrets
        turretLerpTarget.position = Vector3.Lerp(turretLerpTarget.position, turretLookTarget.position, Time.deltaTime * lerpFactor);
        turretTransform.LookAt(turretLerpTarget);

        //A line to make the body rotate as the roomba moves around
        baseModel.transform.Rotate(Vector3.up * Time.deltaTime * rotationMultiplier * rb.velocity.magnitude);
    }

    //Call this when we want to make the turret aim at something, or if we want to make it face a direction
    public void SetLookPosition(Vector3 position)
    {
        turretLookTarget.position = position;
    }
}
