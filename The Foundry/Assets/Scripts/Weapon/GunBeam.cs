using UnityEngine;

public class GunBeam : MonoBehaviour
{
    [SerializeField] private GameObject RayStartingPoint;
    [SerializeField] private GameObject RayBeam;
    [SerializeField] private float RayMaxDistance = 100;
    [SerializeField] public bool On;

    void Update()
    {
        // Set Ray Pivot
        if (RayStartingPoint == null)
        {
            RayStartingPoint = gameObject;
            RayBeam = RayStartingPoint.transform.GetChild(0).gameObject;
        }


        // Turn Beam On or Off
        RayBeam.GetComponent<MeshRenderer>().enabled = On;


        // Adjust Length of Ray
        if (Physics.Raycast(RayStartingPoint.transform.position, RayStartingPoint.transform.forward, out RaycastHit RayHitPoint, RayMaxDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            RayBeam.transform.localScale = new Vector3(RayBeam.transform.localScale.x, RayBeam.transform.localScale.y, RayHitPoint.distance);
        }
        else
        {
            RayBeam.transform.localScale = new Vector3(RayBeam.transform.localScale.x, RayBeam.transform.localScale.y, RayMaxDistance);
        }

        RayBeam.transform.localPosition = new Vector3(RayBeam.transform.localPosition.x, RayBeam.transform.localPosition.y, RayBeam.transform.localScale.z / 2);
    }
}