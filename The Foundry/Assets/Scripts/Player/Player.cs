using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private Actor owningActor;
        public Actor OwningActor => owningActor;

        [SerializeField]
        private PlayerMovement playerMovement;
        public PlayerMovement PlayerMovement => playerMovement;

        [SerializeField]
        private PlayerCombat playerCombat;
        public PlayerCombat PlayerCombat => playerCombat;

        [SerializeField]
        private Health health;
        public Health Health => health;

        private void OnValidate()
        {
            if (!owningActor)
            {
                owningActor = GetComponent<Actor>();
            }

            if (!playerMovement)
            {
                playerMovement = GetComponent<PlayerMovement>();
            }

            if (!playerCombat)
            {
                playerCombat = GetComponent<PlayerCombat>();
            }

            if (!health)
            {
                health = GetComponent<Health>();
            }
        }
    }
}
