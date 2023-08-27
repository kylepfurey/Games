using UnityEngine;

public class Pointer : MonoBehaviour
{
    public Player Player;
    public GameObject Ray;  // Parent is Pivot
    public GameObject PointerDot;

    public Ray hitscan;
    public RaycastHit hitpoint;
    public Ray hitscanCamera;
    public RaycastHit hitpointCamera;
    public float maxDistance;

    public Vector3 pointerPosition;
    public Vector3 pointerCameraPosition;

    public bool randomDeviation;
    public bool fixedDeviation;
    public Vector2 deviation;

    public int shotCounter;
    public int totalShots;

    public float aimCorrectionDistance;
    public bool aimCorrection;

    void Update()
    {
        // Calculate Center Position of Camera
        Vector3 currentPosition = Player.Camera.transform.position;
        Player.Camera.transform.Translate(new Vector3(0, 0, -Player.cameraDistance.z + Player.transform.localScale.z / 2));
        hitscanCamera = Player.Camera.ScreenPointToRay(new Vector3(Player.Camera.pixelWidth / 2, Player.Camera.pixelHeight / 2, 0));
        Player.Camera.transform.position = currentPosition;

        if (Physics.Raycast(hitscanCamera, out hitpointCamera, maxDistance + -Player.cameraDistance.z))
        {
            pointerCameraPosition = hitpointCamera.point;
        }
        else
        {
            PointerDot.transform.position = Player.Camera.transform.position;
            PointerDot.transform.eulerAngles = new Vector3(Player.Camera.transform.eulerAngles.x, Player.Camera.transform.eulerAngles.y, 0);
            PointerDot.transform.Translate(new Vector3(0, 0, maxDistance + -Player.cameraDistance.z));
            pointerCameraPosition = PointerDot.transform.position;
        }


        // Hitscan
        currentPosition = Player.Camera.transform.position;
        Vector3 currentRotation = Player.Camera.transform.eulerAngles;
        Player.Camera.transform.position = Player.transform.position + Player.cameraStart;
        Player.Camera.transform.LookAt(pointerCameraPosition);
        Vector3 aimRotation = Player.Camera.transform.eulerAngles;
        hitscan = Player.Camera.ScreenPointToRay(new Vector3(Player.Camera.pixelWidth / 2, Player.Camera.pixelHeight / 2, 0));
        Player.Camera.transform.position = currentPosition;
        Player.Camera.transform.eulerAngles = currentRotation;


        // Deviation
        if (randomDeviation)
        {
            hitscan.direction = Quaternion.Euler(aimRotation + new Vector3(-Random.Range(-deviation.y, deviation.y), Random.Range(-deviation.x, deviation.x), 0)) * Vector3.forward;
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

            hitscan.direction = Quaternion.Euler(aimRotation + new Vector3(-deviationY, deviationX, 0)) * Vector3.forward;
        }
        else
        {
            hitscan.direction = Quaternion.Euler(aimRotation + new Vector3(-deviation.y, deviation.x, 0)) * Vector3.forward;
        }


        // Check Hitscan
        if (Physics.Raycast(hitscan, out hitpoint, maxDistance))
        {
            // Calculate Hit Position
            pointerPosition = hitpoint.point;

            // Render Ray
            Ray.transform.parent.position = hitscan.origin;
            Ray.transform.parent.transform.LookAt(pointerCameraPosition);
            Ray.transform.localScale = new Vector3(Ray.transform.localScale.x, Ray.transform.localScale.y, hitpoint.distance);
            Ray.transform.localPosition = new Vector3(0, 0, hitpoint.distance / 2);
        }
        else
        {
            // Calculate End Position
            pointerPosition = hitscan.GetPoint(maxDistance);

            // Render Ray
            Ray.transform.parent.position = hitscan.origin;
            Ray.transform.parent.transform.LookAt(pointerCameraPosition);
            Ray.transform.localScale = new Vector3(Ray.transform.localScale.x, Ray.transform.localScale.y, maxDistance);
            Ray.transform.localPosition = new Vector3(0, 0, maxDistance / 2);
        }


        // Smart Aim Correction
        if (aimCorrection && pointerPosition != pointerCameraPosition)
        {
            // Aim Correction Distance
            aimCorrectionDistance = hitpoint.distance + -Player.cameraDistance.z;

            if (aimCorrectionDistance == -Player.cameraDistance.z)
            {
                aimCorrectionDistance = maxDistance + -Player.cameraDistance.z;
            }


            // Calculate Center Position of Camera
            currentPosition = Player.Camera.transform.position;
            Player.Camera.transform.Translate(new Vector3(0, 0, -Player.cameraDistance.z + Player.transform.localScale.z / 2));
            hitscanCamera = Player.Camera.ScreenPointToRay(new Vector3(Player.Camera.pixelWidth / 2, Player.Camera.pixelHeight / 2, 0));
            Player.Camera.transform.position = currentPosition;


            if (Physics.Raycast(hitscanCamera, out hitpointCamera, aimCorrectionDistance))
            {
                pointerCameraPosition = hitpointCamera.point;
            }
            else
            {
                PointerDot.transform.position = Player.Camera.transform.position;
                PointerDot.transform.eulerAngles = new Vector3(Player.Camera.transform.eulerAngles.x, Player.Camera.transform.eulerAngles.y, 0);
                PointerDot.transform.Translate(new Vector3(0, 0, aimCorrectionDistance));
                pointerCameraPosition = PointerDot.transform.position;
            }


            // Hitscan
            currentPosition = Player.Camera.transform.position;
            currentRotation = Player.Camera.transform.eulerAngles;
            Player.Camera.transform.position = Player.transform.position + Player.cameraStart;
            Player.Camera.transform.LookAt(pointerCameraPosition);
            aimRotation = Player.Camera.transform.eulerAngles;
            hitscan = Player.Camera.ScreenPointToRay(new Vector3(Player.Camera.pixelWidth / 2, Player.Camera.pixelHeight / 2, 0));
            Player.Camera.transform.position = currentPosition;
            Player.Camera.transform.eulerAngles = currentRotation;


            // Deviation
            if (randomDeviation)
            {
                hitscan.direction = Quaternion.Euler(aimRotation + new Vector3(-Random.Range(-deviation.y, deviation.y), Random.Range(-deviation.x, deviation.x), 0)) * Vector3.forward;
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

                hitscan.direction = Quaternion.Euler(aimRotation + new Vector3(-deviationY, deviationX, 0)) * Vector3.forward;
            }
            else
            {
                hitscan.direction = Quaternion.Euler(aimRotation + new Vector3(-deviation.y, deviation.x, 0)) * Vector3.forward;
            }


            // Check Hitscan
            if (Physics.Raycast(hitscan, out hitpoint, maxDistance))
            {
                // Calculate Hit Position
                pointerPosition = hitpoint.point;

                // Render Ray
                Ray.transform.parent.position = hitscan.origin;
                Ray.transform.parent.transform.LookAt(pointerCameraPosition);
                Ray.transform.localScale = new Vector3(Ray.transform.localScale.x, Ray.transform.localScale.y, hitpoint.distance);
                Ray.transform.localPosition = new Vector3(0, 0, hitpoint.distance / 2);
            }
            else
            {
                // Calculate End Position
                pointerPosition = hitscan.GetPoint(maxDistance);

                // Render Ray
                Ray.transform.parent.position = hitscan.origin;
                Ray.transform.parent.transform.LookAt(pointerCameraPosition);
                Ray.transform.localScale = new Vector3(Ray.transform.localScale.x, Ray.transform.localScale.y, maxDistance);
                Ray.transform.localPosition = new Vector3(0, 0, maxDistance / 2);
            }


            // Move Pointer Dot
            PointerDot.transform.position = pointerCameraPosition;
        }
        else
        {
            // Move Pointer Dot
            PointerDot.transform.position = pointerPosition;
        }


        // Face Player
        Vector3 playerRotation = Player.transform.eulerAngles;
        Player.transform.LookAt(pointerPosition);
        Player.transform.eulerAngles = new Vector3(playerRotation.x, transform.eulerAngles.y, playerRotation.z);
    }



    // Singular Shot
    public RaycastHit Shoot(float maxDistance, Vector2 deviation, bool randomDeviation, bool aimCorrection, out Vector3 hitPosition, out Collider collision)
    {
        // Calculate Center Position of Camera
        Vector3 currentPosition = Player.Camera.transform.position;
        Player.Camera.transform.Translate(new Vector3(0, 0, -Player.cameraDistance.z + Player.transform.localScale.z / 2));
        hitscanCamera = Player.Camera.ScreenPointToRay(new Vector3(Player.Camera.pixelWidth / 2, Player.Camera.pixelHeight / 2, 0));
        Player.Camera.transform.position = currentPosition;

        if (Physics.Raycast(hitscanCamera, out hitpointCamera, maxDistance + -Player.cameraDistance.z))
        {
            pointerCameraPosition = hitpointCamera.point;
        }
        else
        {
            PointerDot.transform.position = Player.Camera.transform.position;
            PointerDot.transform.eulerAngles = new Vector3(Player.Camera.transform.eulerAngles.x, Player.Camera.transform.eulerAngles.y, 0);
            PointerDot.transform.Translate(new Vector3(0, 0, maxDistance + -Player.cameraDistance.z));
            pointerCameraPosition = PointerDot.transform.position;
        }


        // Hitscan
        currentPosition = Player.Camera.transform.position;
        Vector3 currentRotation = Player.Camera.transform.eulerAngles;
        Player.Camera.transform.position = Player.transform.position + Player.cameraStart;
        Player.Camera.transform.LookAt(pointerCameraPosition);
        Vector3 aimRotation = Player.Camera.transform.eulerAngles;
        hitscan = Player.Camera.ScreenPointToRay(new Vector3(Player.Camera.pixelWidth / 2, Player.Camera.pixelHeight / 2, 0));
        Player.Camera.transform.position = currentPosition;
        Player.Camera.transform.eulerAngles = currentRotation;


        // Deviation
        if (randomDeviation)
        {
            hitscan.direction = Quaternion.Euler(aimRotation + new Vector3(-Random.Range(-deviation.y, deviation.y), Random.Range(-deviation.x, deviation.x), 0)) * Vector3.forward;
        }
        else
        {
            hitscan.direction = Quaternion.Euler(aimRotation + new Vector3(-deviation.y, deviation.x, 0)) * Vector3.forward;
        }


        // Check Hitscan
        if (Physics.Raycast(hitscan, out hitpoint, maxDistance))
        {
            // Calculate Hit Position
            pointerPosition = hitpoint.point;
            hitPosition = hitpoint.point;
            collision = hitpoint.collider;
        }
        else
        {
            // Calculate End Position
            pointerPosition = hitscan.GetPoint(maxDistance);
            hitPosition = pointerCameraPosition;
            collision = null;
        }


        // Smart Aim Correction
        if (aimCorrection && pointerPosition != pointerCameraPosition)
        {
            // Aim Correction Distance
            aimCorrectionDistance = hitpoint.distance + -Player.cameraDistance.z;

            if (aimCorrectionDistance == -Player.cameraDistance.z)
            {
                aimCorrectionDistance = maxDistance + -Player.cameraDistance.z;
            }


            // Calculate Center Position of Camera
            currentPosition = Player.Camera.transform.position;
            Player.Camera.transform.Translate(new Vector3(0, 0, -Player.cameraDistance.z + Player.transform.localScale.z / 2));
            hitscanCamera = Player.Camera.ScreenPointToRay(new Vector3(Player.Camera.pixelWidth / 2, Player.Camera.pixelHeight / 2, 0));
            Player.Camera.transform.position = currentPosition;

            if (Physics.Raycast(hitscanCamera, out hitpointCamera, aimCorrectionDistance))
            {
                pointerCameraPosition = hitpointCamera.point;
            }
            else
            {
                PointerDot.transform.position = Player.Camera.transform.position;
                PointerDot.transform.eulerAngles = new Vector3(Player.Camera.transform.eulerAngles.x, Player.Camera.transform.eulerAngles.y, 0);
                PointerDot.transform.Translate(new Vector3(0, 0, aimCorrectionDistance));
                pointerCameraPosition = PointerDot.transform.position;
            }


            // Hitscan
            currentPosition = Player.Camera.transform.position;
            currentRotation = Player.Camera.transform.eulerAngles;
            Player.Camera.transform.position = Player.transform.position + Player.cameraStart;
            Player.Camera.transform.LookAt(pointerCameraPosition);
            aimRotation = Player.Camera.transform.eulerAngles;
            hitscan = Player.Camera.ScreenPointToRay(new Vector3(Player.Camera.pixelWidth / 2, Player.Camera.pixelHeight / 2, 0));
            Player.Camera.transform.position = currentPosition;
            Player.Camera.transform.eulerAngles = currentRotation;


            // Deviation
            if (randomDeviation)
            {
                hitscan.direction = Quaternion.Euler(aimRotation + new Vector3(-Random.Range(-deviation.y, deviation.y), Random.Range(-deviation.x, deviation.x), 0)) * Vector3.forward;
            }
            else
            {
                hitscan.direction = Quaternion.Euler(aimRotation + new Vector3(-deviation.y, deviation.x, 0)) * Vector3.forward;
            }


            // Check Hitscan
            if (Physics.Raycast(hitscan, out hitpoint, maxDistance))
            {
                // Calculate Hit Position
                pointerPosition = hitpoint.point;
                hitPosition = hitpoint.point;
                collision = hitpoint.collider;
            }
            else
            {
                // Calculate End Position
                pointerPosition = hitscan.GetPoint(maxDistance);
                hitPosition = pointerCameraPosition;
                collision = null;
            }
        }


        // Return Hit Data
        return hitpoint;
    }



    // Multiple Shots at Once
    public RaycastHit[] ShootBurst(int totalShots, float maxDistance, Vector2 deviation, bool randomDeviation, bool aimCorrection, out Vector3[] hitPositions, out Collider[] collisions)
    {
        RaycastHit[] hitpoints = new RaycastHit[totalShots];
        hitPositions = new Vector3[totalShots];
        collisions = new Collider[totalShots];

        for (int currentShot = 0; currentShot < totalShots; currentShot++)
        {
            // Calculate Center Position of Camera
            Vector3 currentPosition = Player.Camera.transform.position;
            Player.Camera.transform.Translate(new Vector3(0, 0, -Player.cameraDistance.z + Player.transform.localScale.z / 2));
            hitscanCamera = Player.Camera.ScreenPointToRay(new Vector3(Player.Camera.pixelWidth / 2, Player.Camera.pixelHeight / 2, 0));
            Player.Camera.transform.position = currentPosition;

            if (Physics.Raycast(hitscanCamera, out hitpointCamera, maxDistance + -Player.cameraDistance.z))
            {
                pointerCameraPosition = hitpointCamera.point;
            }
            else
            {
                PointerDot.transform.position = Player.Camera.transform.position;
                PointerDot.transform.eulerAngles = new Vector3(Player.Camera.transform.eulerAngles.x, Player.Camera.transform.eulerAngles.y, 0);
                PointerDot.transform.Translate(new Vector3(0, 0, maxDistance + -Player.cameraDistance.z));
                pointerCameraPosition = PointerDot.transform.position;
            }


            // Hitscan
            currentPosition = Player.Camera.transform.position;
            Vector3 currentRotation = Player.Camera.transform.eulerAngles;
            Player.Camera.transform.position = Player.transform.position + Player.cameraStart;
            Player.Camera.transform.LookAt(pointerCameraPosition);
            Vector3 aimRotation = Player.Camera.transform.eulerAngles;
            hitscan = Player.Camera.ScreenPointToRay(new Vector3(Player.Camera.pixelWidth / 2, Player.Camera.pixelHeight / 2, 0));
            Player.Camera.transform.position = currentPosition;
            Player.Camera.transform.eulerAngles = currentRotation;


            // Deviation
            if (randomDeviation)
            {
                hitscan.direction = Quaternion.Euler(aimRotation + new Vector3(-Random.Range(-deviation.y, deviation.y), Random.Range(-deviation.x, deviation.x), 0)) * Vector3.forward;
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

                hitscan.direction = Quaternion.Euler(aimRotation + new Vector3(-deviationY, deviationX, 0)) * Vector3.forward;
            }
            else
            {
                hitscan.direction = Quaternion.Euler(aimRotation + new Vector3(-deviation.y, deviation.x, 0)) * Vector3.forward;
            }


            // Check Hitscan
            if (Physics.Raycast(hitscan, out hitpoint, maxDistance))
            {
                // Calculate Hit Position
                pointerPosition = hitpoint.point;
                hitPositions[currentShot] = hitpoint.point;
                collisions[currentShot] = hitpoint.collider;
            }
            else
            {
                // Calculate End Position
                pointerPosition = hitscan.GetPoint(maxDistance);
                hitPositions[currentShot] = pointerCameraPosition;
                collisions[currentShot] = null;
            }


            // Smart Aim Correction
            if (aimCorrection && pointerPosition != pointerCameraPosition)
            {
                // Aim Correction Distance
                aimCorrectionDistance = hitpoint.distance + -Player.cameraDistance.z;

                if (aimCorrectionDistance == -Player.cameraDistance.z)
                {
                    aimCorrectionDistance = maxDistance + -Player.cameraDistance.z;
                }


                // Calculate Center Position of Camera
                currentPosition = Player.Camera.transform.position;
                Player.Camera.transform.Translate(new Vector3(0, 0, -Player.cameraDistance.z + Player.transform.localScale.z / 2));
                hitscanCamera = Player.Camera.ScreenPointToRay(new Vector3(Player.Camera.pixelWidth / 2, Player.Camera.pixelHeight / 2, 0));
                Player.Camera.transform.position = currentPosition;

                if (Physics.Raycast(hitscanCamera, out hitpointCamera, aimCorrectionDistance))
                {
                    pointerCameraPosition = hitpointCamera.point;
                }
                else
                {
                    PointerDot.transform.position = Player.Camera.transform.position;
                    PointerDot.transform.eulerAngles = new Vector3(Player.Camera.transform.eulerAngles.x, Player.Camera.transform.eulerAngles.y, 0);
                    PointerDot.transform.Translate(new Vector3(0, 0, aimCorrectionDistance));
                    pointerCameraPosition = PointerDot.transform.position;
                }


                // Hitscan
                currentPosition = Player.Camera.transform.position;
                currentRotation = Player.Camera.transform.eulerAngles;
                Player.Camera.transform.position = Player.transform.position + Player.cameraStart;
                Player.Camera.transform.LookAt(pointerCameraPosition);
                aimRotation = Player.Camera.transform.eulerAngles;
                hitscan = Player.Camera.ScreenPointToRay(new Vector3(Player.Camera.pixelWidth / 2, Player.Camera.pixelHeight / 2, 0));
                Player.Camera.transform.position = currentPosition;
                Player.Camera.transform.eulerAngles = currentRotation;


                // Deviation
                if (randomDeviation)
                {
                    hitscan.direction = Quaternion.Euler(aimRotation + new Vector3(-Random.Range(-deviation.y, deviation.y), Random.Range(-deviation.x, deviation.x), 0)) * Vector3.forward;
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

                    hitscan.direction = Quaternion.Euler(aimRotation + new Vector3(-deviationY, deviationX, 0)) * Vector3.forward;
                }
                else
                {
                    hitscan.direction = Quaternion.Euler(aimRotation + new Vector3(-deviation.y, deviation.x, 0)) * Vector3.forward;
                }


                // Check Hitscan
                if (Physics.Raycast(hitscan, out hitpoint, maxDistance))
                {
                    // Calculate Hit Position
                    pointerPosition = hitpoint.point;
                    hitPositions[currentShot] = hitpoint.point;
                    collisions[currentShot] = hitpoint.collider;
                }
                else
                {
                    // Calculate End Position
                    pointerPosition = hitscan.GetPoint(maxDistance);
                    hitPositions[currentShot] = pointerCameraPosition;
                    collisions[currentShot] = null;
                }
            }
        }


        // Return Hit Data
        return hitpoints;
    }
}