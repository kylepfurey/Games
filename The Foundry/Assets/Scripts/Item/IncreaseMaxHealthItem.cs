using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame
{
    public class IncreaseMaxHealthItem : Item
    {
        public float maxHealthBonus = 50.0f;

        public override void OnPickedUp(Player player)
        {
            player.Health.maxHealth += maxHealthBonus;
            player.Health.currentHealth = player.Health.maxHealth;

            GameObject.Find("HealthDisplayer").transform.localScale = new Vector3(2.5f, 1.5f, 1.5f);
        }
    }
}