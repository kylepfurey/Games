using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // References
    public PlayerInput Input;
    public Camera Camera;
    public Rigidbody Rigidbody;

    // Controls
    public bool RESTART;
    public bool EXIT;

    // Movement Variables
    public bool isGrounded;

    // Camera Variables
    public bool thirdPerson;
    public Vector2 cameraStart;
    public Vector2 cameraDistance;

    // INSERT VARIABLES

    // Player Variables
    public bool play;

    void Start()        // Start
    {
        // INSERT SCRIPTS
    }

    void Update()       // Input
    {
        if (play)
        {
            // INSERT SCRIPTS
        }


        // Restart and Exit
        RESTART = Button(Input.actions.FindAction("Restart").ReadValue<float>());
        EXIT = Button(Input.actions.FindAction("Exit").ReadValue<float>());

        if (EXIT)
        {
            Application.Quit();
        }
        else if (RESTART && play)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void LateUpdate()   // Position and Rotation
    {
        if (play)
        {
            // Camera Perspective
            Camera.transform.position = transform.position;

            if (thirdPerson)
            {
                Camera.transform.Translate(cameraDistance);
            }


            // INSERT SCRIPTS
        }
    }

    void FixedUpdate()  // Physics
    {
        // INSERT SCRIPTS
    }

    // Entering Collision
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    // In Collision
    void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    // Exiting Collision
    void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    // Entering Trigger
    void OnTriggerEnter(Collider other)
    {

    }

    // In Trigger
    void OnTriggerStay(Collider other)
    {

    }

    // Exiting Trigger
    void OnTriggerExit(Collider other)
    {

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