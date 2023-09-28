using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FPSGame
{
    public class HealthDisplayer : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text;

        [SerializeField]
        private Player player;

        private void OnValidate()
        {
            if (!text)
            {
                text = GetComponent<TMP_Text>();
            }

            if (!player)
            {
                player = FindObjectOfType<Player>();
            }
        }

        private void Start()
        {
            if (!player)
            {
                player = FindObjectOfType<Player>();
            }
        }

        private void Update()
        {
            text.text = $"Health: {player.Health.currentHealth}";

            if (player.Health.maxHealth > 100)
            {
                transform.localScale = new Vector3(2.5f, 1.5f, 1.5f);
            }
            else
            {
                transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
        }
    }
}
