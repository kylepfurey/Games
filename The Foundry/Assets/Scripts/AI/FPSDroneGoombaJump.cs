using System.Collections;
using UnityEngine;

namespace FPSGame
{
    public class FPSDroneGoombaJump : MonoBehaviour
    {
        [SerializeField]
        private Health health;

        [SerializeField]
        private float jumpForce = 20.0f;

        private Coroutine goombaJumpDelay = null;

        private void OnValidate()
        {
            if (!health)
                health = GetComponentInParent<Health>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Debug.Log("Collision");

            PlayerMovement playerMovement = collision.collider.GetComponent<PlayerMovement>();
            if (!playerMovement)
            {
                return;
            }

            if (goombaJumpDelay != null)
            {
                return;
            }

            for (int i = 0; i < collision.contactCount; ++i)
            {
                Vector3 normal = collision.GetContact(i).normal;
                float angle = Vector3.Angle(Vector3.up, normal);

                //Debug.Log($"Angle: {angle}");

                if (angle >= 120.0f)
                {
                    playerMovement.Motor.BaseVelocity.y = 0.0f;
                    playerMovement.AddVelocity(Vector3.up * jumpForce);

                    health.OnTakeDamage(0.5f * health.maxHealth, playerMovement.GetComponentInParent<Actor>());
                    
                    //Debug.Log("Goomba jump!");

                    goombaJumpDelay = StartCoroutine(ReenableGoombaJump());

                    playerMovement.currentJump = 1;
                    break;
                }
            }
        }

        private IEnumerator ReenableGoombaJump()
        {
            yield return new WaitForSeconds(0.5f);
            goombaJumpDelay = null;
        }
    }
}
