using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class LeapDroneLeapAttack : LeapDroneStateTemplate
{
    public float forwardVelocity;
    public float upwardVelocity;
    public GameObject emitterObject;

    private int layerMask;
    public int layer = 13;

    public AIState landingState;

    public float delay = .1f;
    public float timer;

    public MeleeHitbox myHitbox;
    public float damageToInflict = 20f;
    public bool hitboxdelaybool;//Using this to give a brief delay before the hitbox activation

    public GameObject attackSoundEffect;

    public override void OnStateEnter(AIStateMachine myAI)
    {
        base.OnStateEnter(myAI);

        navMeshAgent.enabled = false;
        coordinator.rb.isKinematic = false;

        //leaptargeting
        Vector3 theLeapTarg = coordinator.playerTarget.transform.position;
        theLeapTarg.y = myAI.gameObject.transform.position.y; ;
        myAI.gameObject.transform.LookAt(theLeapTarg);

        //physics stuff
        coordinator.rb.velocity = (myAI.gameObject.transform.forward * forwardVelocity) + (Vector3.up * upwardVelocity);
        navAgentController.Stop();
        layerMask = 1 << layer;

        //start a timer to prevent tripping
        timer = Time.time + delay;

        //reset the delay bool
        hitboxdelaybool = false;

        Instantiate(attackSoundEffect, transform.position, transform.rotation, null);
    }
    public override void OnStateUpdate(AIStateMachine myAI)
    {
        base.OnStateUpdate(myAI);

        if (Time.time > timer)
        {
            AirScan(myAI);

            if (hitboxdelaybool == false)
            {
                hitboxdelaybool = true;
                //activate the hitbox
                myHitbox = myAI.GetComponentInChildren<MeleeHitbox>();
                myHitbox.ActivateHitbox(damageToInflict);
            }
        }
        RaycastHit hit;
    }
    public override void OnStateExit(AIStateMachine myAI)
    {
        base.OnStateExit(myAI);
        if (myHitbox != null)
        {
            myHitbox.DeactivateHitbox();
        }
        //navMeshAgent.enabled = true;
        //coordinator.rb.isKinematic = true;
    }

    float scanDistance = 5f;
    public void AirScan(AIStateMachine myAI)
    {
        //int layerMask = ~0;
        RaycastHit hit;
        Ray theRay = new Ray(gameObject.transform.position + Vector3.up, Vector3.down);
        if (Physics.Raycast(theRay.origin, theRay.direction, out hit, 1.1f, layerMask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(theRay.origin, hit.point, Color.green, Time.deltaTime);
            myAI.SwitchToState(landingState);

        }
        else
        {
            Debug.DrawRay(theRay.origin, theRay.direction * 1.1f, Color.red, Time.deltaTime);
        }
    }
}
