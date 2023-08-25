using UnityEngine;

public class Pointer : MonoBehaviour
{
    public Player Player;
    public GameObject Ray;  // Parent is Pivot
    public GameObject PointerDot;

    public Ray hitscan;
    public RaycastHit hitpoint;
    public float rayMaxDistance;
    public float rayCurrentDistance;

    public Vector3 pointerPosition;
    public Vector3 pointerCenterPosition;

    public bool randomDeviation;
    public Vector2 deviation;

    void Update()
    {
        hitscan = Player.Camera.ScreenPointToRay(new Vector3(Player.Camera.pixelWidth / 2, Player.Camera.pixelHeight / 2 + deviation.y, 0));

        // Deviation
        if (randomDeviation)
        {
            hitscan.direction = Quaternion.Euler(Player.Camera.transform.eulerAngles + new Vector3(-Random.Range(-deviation.y, deviation.y), Random.Range(-deviation.x, deviation.x), 0)) * Vector3.forward;
        }
        else
        {
            hitscan.direction = Quaternion.Euler(Player.Camera.transform.eulerAngles + new Vector3(-deviation.y, deviation.x, 0)) * Vector3.forward;
        }

        // Check Hitscan
        if (Physics.Raycast(hitscan, out hitpoint, rayMaxDistance))
        {
            // Calculate Center Position
            PointerDot.transform.position = Player.Camera.transform.position;
            PointerDot.transform.eulerAngles = new Vector3(Player.Camera.transform.eulerAngles.x, Player.Camera.transform.eulerAngles.y, 0);
            PointerDot.transform.Translate(new Vector3(0, 0, rayMaxDistance));
            pointerCenterPosition = PointerDot.transform.position;

            // Calculate Pointer Hit Position
            pointerPosition = hitpoint.point;

            // Ray Distance
            rayCurrentDistance = Mathf.Abs(Vector3.Distance(Player.Camera.transform.position, pointerPosition));
            Ray.transform.localScale = new Vector3(Ray.transform.localScale.x, Ray.transform.localScale.y, rayCurrentDistance);
            Ray.transform.localPosition = new Vector3(0, 0, rayCurrentDistance / 2);

            if (randomDeviation == false)
            {
                Ray.transform.parent.transform.localEulerAngles = new Vector3(-deviation.y, deviation.x, 0);
            }
            else
            {
                Ray.transform.parent.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
        else
        {
            // Calculate Center Position
            PointerDot.transform.position = Player.Camera.transform.position;
            PointerDot.transform.eulerAngles = new Vector3(Player.Camera.transform.eulerAngles.x, Player.Camera.transform.eulerAngles.y, 0);
            PointerDot.transform.Translate(new Vector3(0, 0, rayMaxDistance));
            pointerCenterPosition = PointerDot.transform.position;

            // Calculate Position
            PointerDot.transform.position = Player.Camera.transform.position;
            PointerDot.transform.eulerAngles = new Vector3(Player.Camera.transform.eulerAngles.x + -deviation.y, Player.Camera.transform.eulerAngles.y + deviation.x, 0);
            PointerDot.transform.Translate(new Vector3(0, 0, rayMaxDistance));

            // Ray Distance
            Ray.transform.localScale = new Vector3(Ray.transform.localScale.x, Ray.transform.localScale.y, rayMaxDistance);
            Ray.transform.localPosition = new Vector3(0, 0, rayMaxDistance / 2);

            if (randomDeviation == false)
            {
                Ray.transform.parent.transform.localEulerAngles = new Vector3(-deviation.y, deviation.x, 0);
            }
            else
            {
                Ray.transform.parent.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }

        // Move Pointer Dot
        PointerDot.transform.position = pointerPosition;
    }

    // Singular Shot
    public RaycastHit Shoot(float maxDistance, bool randomDeviation, Vector2 deviation, out Vector3 hitPosition, out Collider collision)
    {
        hitscan = Player.Camera.ScreenPointToRay(new Vector3(Player.Camera.pixelWidth / 2, Player.Camera.pixelHeight / 2 + deviation.y, 0));

        // Deviation
        if (randomDeviation)
        {
            hitscan.direction = Quaternion.Euler(Player.Camera.transform.eulerAngles + new Vector3(-Random.Range(-deviation.y, deviation.y), Random.Range(-deviation.x, deviation.x), 0)) * Vector3.forward;
        }
        else
        {
            hitscan.direction = Quaternion.Euler(Player.Camera.transform.eulerAngles + new Vector3(-deviation.y, deviation.x, 0)) * Vector3.forward;
        }

        // Check Hitscan
        if (Physics.Raycast(hitscan, out hitpoint, maxDistance))
        {
            // Calculate Pointer Hit Position and Collision
            hitPosition = hitpoint.point;
            collision = hitpoint.collider;
        }
        else
        {
            // Calculate Position
            PointerDot.transform.position = Player.Camera.transform.position;
            PointerDot.transform.eulerAngles = new Vector3(Player.Camera.transform.eulerAngles.x + -deviation.y, Player.Camera.transform.eulerAngles.y + deviation.x, 0);
            PointerDot.transform.Translate(new Vector3(0, 0, maxDistance));

            hitPosition = PointerDot.transform.position;
            collision = null;
        }

        return hitpoint;
    }

    // Multiple Shots at Once
    public RaycastHit[] ShootBurst(int totalShots, float maxDistance, bool randomDeviation, Vector2 deviation, out Vector3[] hitPositions, out Collider[] collisions)
    {
        RaycastHit[] hitpoints = new RaycastHit[totalShots];
        hitPositions = new Vector3[totalShots];
        collisions = new Collider[totalShots];

        for (int shotIndex = 0; shotIndex < totalShots; shotIndex++)
        {
            hitscan = Player.Camera.ScreenPointToRay(new Vector3(Player.Camera.pixelWidth / 2, Player.Camera.pixelHeight / 2 + deviation.y, 0));

            // Deviation
            if (randomDeviation)
            {
                hitscan.direction = Quaternion.Euler(Player.Camera.transform.eulerAngles + new Vector3(-Random.Range(-deviation.y, deviation.y), Random.Range(-deviation.x, deviation.x), 0)) * Vector3.forward;
            }
            else
            {
                hitscan.direction = Quaternion.Euler(Player.Camera.transform.eulerAngles + new Vector3(-deviation.y, deviation.x, 0)) * Vector3.forward;
            }

            // Check Hitscan
            if (Physics.Raycast(hitscan, out hitpoint, maxDistance))
            {
                // Calculate Pointer Hit Position and Collision
                hitPositions[shotIndex] = hitpoint.point;
                collisions[shotIndex] = hitpoint.collider;
            }
            else
            {
                // Calculate Position
                PointerDot.transform.position = Player.Camera.transform.position;
                PointerDot.transform.eulerAngles = new Vector3(Player.Camera.transform.eulerAngles.x + -deviation.y, Player.Camera.transform.eulerAngles.y + deviation.x, 0);
                PointerDot.transform.Translate(new Vector3(0, 0, maxDistance));

                hitPositions[shotIndex] = PointerDot.transform.position;
                collisions[shotIndex] = null;
            }

            hitpoints[shotIndex] = hitpoint;
        }

        return hitpoints;
    }
}