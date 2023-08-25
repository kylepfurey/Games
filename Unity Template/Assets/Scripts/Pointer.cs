using UnityEngine;

public class Pointer : MonoBehaviour
{
    public Player Player;
    public GameObject Ray;  // Parent is Pivot
    public GameObject PointerDot;

    public Ray hitscan;
    public RaycastHit hitpoint;
    public float maxDistance;
    public float currentDistance;

    public Vector3 pointerPosition;
    public Vector3 pointerCenterPosition;

    public bool randomDeviation;
    public bool fixedDeviation;
    public Vector2 deviation;

    public int shotCounter;
    public int totalShots;

    void Update()
    {
        hitscan = Player.Camera.ScreenPointToRay(new Vector3(Player.Camera.pixelWidth / 2, Player.Camera.pixelHeight / 2 + deviation.y, 0));

        // Deviation
        if (randomDeviation)
        {
            hitscan.direction = Quaternion.Euler(Player.Camera.transform.eulerAngles + new Vector3(-Random.Range(-deviation.y, deviation.y), Random.Range(-deviation.x, deviation.x), 0)) * Vector3.forward;
        }
        else if (fixedDeviation)
        {
            float deviationX = 0;
            float deviationY = 0;

            // Bullet Pattern
            switch (shotCounter)
            {
                case 0:
                    deviationX = 0;
                    deviationY = 0;
                    shotCounter++;
                    break;
                case 1:
                    deviationX = 0;
                    deviationY = deviation.y;
                    shotCounter++;
                    break;
                case 2:
                    deviationX = deviation.x;
                    deviationY = 0;
                    shotCounter++;
                    break;
                case 3:
                    deviationX = 0;
                    deviationY = -deviation.y;
                    shotCounter++;
                    break;
                case 4:
                    deviationX = -deviation.x;
                    deviationY = 0;
                    shotCounter++;
                    break;
                case 5:
                    deviationX = -deviation.x;
                    deviationY = deviation.y;
                    shotCounter++;
                    break;
                case 6:
                    deviationX = deviation.x;
                    deviationY = deviation.y;
                    shotCounter++;
                    break;
                case 7:
                    deviationX = deviation.x;
                    deviationY = -deviation.y;
                    shotCounter++;
                    break;
                case 8:
                    deviationX = -deviation.x;
                    deviationY = -deviation.y;
                    shotCounter++;
                    break;
                case 9:
                    deviationX = 0;
                    deviationY = deviation.y / 2;
                    shotCounter++;
                    break;
                case 10:
                    deviationX = deviation.x / 2;
                    deviationY = 0;
                    shotCounter++;
                    break;
                case 11:
                    deviationX = 0;
                    deviationY = -deviation.y / 2;
                    shotCounter++;
                    break;
                case 12:
                    deviationX = -deviation.x / 2;
                    deviationY = 0;
                    shotCounter++;
                    break;
                case 13:
                    deviationX = -deviation.x / 2;
                    deviationY = deviation.y / 2;
                    shotCounter++;
                    break;
                case 14:
                    deviationX = deviation.x / 2;
                    deviationY = deviation.y / 2;
                    shotCounter++;
                    break;
                case 15:
                    deviationX = deviation.x / 2;
                    deviationY = -deviation.y / 2;
                    shotCounter++;
                    break;
                case 16:
                    deviationX = -deviation.x / 2;
                    deviationY = -deviation.y / 2;
                    shotCounter++;
                    break;
            }

            if (shotCounter > totalShots - 1)
            {
                shotCounter = 0;
            }

            hitscan.direction = Quaternion.Euler(Player.Camera.transform.eulerAngles + new Vector3(-deviationY, deviationX, 0)) * Vector3.forward;
        }
        else
        {
            hitscan.direction = Quaternion.Euler(Player.Camera.transform.eulerAngles + new Vector3(-deviation.y, deviation.x, 0)) * Vector3.forward;
        }

        // Check Hitscan
        if (Physics.Raycast(hitscan, out hitpoint, maxDistance))
        {
            // Calculate Center Position
            PointerDot.transform.position = Player.Camera.transform.position;
            PointerDot.transform.eulerAngles = new Vector3(Player.Camera.transform.eulerAngles.x, Player.Camera.transform.eulerAngles.y, 0);
            PointerDot.transform.Translate(new Vector3(0, 0, maxDistance));
            pointerCenterPosition = PointerDot.transform.position;

            // Calculate Pointer Hit Position
            pointerPosition = hitpoint.point;

            // Ray Distance
            currentDistance = Mathf.Abs(Vector3.Distance(Player.transform.position, pointerPosition));
            Ray.transform.localScale = new Vector3(Ray.transform.localScale.x, Ray.transform.localScale.y, currentDistance);
            Ray.transform.localPosition = new Vector3(0, 0, currentDistance / 2);

            if (randomDeviation == false && fixedDeviation == false)
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
            PointerDot.transform.Translate(new Vector3(0, 0, maxDistance));
            pointerCenterPosition = PointerDot.transform.position;

            // Calculate Position
            PointerDot.transform.position = Player.Camera.transform.position;
            PointerDot.transform.eulerAngles = new Vector3(Player.Camera.transform.eulerAngles.x + -deviation.y, Player.Camera.transform.eulerAngles.y + deviation.x, 0);
            PointerDot.transform.Translate(new Vector3(0, 0, maxDistance));
            pointerPosition = PointerDot.transform.position;

            // Ray Distance
            Ray.transform.localScale = new Vector3(Ray.transform.localScale.x, Ray.transform.localScale.y, maxDistance);
            Ray.transform.localPosition = new Vector3(0, 0, maxDistance / 2);

            if (randomDeviation == false && fixedDeviation == false)
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

        int shotCounter = 0;
        float deviationX = 0;
        float deviationY = 0;

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
                // Bullet Pattern
                switch (shotCounter)
                {
                    case 0:
                        deviationX = 0;
                        deviationY = 0;
                        shotCounter++;
                        break;
                    case 1:
                        deviationX = 0;
                        deviationY = deviation.y;
                        shotCounter++;
                        break;
                    case 2:
                        deviationX = deviation.x;
                        deviationY = 0;
                        shotCounter++;
                        break;
                    case 3:
                        deviationX = 0;
                        deviationY = -deviation.y;
                        shotCounter++;
                        break;
                    case 4:
                        deviationX = -deviation.x;
                        deviationY = 0;
                        shotCounter++;
                        break;
                    case 5:
                        deviationX = -deviation.x;
                        deviationY = deviation.y;
                        shotCounter++;
                        break;
                    case 6:
                        deviationX = deviation.x;
                        deviationY = deviation.y;
                        shotCounter++;
                        break;
                    case 7:
                        deviationX = deviation.x;
                        deviationY = -deviation.y;
                        shotCounter++;
                        break;
                    case 8:
                        deviationX = -deviation.x;
                        deviationY = -deviation.y;
                        shotCounter++;
                        break;
                    case 9:
                        deviationX = 0;
                        deviationY = deviation.y / 2;
                        shotCounter++;
                        break;
                    case 10:
                        deviationX = deviation.x / 2;
                        deviationY = 0;
                        shotCounter++;
                        break;
                    case 11:
                        deviationX = 0;
                        deviationY = -deviation.y / 2;
                        shotCounter++;
                        break;
                    case 12:
                        deviationX = -deviation.x / 2;
                        deviationY = 0;
                        shotCounter++;
                        break;
                    case 13:
                        deviationX = -deviation.x / 2;
                        deviationY = deviation.y / 2;
                        shotCounter++;
                        break;
                    case 14:
                        deviationX = deviation.x / 2;
                        deviationY = deviation.y / 2;
                        shotCounter++;
                        break;
                    case 15:
                        deviationX = deviation.x / 2;
                        deviationY = -deviation.y / 2;
                        shotCounter++;
                        break;
                    case 16:
                        deviationX = -deviation.x / 2;
                        deviationY = -deviation.y / 2;
                        shotCounter++;
                        break;
                }

                hitscan.direction = Quaternion.Euler(Player.Camera.transform.eulerAngles + new Vector3(-deviationY, deviationX, 0)) * Vector3.forward;
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