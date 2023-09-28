using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame
{
    public class IncreaseDamageItem : Item
    {
        public float bonusDamage = 20.0f;

        public override void OnPickedUp(Player player)
        {
            player.PlayerCombat.bonusDamage += bonusDamage;
        }
    }
}
