using GameFramework.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMovementAI;

namespace FPSGame
{
    public class FPSDroneMovement : MonoBehaviour
    {
        // ------------------
        // Steering behaviors
        // ------------------

        [SerializeField]
        private FPSDroneBehavior droneBehavior;

        [SerializeField]
        private SteeringBasics steeringBasics;

        [SerializeField]
        private WallAvoidance wallAvoidance;

        [SerializeField]
        private LayerMask obstacleLayerMask = -1;

        [SerializeField]
        private float floorDetectionDistance = 0.1f;

        [SerializeField]
        private float ceilingDetectionDistance = 0.1f;

        [SerializeField]
        private CollisionAvoidance collisionAvoidance;

        [SerializeField]
        private bool movementEnabled = true;

        [SerializeField]
        private bool lookingEnabled = true;

        [SerializeField]
        private bool shouldFaceTowardsDestination = true;

        [SerializeField]
        private Vector3 destinationStatic;

        [SerializeField]
        private Transform destinationDynamic;
        public Transform DestinationDynamic => destinationDynamic;

        [SerializeField]
        private Vector3 lookTargetStatic;

        [SerializeField]
        private Transform lookTargetDynamic;

        private HashSet<MovementAIRigidbody> otherAgents;

        private RaycastHit[] obstacleCastHitInfos;

        private Component currentEnemy = null;

        private float initialArriveTargetRadius;

        private void OnValidate()
        {
            if (!droneBehavior)
                droneBehavior = GetComponent<FPSDroneBehavior>();

            if (!steeringBasics)
                steeringBasics = GetComponent<SteeringBasics>();

            if (!wallAvoidance)
                wallAvoidance = GetComponent<WallAvoidance>();

            if (!collisionAvoidance)
                collisionAvoidance = GetComponent<CollisionAvoidance>();
        }

        private void OnEnable()
        {
            droneBehavior.onEnemyUpdated += OnEnemyUpdated;
        }

        private void OnDisable()
        {
            droneBehavior.onEnemyUpdated -= OnEnemyUpdated;
        }

        private void Awake()
        {
            otherAgents = new HashSet<MovementAIRigidbody>();
            obstacleCastHitInfos = new RaycastHit[8];

            initialArriveTargetRadius = steeringBasics.targetRadius;
        }

        private void OnTriggerEnter(Collider other)
        {
            MovementAIRigidbody aiRigidBody = other.GetComponentInParent<MovementAIRigidbody>();
            if (aiRigidBody && aiRigidBody != steeringBasics.RB)
                otherAgents.Add(aiRigidBody);
        }

        private void OnTriggerExit(Collider other)
        {
            MovementAIRigidbody aiRigidBody = other.GetComponentInParent<MovementAIRigidbody>();
            if (aiRigidBody && aiRigidBody != steeringBasics.RB)
                otherAgents.Remove(aiRigidBody);
        }

        private void FixedUpdate()
        {
            // Update the move destination.
            if (destinationDynamic)
                destinationStatic = destinationDynamic.position;

            // Update the look target.
            if (lookTargetDynamic)
                lookTargetStatic = lookTargetDynamic.position;

            // Handle movement.
            if (movementEnabled)
            {
                // Handle steering.
                Vector3 acceleration = GetMovementAcceleration();
                steeringBasics.Steer(acceleration);
            }

            // Handle looking.
            if (lookingEnabled)
            {
                steeringBasics.LookAtDirection(lookTargetStatic - transform.position);

                //else if (shouldFaceTowardsDestination)
                //    steeringBasics.LookWhereYoureGoing();
            }

            // Some drones are destroyed by the player. Needs to be removed from list.
            otherAgents.RemoveWhere(e => !e);
        }

        private void OnEnemyUpdated(Component enemy)
        {
            if (!enemy)
            {
                // Go to the enemy's last seen position.
                if (currentEnemy)
                {
                    steeringBasics.targetRadius = 0.0f;
                    SetDestination(currentEnemy.transform.position);
                }
                else
                    SetDestination(null);

                LookAtTarget(null);
            }
            else
            {
                steeringBasics.targetRadius = initialArriveTargetRadius;
                SetDestination(enemy.transform);
                LookAtTarget(enemy.transform);
            }

            currentEnemy = enemy;
        }

        private Vector3 GetMovementAcceleration()
        {
            // Raycast to check if the AI can go straight to target.
            Vector3 obstacleCastStart = transform.position;
            Vector3 obstacleCastDirection = destinationStatic - transform.position;
            //float obstacleCastLength = obstacleCastDirection.magnitude;
            float obstacleCastLength = Time.deltaTime * steeringBasics.RB.RealVelocity.magnitude;
            RaycastHit obstacleHitInfo = new RaycastHit();

            //bool hasObstacleInFrontOfTarget = Physics.Raycast(obstacleCastStart, obstacleCastDirection, out obstacleHitInfo, obstacleCastLength, obstacleLayerMask, QueryTriggerInteraction.Ignore);

            int hitCount = 0;
            PhysicsUtils.SphereCastDetailed
            (
                obstacleCastStart,
                steeringBasics.RB.Radius,
                obstacleCastDirection,
                obstacleCastLength,
                transform,
                obstacleLayerMask,
                QueryTriggerInteraction.Ignore,
                ref hitCount,
                ref obstacleCastHitInfos,
                ref obstacleHitInfo
            );

            //bool canGoStraightToTarget = hitCount == 0 || (destinationDynamic != null && obstacleHitInfo.collider.transform.IsChildOf(destinationDynamic));
            bool canGoStraightToTarget = hitCount == 0;
            Debug.DrawLine(obstacleCastStart, obstacleCastStart + obstacleCastDirection * obstacleCastLength, canGoStraightToTarget ? Color.white : Color.red);

            // Prioritize avoiding obstacles over moving towards destination.
            Vector3 acceleration = wallAvoidance.GetSteering();

            // If there is no wall obstacle, avoid other agents.
            if (acceleration.magnitude < 0.005f || canGoStraightToTarget)
            {
                acceleration = collisionAvoidance.GetSteering(otherAgents);

                // If there is no agent to avoid, move to destination.
                if (acceleration.magnitude < 0.005f)
                {
                    acceleration = steeringBasics.Arrive(destinationStatic);

                    /*
                    if (acceleration.magnitude < 0.005f)
                    {
                        float heightDistance = transform.position.y - destination.y;

                        if (heightDistance < 0.0f || heightDistance > 2.0f)
                        {
                            acceleration = Vector3.down * heightDistance * 20.0f;
                        }
                    }
                    */


                    // Adjust the height of the drone to match the destination height.
                    float heightDifference = steeringBasics.RB.Position.y - destinationStatic.y;

                    //bool hasObstacleBelow = Physics.Raycast(steeringBasics.RB.Position, Vector3.down, steeringBasics.RB.Col3D.radius + floorDetectionDistance, obstacleLayerMask,  QueryTriggerInteraction.Ignore);
                    //bool hasObstacleAbove = Physics.Raycast(steeringBasics.RB.Position, Vector3.down, steeringBasics.RB.Col3D.radius + ceilingDetectionDistance, obstacleLayerMask, QueryTriggerInteraction.Ignore);

                    PhysicsUtils.SphereCastDetailed
                    (
                        steeringBasics.RB.Position,
                        steeringBasics.RB.Radius,
                        Vector3.down,
                        steeringBasics.RB.Col3D.radius + floorDetectionDistance,
                        transform,
                        obstacleLayerMask,
                        QueryTriggerInteraction.Ignore,
                        ref hitCount,
                        ref obstacleCastHitInfos,
                        ref obstacleHitInfo
                    );

                    bool hasObstacleBelow = hitCount > 0;

                    PhysicsUtils.SphereCastDetailed
                    (
                        steeringBasics.RB.Position,
                        steeringBasics.RB.Radius,
                        Vector3.up,
                        steeringBasics.RB.Col3D.radius + floorDetectionDistance,
                        transform,
                        obstacleLayerMask,
                        QueryTriggerInteraction.Ignore,
                        ref hitCount,
                        ref obstacleCastHitInfos,
                        ref obstacleHitInfo
                    );

                    bool hasObstacleAbove = hitCount > 0;

                    /*
                    if (heightDifference < 0.0f || heightDifference > 3.0f && !hasObstacleBelow)
                    {
                        acceleration += Vector3.down * heightDifference * 2.0f;
                    }
                    */

                    if ((heightDifference < 0.0f && !hasObstacleAbove) || (heightDifference > 3.0f && !hasObstacleBelow))
                    {
                        acceleration += Vector3.down * heightDifference * 2.0f;
                    }
                }
            }

            return acceleration;
        }

        public void SetDestination(Vector3 destination)
        {
            movementEnabled = true;
            this.destinationStatic = destination;
            this.destinationDynamic = null;
        }

        public void SetDestination(Transform destination)
        {
            if (!destination)
            {
                movementEnabled = false;
                this.destinationDynamic = null;
                this.destinationStatic = transform.position;
                return;
            }

            movementEnabled = true;
            this.destinationDynamic = destination;
            this.destinationStatic = destinationDynamic.position;
        }

        public void StopMovement()
        {
            movementEnabled = false;
        }

        public void LookAtTarget(Vector3 targetPosition)
        {
            lookingEnabled = true;
            lookTargetStatic = targetPosition;
            lookTargetDynamic = null;
        }

        public void LookAtTarget(Transform targetTransform)
        {
            if (!targetTransform)
            {
                lookingEnabled = false;
                lookTargetDynamic = null;
                return;
            }

            lookingEnabled = true;
            lookTargetDynamic = targetTransform;
            lookTargetStatic = lookTargetDynamic.position;
        }

        public void ClearLookTarget()
        {
            lookTargetDynamic = null;
        }
    }
}
