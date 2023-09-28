using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPSGame
{
    public class TeleportIcon : MonoBehaviour
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

        private void Update()
        {
            image.enabled = player.PlayerCombat.HasTeleportProjectile;
        }
    }
}
