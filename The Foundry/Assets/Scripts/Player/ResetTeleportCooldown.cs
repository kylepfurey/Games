using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame
{
    public class ResetTeleportCooldown : MonoBehaviour
    {
        [SerializeField]
        private Actor playerActor;

        [SerializeField]
        private PlayerCombat playerCombat;

        private void OnValidate()
        {
            if (!playerActor)
                playerActor = GetComponentInParent<Actor>();

            if (!playerCombat)
                playerCombat = GetComponentInParent<PlayerCombat>();
        }

        private void OnEnable()
        {
            playerActor.onKillTarget += OnKillTarget;
        }

        private void OnDisable()
        {
            playerActor.onKillTarget -= OnKillTarget;
        }

        private void OnKillTarget(Actor target)
        {
            //playerCombat.ResetTeleportCooldown();
        }
    }
}
