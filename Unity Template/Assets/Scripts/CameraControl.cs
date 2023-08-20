using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // References
    public Player Player;
    public Camera Camera;

    // Camera Variables
    public int currentState;
    [HideInInspector]
    public int totalStates;
    public bool cameraMoving = false;
    public CameraState[] cameraState;

    void Start()
    {
        totalStates = cameraState.Length;
    }

    void FixedUpdate()
    {
        switch (cameraState[currentState].cameraType)
        {
            case CameraState.CameraType.FixedCamera:
                FixedCameraMovement();
                break;

            case CameraState.CameraType.FollowCamera:
                FollowCameraMovement();
                break;
        }
    }

    public void FixedCameraMovement()
    {
        // Will the Camera move to its new Positon?
        if (cameraState[currentState].smoothTransition)
        {
            // Is the Camera close enough to its Final Position?
            if (Mathf.Abs(Vector3.Distance(Camera.transform.position, cameraState[currentState].fixedPosition)) < cameraState[currentState].transitionMovementSpeed)
            {
                // Set the Camera Position to its New Position and resume Player Movement
                Camera.transform.position = cameraState[currentState].fixedPosition;
            }

            // Is the Camera close enough to its Final Rotation?
            if ((Mathf.Abs(Vector3.Distance(Camera.transform.eulerAngles, cameraState[currentState].fixedRotation)) < cameraState[currentState].transitionRotationSpeed) && cameraState[currentState].lookAtPlayer == false)
            {
                // Set the Camera Rotation to its New Rotation and resume Player Movement
                Camera.transform.eulerAngles = cameraState[currentState].fixedRotation;
            }

            // Player's New Rotation and "Flipping"
            if (Mathf.Abs(Vector3.Distance(Camera.transform.position, cameraState[currentState].fixedPosition)) < cameraState[currentState].transitionMovementSpeed || Mathf.Abs(Vector3.Distance(Camera.transform.eulerAngles, cameraState[currentState].fixedRotation)) < cameraState[currentState].transitionRotationSpeed)
            {
                // Resume Player Movement
                cameraMoving = false;

                // Set the Player's new Rotation
                Player.transform.eulerAngles = cameraState[currentState].playerRotation;
            }
            else if (cameraState[currentState].flip && cameraState[currentState].lookAtPlayer == false)
            {
                // Halt Player Movement
                cameraMoving = true;

                // Flip Player
                Player.transform.eulerAngles = new Vector3(cameraState[currentState].playerRotation.x, Player.transform.eulerAngles.y + cameraState[currentState].flipSpeed, cameraState[currentState].playerRotation.z);
            }
            else
            {
                // Halt Player Movement
                cameraMoving = true;
            }


            // Glide the Camera to its new Position and Rotation
            Camera.transform.position = Vector3.MoveTowards(Camera.transform.position, cameraState[currentState].fixedPosition, cameraState[currentState].transitionMovementSpeed);

            if (cameraState[currentState].lookAtPlayer == false)
            {
                // Look at the new rotation
                Camera.transform.eulerAngles = Vector3.MoveTowards(Camera.transform.eulerAngles, cameraState[currentState].fixedRotation, cameraState[currentState].transitionRotationSpeed);
            }
            else
            {
                // Look at the Player
                transform.LookAt(Player.transform.position);
            }
        }
        else
        {
            // Set the Camera to its new Position and Rotation
            Camera.transform.position = cameraState[currentState].fixedPosition;

            if (cameraState[currentState].lookAtPlayer == false)
            {
                // Look at the new rotation
                Camera.transform.eulerAngles = Vector3.MoveTowards(Camera.transform.eulerAngles, cameraState[currentState].fixedRotation, cameraState[currentState].transitionRotationSpeed);
            }
            else
            {
                // Look at the Player
                transform.LookAt(Player.transform.position);
            }

            Player.transform.eulerAngles = cameraState[currentState].playerRotation;
        }
    }

    public void FollowCameraMovement()
    {
        // Calculated new Position and Rotation
        Vector3 finalPosition = new Vector3(Player.transform.position.x + cameraState[currentState].distance.x, Player.transform.position.y + cameraState[currentState].distance.y - Player.transform.position.y, Player.transform.position.z + cameraState[currentState].distance.z);
        Vector3 finalRotation = new Vector3(cameraState[currentState].angle.x, cameraState[currentState].angle.y, cameraState[currentState].angle.z);

        // Will the Camera move to its new Positon?
        if (cameraState[currentState].smoothTransition)
        {
            // Is the Camera close enough to its Final Position?
            if (Mathf.Abs(Vector3.Distance(Camera.transform.position, finalPosition)) < cameraState[currentState].transitionMovementSpeed)
            {
                // Set the Camera Position to its New Position and resume Player Movement
                Camera.transform.position = finalPosition;
            }

            // Is the Camera close enough to its Final Rotation?
            if (Mathf.Abs(Vector3.Distance(Camera.transform.eulerAngles, finalRotation)) < cameraState[currentState].transitionRotationSpeed)
            {
                // Set the Camera Rotation to its New Rotation and resume Player Movement
                Camera.transform.eulerAngles = finalRotation;
            }

            // Player's New Rotation and "Flipping"
            if (Mathf.Abs(Vector3.Distance(Camera.transform.position, finalPosition)) < cameraState[currentState].transitionMovementSpeed || Mathf.Abs(Vector3.Distance(Camera.transform.eulerAngles, finalRotation)) < cameraState[currentState].transitionRotationSpeed)
            {
                // Resume Player Movement
                cameraMoving = false;

                // Set the Player's new Rotation
                Player.transform.eulerAngles = cameraState[currentState].playerRotation;
            }
            else if (cameraState[currentState].flip)
            {
                // Halt Player Movement
                cameraMoving = true;

                // Flip Player
                Player.transform.eulerAngles = new Vector3(cameraState[currentState].playerRotation.x, Player.transform.eulerAngles.y + cameraState[currentState].flipSpeed, cameraState[currentState].playerRotation.z);
            }
            else
            {
                // Halt Player Movement
                cameraMoving = true;
            }


            // Glide the Camera to its new Position and Rotation
            Camera.transform.position = Vector3.MoveTowards(Camera.transform.position, finalPosition, cameraState[currentState].transitionMovementSpeed);
            Camera.transform.eulerAngles = Vector3.MoveTowards(Camera.transform.eulerAngles, finalRotation, cameraState[currentState].transitionRotationSpeed);
        }
        else
        {
            // Set the Camera to its new Position and Rotation
            Camera.transform.position = finalPosition;
            Camera.transform.eulerAngles = finalRotation;
            Player.transform.eulerAngles = cameraState[currentState].playerRotation;
        }
    }
}