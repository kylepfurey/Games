
// Player Movement Template Script
// Made for educational purposes.
// Copyright © 2023 by Kyle Furey

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Drop onto any object in a 3D Unity game to generate a Player!")]

    // Player Settings
    [Header("Third Person Variables")]
    [SerializeField] private bool thirdPerson = true;
    [SerializeField] private Vector3 thirdPersonCameraDistance = new Vector3(0, 0, -5);

    [Header("Rotation Variables")]
    [SerializeField] private bool playerRotatesToCamera = false;
    [SerializeField] private float playerRotateSpeed = 10;
    [SerializeField] private Vector2 firstPersonCameraClamp = new Vector2(-85, 85);
    [SerializeField] private Vector2 thirdPersonCameraClamp = new Vector2(0, 85);

    [Header("Mouse Sensitivity")]
    [SerializeField] private float mouseSensitivityX = 3;
    [SerializeField] private float mouseSensitivityY = 3;
    [SerializeField] private bool invertX = false;
    [SerializeField] private bool invertY = false;

    [Header("Player Speed")]
    [SerializeField] private float speed = 7.5f;
    [SerializeField] private float airControl = 0.5f;

    [Header("Jumping Variables")]
    [SerializeField] private bool canJump = true;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private float jumpCheckHeight = 0.25f;
    [SerializeField] private float jumpCheckDepth = 0.25f;
    [SerializeField] private Vector3 gravity = new Vector3(0, -9.8f, 0);

    [Header("Sprinting Variables")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool toggleSprint = false;
    [SerializeField] private float sprintModifer = 1.5f;
    [SerializeField] private float sprintAcceleration = 20;

    [Header("Crouching Variables")]
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool toggleCrouch = false;
    [SerializeField] private float crouchModifier = 0.5f;
    [SerializeField] private float crouchSpeed = 0.75f;
    [SerializeField] private float crouchTime = 20;

    // Variables
    private Rigidbody Rigidbody;
    private Camera Camera;
    private float currentSpeed;
    private bool isGrounded;
    private Vector3 groundVelocity;
    private Vector2 mouseDelta;
    private bool isSprinting;
    private bool isCrouching;
    private float startingScale;

    private void Start()
    {
        if (!GetComponent<Collider>())
        {
            gameObject.AddComponent<CapsuleCollider>();
        }

        if (GetComponent<Rigidbody>())
        {
            Rigidbody = GetComponent<Rigidbody>();
        }
        else
        {
            Rigidbody = gameObject.AddComponent<Rigidbody>();
        }

        if (FindObjectOfType<Camera>())
        {
            Camera = FindObjectOfType<Camera>();
        }
        else
        {
            Camera = new GameObject().AddComponent<Camera>();
        }

        Rigidbody.constraints = ~RigidbodyConstraints.FreezeAll | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        name = "Player";
        tag = "Player";
        Camera.name = "Player Camera";
        Camera.tag = "MainCamera";

        currentSpeed = speed;
        startingScale = transform.localScale.y;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Camera Position
        if (!thirdPerson)
        {
            Camera.transform.parent = transform;
            Camera.transform.localPosition = new Vector3(0, transform.lossyScale.y / 8, 0);
        }
        else
        {
            Camera.transform.parent = null;
            Camera.transform.position = transform.position;
            Camera.transform.Translate(Camera.transform.rotation * new Vector3(0, transform.lossyScale.y / 8, 0) + thirdPersonCameraDistance);
        }


        // Camera Input
        if (!invertX)
        {
            mouseDelta += new Vector2(Input.GetAxis("Mouse X") * mouseSensitivityX, 0);
        }
        else
        {
            mouseDelta -= new Vector2(Input.GetAxis("Mouse X") * mouseSensitivityX, 0);
        }

        if (!invertY)
        {
            mouseDelta += new Vector2(0, -Input.GetAxis("Mouse Y") * mouseSensitivityY);
        }
        else
        {
            mouseDelta -= new Vector2(0, -Input.GetAxis("Mouse Y") * mouseSensitivityY);
        }

        if (!thirdPerson)
        {
            mouseDelta.y = Mathf.Clamp(mouseDelta.y, firstPersonCameraClamp.x, firstPersonCameraClamp.y);
        }
        else
        {
            mouseDelta.y = Mathf.Clamp(mouseDelta.y, thirdPersonCameraClamp.x, thirdPersonCameraClamp.y);
        }


        // Jumping
        Physics.gravity = gravity;

        isGrounded = Physics.BoxCast(new Vector3(transform.position.x, transform.position.y - transform.lossyScale.y / 2 + jumpCheckHeight, transform.position.z), new Vector3(transform.lossyScale.x / 2, 0, transform.lossyScale.z / 2), -Vector3.up, Quaternion.identity, jumpCheckHeight + jumpCheckDepth);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, 0, Rigidbody.velocity.z);
            Rigidbody.AddForce(new Vector3(0, jumpForce * Rigidbody.mass, 0), ForceMode.Impulse);
        }


        // Sprinting
        if (Input.GetKeyDown(KeyCode.LeftShift) && toggleSprint && canSprint)
        {
            isSprinting = !isSprinting;
        }
        else if (Input.GetKey(KeyCode.LeftShift) && !toggleSprint && canSprint)
        {
            isSprinting = true;
        }
        else if (!toggleSprint || !canSprint)
        {
            isSprinting = false;
        }

        if (isSprinting && canSprint && !isCrouching)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, speed * sprintModifer, sprintAcceleration * Time.deltaTime);
        }
        else if (isCrouching)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, speed * crouchSpeed, crouchTime * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, speed, sprintAcceleration * Time.deltaTime);
        }


        // Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl) && toggleCrouch && canCrouch)
        {
            isCrouching = !isCrouching;
        }
        else if (Input.GetKey(KeyCode.LeftControl) && !toggleCrouch && canCrouch)
        {
            isCrouching = true;
        }
        else if (!toggleCrouch || !canCrouch)
        {
            isCrouching = false;
        }

        if (isCrouching && canCrouch)
        {
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(transform.localScale.y, startingScale * crouchModifier, crouchTime * Time.deltaTime), transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(transform.localScale.y, startingScale, crouchTime * Time.deltaTime), transform.localScale.z);
        }


        // Restart and Exit
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void FixedUpdate()
    {
        // Movement
        float fallingVelocity = Rigidbody.velocity.y;
        Rigidbody.velocity = Vector3.zero;

        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (isGrounded)
        {
            Rigidbody.velocity = transform.forward * currentSpeed * movement.y + transform.right * currentSpeed * movement.x;

            groundVelocity = Rigidbody.velocity;
        }
        else
        {
            Rigidbody.velocity = groundVelocity - groundVelocity * airControl;

            Rigidbody.velocity += transform.forward * currentSpeed * movement.y * airControl + transform.right * currentSpeed * movement.x * airControl;
        }

        Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, fallingVelocity, Rigidbody.velocity.z);


        // Player Rotation
        if (!thirdPerson || playerRotatesToCamera || movement.magnitude > 0)
        {
            RotatePlayer();
        }


        // Camera Rotation
        if (!thirdPerson)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, mouseDelta.x, transform.eulerAngles.z);
            Camera.transform.eulerAngles = new Vector3(mouseDelta.y, transform.eulerAngles.y, Camera.transform.eulerAngles.z);
        }
        else
        {
            Camera.transform.eulerAngles = new Vector3(mouseDelta.y, mouseDelta.x, transform.eulerAngles.z);
        }
    }

    private void RotatePlayer()
    {
        if (thirdPerson)
        {
            Vector3 playerRotation = transform.eulerAngles;
            Vector3 cameraRotation = Camera.transform.eulerAngles;

            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            Camera.transform.eulerAngles = new Vector3(0, Camera.transform.eulerAngles.y, 0);

            transform.rotation = Quaternion.Lerp(transform.rotation, Camera.transform.rotation, playerRotateSpeed * Time.deltaTime);

            transform.eulerAngles = new Vector3(playerRotation.x, transform.eulerAngles.y, playerRotation.z);
            Camera.transform.eulerAngles = cameraRotation;
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Camera.transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
}
