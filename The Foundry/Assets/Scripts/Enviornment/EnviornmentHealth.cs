using UnityEngine;

public class EnviornmentHealth : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private bool canBeDamaged;
    [SerializeField] private GameObject Death;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (canBeDamaged)
            {
                health -= 1;

                if (health <= 0)
                {
                    GameObject explosion = Instantiate(Death);
                    explosion.transform.position = transform.position;

                    Destroy(gameObject);
                }
            }
        }
    }
}