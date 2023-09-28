using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame
{
    public class PlayerTeleportEffects : MonoBehaviour
    {
        [SerializeField]
        private PlayerMovement playerMovement;

        [SerializeField]
        private GameObject teleportVFXPrefab;

        private void OnValidate()
        {
            if (!playerMovement)
            {
                playerMovement = FindObjectOfType<PlayerMovement>();
            }
        }

        private void Start()
        {
            if (!playerMovement)
            {
                playerMovement = FindObjectOfType<PlayerMovement>();
            }

            playerMovement.onTeleportBegins += OnTeleportBegins;
            playerMovement.onTeleportEnds += OnTeleportEnds;
        }

        private void OnDestroy()
        {
            playerMovement.onTeleportBegins -= OnTeleportBegins;
            playerMovement.onTeleportEnds -= OnTeleportEnds;
        }

        private void OnTeleportBegins()
        {
            PlayVFXEffect();
        }

        private void OnTeleportEnds()
        {
            PlayVFXEffect();
        }     

        private void PlayVFXEffect()
        {
            GameObject vfxEffectInstance = Instantiate(teleportVFXPrefab, transform.position, transform.rotation, null);

            float vfxDuration = vfxEffectInstance.GetComponent<ParticleSystem>().main.duration;

            Destroy(vfxEffectInstance, vfxDuration);
        }
    }
}
