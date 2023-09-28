using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPSGame;


public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    public Rigidbody RigidBody => rb;

    public float LaunchSpeed = 60.0f;

    [SerializeField] private float damage;
    public float Damage
    {
        get => damage;
        set => damage = value < 0 ? 0 : value;
    }

    [SerializeField] private GameObject impactEffect;

    public float GravityScale = 1.0f;

    [Tooltip("How long can the projectile exists before it is destroyed? If the value is non-positive, the projectile exists forever.")]
    public float Age = 5.0f;

    [Tooltip("If the projectile hits an object, it is destroyed.")]
    public bool DestroyedOnImpact = true;

    [Tooltip("What component is responsible for the existence of the projectile (e.g. the character that shoots this projectile, or the weapon this projectile comes from)?")]
    public Component Instigator = null;

    public Rigidbody ownedRigidbody; //Gonna use this to prevent selfcollisions

    private bool shouldMove = true;

    private GameObject Player;
    [SerializeField] private GameObject BurnMark;
    [SerializeField] private float renderDistance;

    private void OnValidate()
    {
        if (!rb)
        {
            rb = GetComponentInChildren<Rigidbody>(true);
        }
        if (rb)
        {
            rb.useGravity = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (transform.tag != collision.transform.tag || transform.tag == "Untagged")
        {
            if (collision.collider.attachedRigidbody != null)
            {
                if (collision.collider.attachedRigidbody != ownedRigidbody)
                {
                    Debug.Log($"Collider: {collision.collider.name}");

                    if (DestroyedOnImpact)
                    {
                        InflictContactDamage(collision);
                        DestroyProjectile();
                    }
                }
            }
            else
            {
                if (DestroyedOnImpact)
                {
                    GameObject burnMark = Instantiate(BurnMark);
                    burnMark.transform.position = transform.position;
                    burnMark.transform.rotation = Quaternion.FromToRotation(Vector3.forward, collision.contacts[0].normal);

                    if (collision.transform.parent != null)
                    {
                        burnMark.transform.parent = collision.transform.parent;
                    }

                    if (Mathf.Abs(Vector3.Distance(burnMark.transform.position, Player.transform.position)) > renderDistance)
                    {
                        Destroy(burnMark);
                    }

                    InflictContactDamage(collision);
                    DestroyProjectile();
                }
            }
        }
    }

    private void InflictContactDamage(Collision collision)
    {
        Health theHealth = collision.collider.GetComponent<Health>();
        if (theHealth == null)
        {
            theHealth = collision.collider.GetComponent<Health>();
        }
        if (theHealth == null)
        {
            theHealth = collision.collider.GetComponentInParent<Health>();
        }
        if (theHealth == null)
        {
            theHealth = collision.collider.GetComponentInChildren<Health>();
        }

        if (theHealth != null)
        {
            Actor theActor = Instigator as Actor;

            theHealth.OnTakeDamage(Damage, theActor);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        shouldMove = false;
    }

    private void Start()
    {
        rb.AddForce(transform.forward * LaunchSpeed, ForceMode.VelocityChange);

        if (Age > 0)
        {
            StartCoroutine(DestroyCoroutine());
        }

        Player = GameObject.Find("FPSCharacter");
    }

    private void FixedUpdate()
    {
        if (shouldMove)
        {
            rb.AddForce(Physics.gravity * GravityScale, ForceMode.Acceleration);
        }
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(Age);
        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        Debug.Log("Destroy projectile");
        SpawnEffect();
        Destroy(gameObject);
    }

    private void SpawnEffect()
    {
        if (impactEffect)
        {
            GameObject impactEffectInstance = Instantiate(impactEffect, transform.position, Quaternion.identity, null);
            impactEffectInstance.name = $"{name} - Impact";

            Destroy(impactEffectInstance, impactEffectInstance.GetComponent<ParticleSystem>().main.duration);
        }
    }

    public static Projectile SpawnProjectile(GameObject projectilePrefab, Vector3 position, Quaternion rotation, Component instigator)
    {
        GameObject projectileObject = Instantiate(projectilePrefab, position, rotation);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Instigator = instigator;

        return projectile;
    }
}