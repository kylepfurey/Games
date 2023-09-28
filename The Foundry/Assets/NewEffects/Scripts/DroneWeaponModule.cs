using FPSGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneWeaponModule : MonoBehaviour
{
    public float fireCooldown = .5f;
    public float fireRange = 20f;
    private float fireTimer;

    public List<Transform> fireTransforms = new List<Transform>(); //Assign these in the inspector for multibarreled weapons
    public int currentFireIndex;

    public GameObject projectilePrefab;
    public GameObject muzzleFlashPrefab;


    private DroneCombatA droneCombat;
    // Start is called before the first frame update
    void Start()
    {
        droneCombat = GetComponent<DroneCombatA>();
        droneCombat.fireCommand += FireUpdate;
    }

    public void FireUpdate(Transform target)
    {
        if (Vector3.Distance(gameObject.transform.position, target.position) < fireRange) //Distance checking here is probably unoptimized but whatev
        {
            if (fireTimer < Time.time)
            {
                fireTimer = Time.time + fireCooldown;
                Fire(target);
            }
        }
    }

    public void Fire(Transform target)
    {
        CycleIndex();

        Transform fireTransform = fireTransforms[currentFireIndex];
        fireTransform.LookAt(target);//Face the target to ensure projectiles go towards them (some barrels will be spaced too far for just the turret transform). Should rotate transform in case projectile has raycast logic

        //GameObject newProjectile = GameObject.Instantiate(projectilePrefab, fireTransform.position, fireTransform.rotation);

        Projectile projectile = Projectile.SpawnProjectile(projectilePrefab, fireTransform.position, fireTransform.rotation, GetComponentInParent<Actor>());
        projectile.ownedRigidbody = GetComponent<Rigidbody>();

        GameObject.Instantiate(muzzleFlashPrefab, fireTransform);
        Debug.Log($"Drone {gameObject.name} has fired!");
    }

    void CycleIndex()
    {
        if (currentFireIndex == fireTransforms.Count - 1)
        {
            currentFireIndex = 0;
        }
        else
        {
            currentFireIndex++;
        }
    }
}
