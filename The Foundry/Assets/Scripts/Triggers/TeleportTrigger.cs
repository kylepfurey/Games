using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame
{
    public class TeleportTrigger : MonoBehaviour
    {
        [SerializeField]
        private Transform teleportDestination;

        private void OnTriggerEnter(Collider other)
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (!playerMovement)
                return;

            playerMovement.Motor.SetPosition(teleportDestination.position);
            playerMovement.Motor.SetRotation(teleportDestination.rotation);
        }
    }
}
