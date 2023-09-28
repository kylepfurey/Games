using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FPSGame
{
    public class WeaponModeDisplayer : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text;

        [SerializeField]
        private PlayerCombat playerCombat;

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
            text.text = $"Weapon Mode: {(playerCombat.PlayerCombatMode == PlayerCombatMode.AssaultRifle ? "Assault Rifle": "Teleport")}";
        }
    }
}
