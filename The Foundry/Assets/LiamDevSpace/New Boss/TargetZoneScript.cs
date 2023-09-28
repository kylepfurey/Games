using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPSGame;

public class TargetZoneScript : MonoBehaviour
{
    public float Damage = 20f;
    public GameObject targetIndicator;
    public string layername = "Player";

    public List<Health> healths = new List<Health>();

    public float rotSpeedMultiplier = 5f;
    public Vector3 theVect;

    public bool testbutton;
    public GameObject explosionFXPrefab;
    // Start is called before the first frame update
    void Start()
    {
        AirScan();
    }

    // Update is called once per frame
    void Update()
    {
        targetIndicator.transform.Rotate(theVect * rotSpeedMultiplier * Time.deltaTime);

        if (testbutton)
        {
            testbutton = false;
            Detonate();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == layername)
        {
            Debug.Log($"ISLAYER ");
            Health theHealh = other.GetComponentInParent<Health>();
            if (theHealh != null && !healths.Contains(theHealh))
            {
                healths.Add(theHealh);
            }
        }
        else
        {
            Debug.Log($"NOTLAYER ");

        }
    }
    private void OnTriggerExit(Collider other)
    {
        Health theHealh = other.GetComponentInParent<Health>();
        if (theHealh != null && healths.Contains(theHealh))
        {
            healths.Remove(theHealh);
        }
    }

    public void Detonate()
    {
        GameObject.Instantiate(explosionFXPrefab, transform, false).transform.parent = null;
        foreach (Health health in healths)
        {
            if (LOSCheck(health) == false)
            {
                Debug.Log("CAUSEDAMAGE");
                health.OnTakeDamage(Damage, null);
            }
            else
            {
                Debug.Log("NODAMAGE");
            }
        }
        Destroy(gameObject);
    }

    public LayerMask layermask;
    public bool LOSCheck(Health health)
    {
        return (Physics.Linecast(transform.position + Vector3.up*.5f, health.transform.position + Vector3.up * 0.5f, layermask, QueryTriggerInteraction.Ignore));

    }

    public void AirScan()
    {
        int layerMask = 1<<13;
        RaycastHit hit;
        Ray theRay = new Ray(gameObject.transform.position + Vector3.up, Vector3.down);
        if (Physics.Raycast(theRay.origin, theRay.direction, out hit, 10f, layerMask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(theRay.origin, hit.point, Color.green, Time.deltaTime);
            transform.position = hit.point + Vector3.up * .1f;
        }
        else
        {
            Debug.DrawRay(theRay.origin, theRay.direction * 10f, Color.red, Time.deltaTime);
        }
    }

    private void OnTriggerStay(Collider other)
    {

    }
}
