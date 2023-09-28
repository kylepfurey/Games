using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame
{
    public class TestDroneSteeringBehavior : MonoBehaviour
    {
        [SerializeField]
        private FPSDroneMovement droneMovement;

        [SerializeField]
        private Transform destinationTransform;

        private void OnValidate()
        {
            if (!droneMovement)
            {
                droneMovement = GetComponent<FPSDroneMovement>();
            }
        }

        private void LateUpdate()
        {
            if (destinationTransform)
            {
                droneMovement.SetDestination(destinationTransform.position);
            }
        }
    }
}
