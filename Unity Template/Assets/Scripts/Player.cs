using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // References
    public PlayerInput Input;
    public Camera Camera;
    public Rigidbody Rigidbody;

    // Camera Variables
    public Vector3 cameraStart;
    public Vector3 cameraDistance;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        // Move Camera
        Camera.transform.position = transform.position + cameraStart;
        Camera.transform.Translate(cameraDistance);


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