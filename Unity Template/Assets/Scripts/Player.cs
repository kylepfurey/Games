using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // References
    public PlayerInput Input;
    public Camera Camera;
    public Rigidbody Rigidbody;

    // Camera Variables
    public bool thirdPerson;
    public Vector2 cameraStart;
    public Vector2 cameraDistance;

    // Controls
    public bool RESTART;
    public bool EXIT;

    // Player Variables
    public bool play;

    // INSERT VARIABLES

    void Start()        // Start
    {
        // INSERT SCRIPTS
    }

    void Update()       // Input and Position
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

    void LateUpdate()   // Rotation
    {
        // INSERT SCRIPTS   
    }

    void FixedUpdate()  // Physics
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