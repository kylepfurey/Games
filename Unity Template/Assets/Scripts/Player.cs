using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // References
    public PlayerInput Input;
    public Camera Camera;
    public Rigidbody Rigidbody;

    // Camera Variables
    public bool thirdPerson;
    public Vector3 cameraStart;
    public Vector3 cameraDistance;

    // INSERT VARIABLES

    void Start()        // Start
    {
        // INSERT SCRIPTS
    }

    void Update()       // Input and Position
    {
        // Camera Perspective
        Camera.transform.position = transform.position + cameraStart;

        if (thirdPerson)
        {
            Camera.transform.Translate(cameraDistance);
        }


        // INSERT SCRIPTS
    }

    void FixedUpdate()  // Physics and Rotation
    {
        // INSERT SCRIPTS
    }

    public bool Button(float input)
    {
        if (input > 0)
        {
            return true;
        }

        return false;
    }
}