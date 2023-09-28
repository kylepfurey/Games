using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;

namespace GameFramework
{
    public class PlatformMovement : MonoBehaviour, IMoverController
    {
        [SerializeField]
        private PhysicsMover physicsMover;

        protected virtual void OnValidate()
        {
            if (!physicsMover)
                physicsMover = GetComponent<PhysicsMover>();
        }

        protected virtual void Awake() {}

        protected virtual void Start()
        {
            physicsMover.MoverController = this;
        }

        public virtual void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            goalPosition = transform.position;
            goalRotation = transform.rotation;
        }
    }
}
