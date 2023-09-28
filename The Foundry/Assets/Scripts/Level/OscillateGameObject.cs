using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame
{
    public class OscillateGameObject : MonoBehaviour
    {
        [SerializeField]
        private float oscillateMagnitude;

        [SerializeField]
        private float oscillatePeriod = 1.0f;

        [SerializeField]
        private float initialPhase = 0.0f;

        private Vector3 initialPosition;

        private void Awake()
        {
            initialPosition = transform.position;
        }

        private void Update()
        {
            transform.position = initialPosition + transform.forward * oscillateMagnitude * Mathf.Cos(2 * Mathf.PI / oscillatePeriod * Time.realtimeSinceStartup + Mathf.Deg2Rad * initialPhase);
        }
    }
}
