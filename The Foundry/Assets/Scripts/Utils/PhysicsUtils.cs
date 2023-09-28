using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Utils
{
    /// <summary>
    /// Utility functions for physics.
    /// </summary>
    public static class PhysicsUtils
    {
        /// <summary>
        /// Performs a detailed raycast. This raycast ignores all transforms and their children in ignoredTransforms
        /// and returns 1) the number of hit colliders, 2) hit results and 3) closest hit object.
        /// </summary>
        public static void RaycastDetailed
        (
            Vector3 origin,
            Vector3 direction,
            float maxDistance,
            Transform[] ignoredTransforms,
            int layerMask,
            QueryTriggerInteraction queryTriggerInteraction,
            ref int hitCount,
            ref RaycastHit[] results,
            ref RaycastHit closestHitResult
        )
        {
            hitCount = Physics.RaycastNonAlloc(origin, direction, results, maxDistance, layerMask, queryTriggerInteraction);

            // Filter out colliders that belong to any of ignored game objects.
            if (ignoredTransforms != null && ignoredTransforms.Length > 0)
            {
                for (int i = 0; i < hitCount; ++i)
                {
                    Collider currentCollider = results[i].collider;

                    for (int j = 0; j < ignoredTransforms.Length; ++j)
                    {
                        if (ignoredTransforms[j] == null)
                            continue;

                        if (currentCollider.transform.IsChildOf(ignoredTransforms[j].transform))
                        {
                            for (int k = i + 1; k < hitCount; ++k)
                                results[k - 1] = results[k];
                            --i;
                            --hitCount;
                            break;
                        }
                    }
                }
            }

            // Get the closest hit result.
            if (hitCount > 0)
            {
                closestHitResult = results[0];
                float closestDistanceSq = (closestHitResult.point - origin).sqrMagnitude;

                for (int i = 1; i < hitCount; ++i)
                {
                    RaycastHit currentHitResult = results[i];
                    float distanceSq = (currentHitResult.point - origin).sqrMagnitude;

                    if (distanceSq < closestDistanceSq)
                    {
                        closestHitResult = currentHitResult;
                        closestDistanceSq = distanceSq;
                    }
                }
            }
        }

        public static void RaycastDetailed
        (
            Vector3 origin,
            Vector3 direction,
            float maxDistance,
            Transform ignoredTransform,
            int layerMask,
            QueryTriggerInteraction queryTriggerInteraction,
            ref int hitCount,
            ref RaycastHit[] results,
            ref RaycastHit closestHitResult
        )
        {
            hitCount = Physics.RaycastNonAlloc(origin, direction, results, maxDistance, layerMask, queryTriggerInteraction);

            // Filter out colliders that belong to the ignored transform.
            if (ignoredTransform != null)
            {
                for (int i = 0; i < hitCount; ++i)
                {
                    Collider currentCollider = results[i].collider;

                    if (currentCollider.transform.IsChildOf(ignoredTransform))
                    {
                        for (int k = i + 1; k < hitCount; ++k)
                            results[k - 1] = results[k];
                        --i;
                        --hitCount;
                    }

                }
            }

            // Get the closest hit result.
            if (hitCount > 0)
            {
                closestHitResult = results[0];
                float closestDistanceSq = (closestHitResult.point - origin).sqrMagnitude;

                for (int i = 1; i < hitCount; ++i)
                {
                    RaycastHit currentHitResult = results[i];
                    float distanceSq = (currentHitResult.point - origin).sqrMagnitude;

                    if (distanceSq < closestDistanceSq)
                    {
                        closestHitResult = currentHitResult;
                        closestDistanceSq = distanceSq;
                    }
                }
            }
        }

        public static void SphereCastDetailed
        (
            Vector3 origin,
            float radius,
            Vector3 direction,
            float maxDistance,
            Transform ignoredTransform,
            int layerMask,
            QueryTriggerInteraction queryTriggerInteraction,
            ref int hitCount,
            ref RaycastHit[] results,
            ref RaycastHit closestHitResult
        )
        {
            hitCount = Physics.SphereCastNonAlloc(origin, radius, direction.normalized, results, maxDistance, layerMask, queryTriggerInteraction);

            // Filter out colliders that belong to the ignored transform.
            if (ignoredTransform != null)
            {
                for (int i = 0; i < hitCount; ++i)
                {
                    Collider currentCollider = results[i].collider;

                    if (currentCollider.transform.IsChildOf(ignoredTransform))
                    {
                        for (int k = i + 1; k < hitCount; ++k)
                            results[k - 1] = results[k];
                        --i;
                        --hitCount;
                    }

                }
            }

            // Get the closest hit result.
            if (hitCount > 0)
            {
                closestHitResult = results[0];
                float closestDistanceSq = (closestHitResult.point - origin).sqrMagnitude;

                for (int i = 1; i < hitCount; ++i)
                {
                    RaycastHit currentHitResult = results[i];
                    float distanceSq = (currentHitResult.point - origin).sqrMagnitude;

                    if (distanceSq < closestDistanceSq)
                    {
                        closestHitResult = currentHitResult;
                        closestDistanceSq = distanceSq;
                    }
                }
            }
        }
    }
}
