using UnityEngine;
using FPSGame;
using GameFramework.AI;
using System;

//DRONE COMBAT A
//Put this on the drones to let them use DroneWeaponModules 
public class DroneCombatA : MonoBehaviour
{
    [SerializeField]
    private FPSDroneBehavior droneBehavior;

    public GameObject target;
    public Transform turretTransform;

    public Health health;

    public bool hasLOS;

    public LayerMask layermask;

    private void OnValidate()
    {
        if (!droneBehavior)
            droneBehavior = GetComponent<FPSDroneBehavior>();

        if (!health)
            health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        droneBehavior.onEnemyUpdated += OnEnemyUpdated;
        health.onTakeDamage += OnTakeDamage;
    }

    private void OnDisable()
    {
        droneBehavior.onEnemyUpdated -= OnEnemyUpdated;
        health.onTakeDamage -= OnTakeDamage;
    }

    private void Update()
    {
        if (target != null)
        {
            turretTransform.LookAt(target.transform);
            hasLOS = (Physics.Linecast(turretTransform.position, target.transform.position, layermask, QueryTriggerInteraction.Ignore));

            if (Physics.Linecast(turretTransform.position, target.transform.position, layermask, QueryTriggerInteraction.Ignore) == false)
            {
                LineOfSightFunction();
            }
        }
    }

    private void OnEnemyUpdated(Component enemy)
    {
        if (!enemy)
        {
            target = null;
            return;
        }

        Actor actor = enemy.GetComponent<Actor>();
        if (!actor)
            target = enemy.transform.gameObject;
        else
            target = actor.TargetLookAtTransform.gameObject;
    }

    private void OnTakeDamage(float damage, Actor instigator)
    {
        if (!instigator)
            return;

        target = instigator.gameObject;
    }

    public delegate void FireCommand(Transform target);
    public FireCommand fireCommand;
    //This function should be treated as an update loop for the weapon modules
    public void LineOfSightFunction()
    {
        fireCommand(target.transform);
    }

    public void SetTarget(GameObject aTarget)
    {
        target = aTarget;
    }
}
