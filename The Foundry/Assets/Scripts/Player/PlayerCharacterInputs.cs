using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame
{
    public struct PlayerCharacterInputs
    {
        // Movement
        public float RawMoveAxisForward;
        public float RawMoveAxisRight;
        public float MoveAxisForward;
        public float MoveAxisRight;
        public Quaternion CameraRotation;
        public bool JumpDown;
        public bool CrouchDown;
        public bool CrouchUp;

        // Combat
        public bool SwitchAutomatic;
        public bool SwitchTeleport;
        public bool Fire;
        public bool Teleport;
        public bool DestroyTeleportProjectile;
        public bool ToggleFlashLight;
    }
}
