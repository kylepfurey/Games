using UnityEngine;

namespace FPSGame
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private Actor owningActor;

        public float maxHealth = 100.0f;
        public float currentHealth = 100.0f;
        public float damageBuffer = 0;
        public bool godMode = false;

        public delegate void OnHealthChanged(float newHealth);
        public event OnHealthChanged onHealthChanged;

        public delegate void OnTakeDamageDelegate(float damage, Actor instigator);
        public event OnTakeDamageDelegate onTakeDamage;

        public delegate void OnDead();
        public event OnDead onDead;

        private void OnValidate()
        {
            if (!owningActor)
            {
                owningActor = GetComponent<Actor>();
            }

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }

        public void OnTakeDamage(float damage, Actor instigator)
        {
            if (godMode)
                return;

            if (instigator)
            {
                // Allies cannot take damage from each other.
                if ((instigator == owningActor || instigator.Affiliation == owningActor.Affiliation))
                    return;
            }

            SetHealth(currentHealth - damage);

            onTakeDamage?.Invoke(damage, instigator);

            if (currentHealth <= 0)
                Die(instigator);
        }

        public void SetHealth(float newHealth)
        {
            currentHealth = Mathf.Clamp(newHealth, 0f, maxHealth);
            onHealthChanged?.Invoke(currentHealth);
        }

        public void Die(Actor instigator)
        {
            onDead?.Invoke();
            instigator?.onKillTarget?.Invoke(owningActor);
        }

        private void Update()
        {
            // Damage Buffer
            if (damageBuffer > 0)
            {
                damageBuffer -= Time.deltaTime;
            }
            else
            {
                damageBuffer = 0;
            }
        }
    }
}