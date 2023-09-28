using GameFramework.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame
{
    public class Turret : MonoBehaviour
    {
        [SerializeField]
        private Actor actor;

        [Header("Rotation")]
        [SerializeField]
        private Transform cannonHeadTransform;

        [SerializeField]
        private float minPitch = -45.0f;

        [SerializeField]
        private float maxPitch = 45.0f;

        [SerializeField]
        private float rotationSpeed = 30.0f;

        [Header("Combat")]
        [SerializeField]
        private GameFramework.AI.AISight aiSight;

        [SerializeField]
        private Actor target;

        [SerializeField]
        private Transform leftMuzzleTransform;

        [SerializeField]
        private Transform rightMuzzleTransform;

        // Used for detecting the player.
        [SerializeField]
        private LayerMask detectionLayerMask;

        [SerializeField]
        private GameObject projectilePrefab;

        [SerializeField]
        private GameObject muzzleFlashPrefab;

        [SerializeField]
        private float fireInterval = 0.5f;

        [SerializeField]
        private float fireCooldown = 0.0f;

        private Coroutine fireCoroutine = null;

        [Header("Health")]

        [SerializeField]
        private Health health;

        [SerializeField]
        private GameObject destroyParticleEffect;

        private void OnValidate()
        {
            if (!actor)
                actor = GetComponent<Actor>();

            if (!aiSight)
                aiSight = GetComponent<GameFramework.AI.AISight>();

            if (!health)
                health = GetComponent<Health>();
        }

        private void Start()
        {
            aiSight.onStimulusEvaluated += OnStimulusEvaluated;

            health.onDead += OnDead;
        }
        private void Update()
        {
            RotateTowardsTarget();

            if (CanSeeTarget())
            {
                if (fireCooldown <= 0.0f)
                {
                    FireProjectile();
                    fireCooldown = fireInterval;
                }
            }

            if (fireCooldown > 0.0f)
            {
                fireCooldown -= Time.deltaTime;
            }
        }

        private void OnDestroy()
        {
            aiSight.onStimulusEvaluated -= OnStimulusEvaluated;

            health.onDead -= OnDead;
        }

        private void OnStimulusEvaluated(IAISightStimulus stimulus, bool canBeSeen)
        {
            if (canBeSeen)
            {
                Actor potentialTarget = stimulus.transform.GetComponentInParent<Actor>();
                if (!potentialTarget)
                    return;

                target = potentialTarget;
            }
            else
            {
                Actor potentialTarget = stimulus.transform.GetComponentInParent<Actor>();
                if (!potentialTarget || target != potentialTarget)
                {
                    return;
                }

                target = null;
            }
        }

        private void OnTargetDetected(GameObject arg0)
        {
            Debug.Log("Enemy detected!");

            Actor potentialTarget = arg0.GetComponentInParent<Actor>();
            if (!potentialTarget)
            {
                return;
            }

            target = potentialTarget;
        }

        private void OnTargetLost(GameObject arg0)
        {
            Actor potentialTarget = arg0.GetComponentInParent<Actor>();
            if (!potentialTarget || target != potentialTarget)
            {
                return;
            }

            target = null;
        }

        private void RotateTowardsTarget()
        {
            if (!target)
            {
                return;
            }

            Quaternion targetRotation;

            targetRotation = Quaternion.LookRotation(target.TargetLookAtTransform.position - cannonHeadTransform.position);

            float targetPitch = targetRotation.eulerAngles.x;
            //targetPitch = Mathf.Clamp(targetPitch, minPitch, maxPitch);

            float targetYaw = targetRotation.eulerAngles.y;

            targetRotation = Quaternion.Euler(targetPitch, targetYaw, 0.0f);

            cannonHeadTransform.rotation = Quaternion.RotateTowards(cannonHeadTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Fix the roll bug,
            cannonHeadTransform.rotation = Quaternion.Euler(cannonHeadTransform.rotation.eulerAngles.x, cannonHeadTransform.rotation.eulerAngles.y, 0.0f);
        }

        private bool CanSeeTarget()
        {
            if (!target)
                return false;

            // Check if the left muzzle points at the target.
            RaycastHit leftMuzzleHitInfo;

            if (Physics.Raycast(leftMuzzleTransform.position, leftMuzzleTransform.forward, out leftMuzzleHitInfo, float.PositiveInfinity, detectionLayerMask, QueryTriggerInteraction.Ignore))
            {
                Actor potentialTarget = leftMuzzleHitInfo.collider.GetComponent<Actor>();
                if (potentialTarget && potentialTarget == target)
                {
                    Debug.DrawLine(leftMuzzleTransform.position, leftMuzzleHitInfo.point, Color.green);
                    return true;
                }
            }

            // Check if the right muzzle points at the target.
            RaycastHit rightMuzzleHitInfo;

            if (Physics.Raycast(rightMuzzleTransform.position, rightMuzzleTransform.forward, out rightMuzzleHitInfo, float.PositiveInfinity, detectionLayerMask, QueryTriggerInteraction.Ignore))
            {
                Actor potentialTarget = rightMuzzleHitInfo.collider.GetComponent<Actor>();
                if (potentialTarget && potentialTarget == target)
                {
                    Debug.DrawLine(rightMuzzleTransform.position, rightMuzzleHitInfo.point, Color.green);
                    return true;
                }
            }

            return false;
        }

        private void FireProjectile()
        {
            //GameObject leftProjectile = Instantiate(projectilePrefab, leftMuzzleTransform.position, leftMuzzleTransform.rotation, null);

            Projectile leftProjectile = Projectile.SpawnProjectile(projectilePrefab, leftMuzzleTransform.position, leftMuzzleTransform.rotation, actor);
            GameObject leftMuzzleFlash = Instantiate(muzzleFlashPrefab, leftMuzzleTransform.position, leftMuzzleTransform.rotation, null);

            //GameObject rightProjectile = Instantiate(projectilePrefab, rightMuzzleTransform.position, rightMuzzleTransform.rotation, null);

            Projectile rightProjectile = Projectile.SpawnProjectile(projectilePrefab, rightMuzzleTransform.position, rightMuzzleTransform.rotation, actor);
            GameObject rightMuzzleFlash = Instantiate(muzzleFlashPrefab, rightMuzzleTransform.position, rightMuzzleTransform.rotation, null);
        }

        private void OnDead()
        {
            GameObject destroyEffect = Instantiate(destroyParticleEffect, cannonHeadTransform.position, cannonHeadTransform.rotation, null);

            Destroy(gameObject);
        }
    }
}
