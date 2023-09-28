using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class OscillatingPlatformMovement : PlatformMovement
    {
        [SerializeField]
        private float oscillateMagnitude;

        [SerializeField]
        private float oscillatePeriod = 1.0f;

        [SerializeField]
        private float initialPhase = 0.0f;

        private Vector3 initialPosition;

        protected override void Awake()
        {
            base.Awake();

            initialPosition = transform.position;
        }

        public override void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            goalPosition = initialPosition + transform.forward * oscillateMagnitude * Mathf.Cos(2 * Mathf.PI / oscillatePeriod * Time.realtimeSinceStartup + Mathf.Deg2Rad * initialPhase);
            goalRotation = transform.rotation;
        }
    }
}
