using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class RotatingPlatformMovement : PlatformMovement
    {
        [SerializeField]
        private Vector3 eulers;

        public override void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            goalPosition = transform.position;
            goalRotation = Quaternion.Euler(eulers * deltaTime) * transform.rotation; 
        }
    }
}
