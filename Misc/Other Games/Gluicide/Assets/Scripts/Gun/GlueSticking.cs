using UnityEngine;

public class GlueSticking : MonoBehaviour
{
    private Rigidbody2D Rigidbody;
    [HideInInspector] public bool objectSlidesOnGlue;

    private void Start()
    {
        if (transform.parent.transform.parent.transform.parent.GetComponent<Rigidbody2D>() != null)
        {
            Rigidbody = transform.parent.transform.parent.transform.parent.GetComponent<Rigidbody2D>();
        }

        Debug.LogWarning("This script is meant to be created by the Glue Projectile! Do not add this script to anything!");
    }

    private void Sticking(Collider2D collision)
    {
        // Glue Sticking Logic
        if (Rigidbody != null)
        {
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = 0;

            if (objectSlidesOnGlue == false)
            {
                Rigidbody.AddForce(-Physics2D.gravity * Rigidbody.mass);
            }
        }

        if (collision.attachedRigidbody != null)
        {
            collision.attachedRigidbody.velocity = Vector3.zero;
            collision.attachedRigidbody.angularVelocity = 0;

            if (objectSlidesOnGlue == false)
            {
                collision.attachedRigidbody.AddForce(-Physics2D.gravity * collision.attachedRigidbody.mass);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Sticking(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Sticking(collision);
    }
}