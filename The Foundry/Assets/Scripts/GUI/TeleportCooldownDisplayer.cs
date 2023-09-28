using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace FPSGame
{
    public class TeleportCooldownDisplayer : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text;

        [SerializeField]
        private PlayerCombat playerCombat;

        //public GameObject isReadyObject;
        public GameObject isCooldownObject;

        public Slider cooldownBar;

        private void OnValidate()
        {
            if (!text)
            {
                text = GetComponent<TMP_Text>();
            }

            if (!playerCombat)
            {
                playerCombat = FindObjectOfType<PlayerCombat>();
            }
        }

        private void Start()
        {
            if (!playerCombat)
            {
                playerCombat = FindObjectOfType<PlayerCombat>();
            }
        }

        private void Update()
        {
            if (playerCombat.CurrentTeleportCooldown > 0.0f)
            {
                text.text = $"Teleport cooldown: {playerCombat.CurrentTeleportCooldown:.##}";

                isCooldownObject.SetActive(true);
                //isReadyObject.SetActive(false);

                cooldownBar.maxValue = playerCombat.teleportCooldown;
                cooldownBar.value = playerCombat.CurrentTeleportCooldown;
            }
            else
            {
                text.text = $"";

                isCooldownObject.SetActive(false);
                //isReadyObject.SetActive(true);
            }
        }
    }
}
