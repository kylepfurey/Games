using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class SteeringDestination : MonoBehaviour
    {
        private Transform destinationTransform;
        public Transform DestinationTransform => destinationTransform;

        private GameObject followTargetObject;
        private Transform followedTarget;
        private Vector3 followOffset;

        /// <summary>
        /// Invoked when the destination has been changed.
        /// </summary>
        public delegate void OnDestinationUpdated(Vector3 newDestination);
        public event OnDestinationUpdated onDestinationUpdated;

        private void Awake()
        {
            // Create an empty object whose position is used for determining the NPC's destination.
            followTargetObject = new GameObject($"{name}_Destination");
            followTargetObject.transform.position = transform.position;

            destinationTransform = followTargetObject.transform;

            // Ensure that when the scene containing the agent is destroyed, this game object is also destroyed.
            SceneManager.MoveGameObjectToScene(followTargetObject, gameObject.scene);
        }

        private void OnDestroy()
        {
            Destroy(followTargetObject);
        }

        private void Update()
        {
            if (followedTarget)
            {
                Vector3 newTargetPosition = followedTarget.TransformPoint(followOffset);

                Vector3 difference = newTargetPosition - destinationTransform.position;
                if (difference.sqrMagnitude > Mathf.Epsilon)
                {
                    destinationTransform.position = newTargetPosition;
                    onDestinationUpdated?.Invoke(destinationTransform.position);           
                }
            }
        }

        /// <summary>
        /// Sets the destination to a fixed position.
        /// </summary>
        public void SetDestination(Vector3 position, Vector3 offset)
        {
            if (destinationTransform)
            {
                followedTarget = null;
                destinationTransform.position = position + offset;

                onDestinationUpdated?.Invoke(destinationTransform.position);
            }
        }

        /// <summary>
        /// Sets the destination to a dynamic position.
        /// </summary>
        public void SetDestination(Transform transform, Vector3 followOffset)
        {
            if (destinationTransform)
            {
                this.followedTarget = transform;
                this.followOffset = followOffset;
            }
        }

        public Vector3 GetDestinationPosition()
        {
            return destinationTransform.position;
        }
    }
}
