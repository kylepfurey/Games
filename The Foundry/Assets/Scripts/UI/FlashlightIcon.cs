using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPSGame
{
    public class FlashlightIcon : MonoBehaviour
    {
        [SerializeField]
        private Player player;

        [SerializeField]
        private Image image;

        private void OnValidate()
        {
            if (!player)
            {
                player = FindObjectOfType<Player>();
            }

            if (!image)
            {
                image = GetComponent<Image>();
            }
        }

        private void Start()
        {
            UpdateIcon();

            player.PlayerCombat.onFlashLightUpdated += OnFlashLightUpdate;
        }

        private void OnFlashLightUpdate(bool isLightOn)
        {
            UpdateIcon();
        }

        private void UpdateIcon()
        {
            if (image != null && player.PlayerCombat.FlashLight != null)
            {
                image.enabled = player.PlayerCombat.FlashLight.activeSelf;
            }
        }
    }
}
