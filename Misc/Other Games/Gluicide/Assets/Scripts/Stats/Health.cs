using UnityEngine;

public class Health : MonoBehaviour
{
    // Player Health
    private bool playerHealth;

    // Health Variables
    public int health = 10;
    private int maxHealth;
    [SerializeField] private bool destroyOnDeath = true;

    [SerializeField] private bool takesImpactDamage;
    [SerializeField] private float impactDamageVelocity;
    [SerializeField] private float impactDamageScale;

    // SOURCE https://www.youtube.com/watch?app=desktop&v=l3iK-Vp2Nm8

    private void Start()
    {
        // Check if Player
        if (GetComponent<Player>() != null)
        {
            playerHealth = true;
        }
        else
        {
            playerHealth = false;
        }

        maxHealth = health;

        Invoke("UpdateStat", 0.01f);
    }

    public void Damage(int damage)
    {
        if (health > 0)
        {
            // Damage Player
            health -= damage;

            // Death
            if (health <= 0)
            {
                health = 0;

                Death();
            }
        }

        UpdateStat();
    }

    public void Heal(int healing)
    {
        if (health < maxHealth)
        {
            // Damage Player
            health += healing;

            // Death
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }

        UpdateStat();
    }

    private void Death()
    {
        print(gameObject.name + " was killed.");

        // Destroy Object
        if (destroyOnDeath)
        {
            Destroy(gameObject);
        }

        // Player Death
        if (playerHealth)
        {
            GameManager.Player.PlayerDeath();
        }
    }

    private void UpdateStat()
    {
        if (GetComponent<StatSystem>() != null)
        {
            StatSystem Stats = GetComponent<StatSystem>();

            Stats.SetStat(StatSystem.StatType.Health, health);
            Stats.resources[(int)StatSystem.StatType.Health - 1].maximum = maxHealth;
        }
        else
        {
            print("No Stat System found.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Impact Damage
        if (takesImpactDamage && collision.gameObject.layer != 3 && collision.gameObject.layer != 6 && collision.gameObject.layer != 7 && collision.otherCollider.gameObject.layer != 7)
        {
            int damage = 0;

            if (Mathf.Abs(collision.relativeVelocity.x) > impactDamageVelocity)
            {
                Damage((int)((Mathf.Abs(collision.relativeVelocity.x) - impactDamageVelocity) * impactDamageScale));

                damage += (int)((Mathf.Abs(collision.relativeVelocity.x) - impactDamageVelocity) * impactDamageScale);
            }

            if (Mathf.Abs(collision.relativeVelocity.y) > impactDamageVelocity)
            {
                Damage((int)((Mathf.Abs(collision.relativeVelocity.y) - impactDamageVelocity) * impactDamageScale));

                damage += (int)((Mathf.Abs(collision.relativeVelocity.y) - impactDamageVelocity) * impactDamageScale);
            }

            if (damage > 0)
            {
                print(name + " took " + damage + " impact damage after colliding with " + collision.collider.name + "!");
            }
        }
    }
}