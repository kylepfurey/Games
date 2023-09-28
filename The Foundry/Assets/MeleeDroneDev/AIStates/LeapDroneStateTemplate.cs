using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeapDroneStateTemplate : AIState
{
    protected DeathRoombaCoordinator coordinator;
    protected Animator animator;
    protected NavMeshAgent navMeshAgent;
    public string AnimatorState;
    public override void OnStateEnter(AIStateMachine myAI)
    {
        base.OnStateEnter(myAI);

        if (animator != null)
        {
            animator.CrossFade(AnimatorState, .25f);
        }
    }
    protected override void Start()
    {
        base.Start();
        coordinator = GetComponentInParent<DeathRoombaCoordinator>();
        animator = GetComponentInParent<Animator>();
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
    }
}
