using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//BALL DRONE MOVEMENT MODULE
//This is the AgentMovementModule that the 'Death Roomba' characters use. It is intended for physics-driven characters that have their rigidbody rotations locked in all axis (the roomba's perceived rotations are simply their models rotating locally)
//This movement currently requires the navigation nodes to be flush with the ground floor, but also with the final destination being elevated (spherecast quirks)
//WRITTEN BY LIAM SHELTON
public class BalldroneMovementModule : AgentMovementModule
{
    public NavAgentController navController;
    public ConstantForce theForce;

    //This Start() function previously helped solve a mysterious friction problem, but after some time friction returned to normal. Leaving it here in case problem arises again
    //private void Start()
    //{
        //Collider[] sslsl = GetComponentsInChildren<Collider>();
        //for (int i = 0; i < sslsl.Length; i++)
        //{
        //    sslsl[i].material.frictionCombine = 0;
        //}
    //}

    protected override void MovementUpdateFunction()
    {
        base.MovementUpdateFunction();

        if (navController == null)
        {
            navController = GetComponent<NavAgentController>();
        }

        //This block is meant to ground waypoints, so that waypoints off the ground can't cause flight. However, a fatal flaw in the CustomNavAgent script prevents this from being usable at this time, so make sure pathnodes are flush with ground
        Vector3 targetPositionGrounded = targetPositionCurrent;
        if (navController.targetLOS && navController.targetInRoom)
        {
            RaycastHit hitInfo;
            bool raycastHit = Physics.Raycast(targetPositionCurrent, Vector3.down, out hitInfo, 5f, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore);
            if (raycastHit)
            {
                targetPositionGrounded = hitInfo.point;
            }
            else
            {
                //Commenting this error out for now. Doesn't cause navigation failure if module is used correctly, but could be worth reinstating in the future if further attempts at raycast-based grounding are desired.
                //Debug.LogError("GROUNDED ENEMY FAILED TO GET GROUNDED POSITION");
            }
        }

        //This block controls the constantforce to move the character towards its current waypoint.
        Vector3 theDir = targetPositionGrounded - transform.position;
        theDir = theDir.normalized;
        theDir = theDir * speed;
        theForce.force = theDir;
    }
}
