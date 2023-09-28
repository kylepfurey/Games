using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame
{
    public class Item : MonoBehaviour
    {
        [SerializeField]
        protected GameObject pickUpSound;

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponent<Player>();
            if (player)
            {
                OnPickedUp(player);

                if (pickUpSound)
                {
                    Instantiate(pickUpSound, transform.position, Quaternion.identity);
                }

                Destroy(gameObject);
            }
        }

        public virtual void OnPickedUp(Player player)
        {
            // Implemented in child classes.
        }
    }
}
