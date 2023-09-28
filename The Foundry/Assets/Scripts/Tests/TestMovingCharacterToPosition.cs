using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame
{
    public class TestMovingCharacterToPosition : MonoBehaviour
    {
        [SerializeField]
        private PlayerMovement playerMovement;

        [ContextMenu("Teleport Character")]
        private void TeleportCharacter()
        {
            playerMovement.Motor.SetPosition(transform.position);
            playerMovement.Motor.SetRotation(transform.rotation);
        }
    }
}
