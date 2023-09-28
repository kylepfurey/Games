using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPSGame
{
    public class GameTrigger : MonoBehaviour
    {
        public UnityEvent onTriggerActivated;

        [SerializeField]
        private bool shouldDestroyAfterTriggered = true;

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponentInParent<Player>();
            if (player)
            {
                onTriggerActivated?.Invoke();

                if (shouldDestroyAfterTriggered)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
