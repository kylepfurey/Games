using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPSGame;

//DRONE DEATH LOGIC
//Setting up death logic separately just in case anyone makes changes to the prefab or health scripts before I push.
public class DroneDeathLogic : MonoBehaviour
{
    private Health m_Health;

    public GameObject DeathExplosionFX;
    public float deathDuration = 1f;

    public GameObject droneDetectorTrigger; //This collider needs to be moved out of the playable area before drone gameobject destruction to avoid causing deep NRE issues with the drone avoidance systems

    public GameObject[] dropPrefabs;
    public float itemDropChance = .5f;


    private bool isDead; //bool to stop repeated deaths when enemy is shot while dead
    // Start is called before the first frame update
    void Start()
    {
        m_Health = GetComponent<Health>();
        m_Health.onDead += DeathListener;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DeathListener()
    {
        if (isDead == false)
        {
            isDead = true;
            MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                if (script != null)
                {
                    if (script != this)
                    {
                        script.enabled = false;
                    }
                }
            }

            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
            rb.angularVelocity = new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), Random.Range(-20f, 20f));

            droneDetectorTrigger.transform.position = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 1000f; //Move the detector away to avoid triggering NREs in the avoidance system
            GameObject.Instantiate(DeathExplosionFX, transform.position, transform.rotation);

            float chanceRoll = Random.Range(0f, 1f);
            if (chanceRoll <= itemDropChance)
            {
                GameObject.Instantiate(dropPrefabs[Random.Range(0, dropPrefabs.Length)], transform.position, transform.rotation);

            }

            Destroy(gameObject, deathDuration);
        }

    }

    //private void StartDea
}
