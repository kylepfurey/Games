using FPSGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//WARNING: This contains a lot of duplication from the projectile script.
//Use this as a generic melee hitbox script.
public class MeleeHitbox : MonoBehaviour
{
    [Tooltip("What component controls this hitbox?")]
    private Actor Instigator;
    [Tooltip("If the projectile hits an object, it is destroyed.")]
    public bool DestroyedOnImpact = true;
    public Rigidbody ownedRigidbody; //Gonna use this to prevent selfcollisions
    [SerializeField] private GameObject impactEffect;

    public float currentDamage;
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        Instigator = GetComponentInParent<Actor>();
        ownedRigidbody = gameObject.transform.parent.GetComponentInParent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ActivateHitbox(float damage)
    {
        isActive = true;
        currentDamage = damage;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (isActive)// && (collider.gameObject.layer == layername))
        {
            Debug.Log($"Collider: {collider.gameObject.name}");
            if (collider.attachedRigidbody != null)
            {
                Debug.Log($"attachedrigidbody {collider.attachedRigidbody.gameObject.name}");
                if (collider.attachedRigidbody != ownedRigidbody)
                {
                    Debug.Log($"Collider: {collider.name}");

                    if (DestroyedOnImpact)
                    {
                        InflictContactDamage(collider);
                        DeactivateHitbox();


                    }
                }


            }
            else
            {
                Debug.Log($"no attached rigidbody");
                if (DestroyedOnImpact)
                {
                    InflictContactDamage(collider);
                    DeactivateHitbox();


                }
            }
        }


    }
    public void DeactivateHitbox()
    {
        Debug.Log("Deactivate hurtbox");
        SpawnEffect();
        isActive = false;
        //Destroy(gameObject);
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

    private void InflictContactDamage(Collider collider)
    {
        Health theHealth = collider.GetComponent<Health>();
        if (theHealth == null)
        {
            theHealth = collider.GetComponent<Health>();
        }
        if (theHealth == null)
        {
            theHealth = collider.GetComponentInParent<Health>();
        }
        if (theHealth == null)
        {
            theHealth = collider.GetComponentInChildren<Health>();
        }

        if (theHealth != null)
        {
            Actor theActor = Instigator as Actor;
            theHealth.OnTakeDamage(currentDamage, theActor);
        }
    }
}
