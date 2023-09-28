using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPSGame
{
    public class AISight : MonoBehaviour
    {
        [SerializeField]
        private Actor selfActor;

        [SerializeField]
        private Transform eyeTransform;

        [SerializeField]
        private float detectionRange = 10.0f;

        [SerializeField]
        private float attackRange = 8.0f;

        [Tooltip("The amount of time the NPC attempts to find the target before it gives up.")]
        [SerializeField]
        private float searchDuration = 10.0f;

        [SerializeField]
        private List<Collider> ignoredColliders;

        private GameObject lastSeenTarget = null;

        [SerializeField]
        private LayerMask layers = -1;

        bool canSeeTarget = false;
        GameObject seenTarget;
        bool isTargetInAttackRange = false;
        float lastSeenTargetTime;

        public UnityAction<GameObject> onTargetDetected;
        public UnityAction<GameObject> onTargetLost;
        public UnityAction<GameObject> onTargetForgotten;
        public UnityAction<GameObject> onTargetInAttackRange;

        private void OnValidate()
        {
            if (!selfActor)
            {
                selfActor = GetComponent<Actor>();
            }

            if (!eyeTransform)
            {
                eyeTransform = transform;
            }
        }

        public void ScanEnvironment()
        {
            if (seenTarget && !canSeeTarget && (Time.time - lastSeenTargetTime) >= searchDuration)
            {
                onTargetForgotten?.Invoke(seenTarget);
                seenTarget = null;
                Debug.Log($"{name}: Target forgotten");
            }

            float sightRadiusSquared = detectionRange * detectionRange;
            float closestDistanceSquared = float.PositiveInfinity;
            canSeeTarget = false;
            GameObject latestSeenTarget = null;
            
            foreach (Actor actor in ActorManager.Instance.Actors)
            {
                if (actor == selfActor || actor.Affiliation == selfActor.Affiliation)
                {
                    continue;
                }

                // Check if the other actor is within this actor's sight range.
                float distanceSquared = (actor.TargetLookAtTransform.position - selfActor.TargetLookAtTransform.position).sqrMagnitude;
                if (distanceSquared > sightRadiusSquared || distanceSquared > closestDistanceSquared)
                {
                    continue;
                }

                /*
                // Raycast to check if the other actor is behind a wall.
                Vector3 directionToTargetActor = (actor.TargetLookAtTransform.position - eyeTransform.position).normalized;
                RaycastHit[] hits = Physics.RaycastAll(eyeTransform.position, (actor.TargetLookAtTransform.position - selfActor.TargetLookAtTransform.position).normalized, detectionRange, layers, QueryTriggerInteraction.Ignore);

                bool colliderDetected = false;
                RaycastHit closestHit = new RaycastHit();
                float closestHitDistance = float.PositiveInfinity;
                foreach (RaycastHit hit in hits)
                {
                    if (hit.distance > closestHitDistance || ignoredColliders.Contains(hit.collider))
                    {
                        continue;
                    }

                    colliderDetected = true;
                    closestHit = hit;
                }

                if (!colliderDetected)
                {
                    continue;
                }

                Actor hitActor = closestHit.collider.GetComponentInParent<Actor>();
                if (hitActor != actor)
                {
                    continue;
                }
                */

                RaycastHit hitInfo;
                bool raycastHit = Physics.Raycast(eyeTransform.position, (actor.TargetLookAtTransform.position - selfActor.TargetLookAtTransform.position).normalized, out hitInfo, detectionRange, layers, QueryTriggerInteraction.Ignore);

                if (raycastHit)
                {
                    Actor raycastHitActor = hitInfo.collider.GetComponentInParent<Actor>();
                    if (raycastHitActor != actor)
                    {
                        continue;
                    }
                }

                closestDistanceSquared = distanceSquared;
                latestSeenTarget = actor.TargetLookAtTransform.gameObject;
                lastSeenTargetTime = Time.time;         

                canSeeTarget = true;

                Debug.DrawLine(eyeTransform.position, actor.TargetLookAtTransform.position, Color.green);             
            }

            // Check if the target is in the attack range.
            isTargetInAttackRange = latestSeenTarget && (closestDistanceSquared <= attackRange * attackRange);

            if (!seenTarget && canSeeTarget)
            {
                onTargetDetected?.Invoke(latestSeenTarget);
                Debug.Log($"{name}: Target detected");
            }

            if (seenTarget && !canSeeTarget)
            {
                onTargetLost?.Invoke(seenTarget);
                Debug.Log($"{name}: Target lost");
            }

            seenTarget = latestSeenTarget;
        }
    }
}
