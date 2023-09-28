using GameFramework.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame
{
    public class FPSDroneBehavior : MonoBehaviour
    {
        public delegate void OnEnemyUpdatedDelegate(Component enemy);
        public OnEnemyUpdatedDelegate onEnemyUpdated;

        [SerializeField]
        private Actor owningActor;

        [SerializeField]
        private GameFramework.AI.AISight aiSight;

        [SerializeField]
        private Health health;

        [SerializeField]
        private float alertFriendlyRadius = 20.0f;

        [SerializeField]
        private LayerMask friendlyDroneLayer = -5;

        private Collider[] possibleFriendyColliders;

        private Component lastSeenEnemy = null;

        private void OnValidate()
        {
            if (!owningActor)
                owningActor = GetComponent<Actor>();

            if (!aiSight)
                aiSight = GetComponent<GameFramework.AI.AISight>();

            if (!health)
                health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            aiSight.onStimulusEvaluated += OnStimulusEvaluated;
            health.onTakeDamage += OnTakeDamage;
        }

        private void OnDisable()
        {
            aiSight.onStimulusEvaluated -= OnStimulusEvaluated;
            health.onTakeDamage -= OnTakeDamage;
        }

        private void Awake()
        {
            possibleFriendyColliders = new Collider[8];
        }

        private void OnStimulusEvaluated(IAISightStimulus stimulus, bool canBeSeen)
        {
            if (canBeSeen)
                SetEnemy(stimulus.transform, true);
            else
                SetEnemy(null);
        }

        private void OnTakeDamage(float damage, Actor instigator)
        {
            if (!instigator)
                return;

            if (owningActor == instigator || owningActor.Affiliation == instigator.Affiliation)
                return;

            SetEnemy(instigator.transform, true);
        }

        public void SetEnemy(Component enemy, bool shouldAlertFriendlies = false)
        {
            if (enemy != lastSeenEnemy)
            {
                onEnemyUpdated?.Invoke(enemy ? enemy.transform : null);

                lastSeenEnemy = enemy;

                if (enemy && shouldAlertFriendlies)
                    AlertFriendlies(enemy);
            }
        }

        /// <summary>
        /// Alerts friendlies about an enemy.
        /// </summary>
        public void AlertFriendlies(Component enemy)
        {
            if (!enemy)
                return;

            int colliderCount = Physics.OverlapSphereNonAlloc(transform.position, alertFriendlyRadius, possibleFriendyColliders, friendlyDroneLayer, QueryTriggerInteraction.Ignore);

            for (int i = 0; i < colliderCount; ++i) 
            {
                Actor otherActor = possibleFriendyColliders[i].GetComponentInParent<Actor>();
                if (!otherActor || otherActor == owningActor || otherActor.Affiliation != owningActor.Affiliation)
                    continue;

                FPSDroneBehavior droneBehavior = otherActor.GetComponent<FPSDroneBehavior>();
                if (droneBehavior)
                    droneBehavior.SetEnemy(enemy);
            }
        }
    }
}
