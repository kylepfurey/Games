using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // References
    [Header("References")]
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private Camera Camera;
    [SerializeField] private PlayerInput Input;

    // Player Settings
    [Header("Player Camera")]
    [SerializeField] private Vector2 cameraClamp = new Vector2(-85, 85);
    [SerializeField] private float mouseSensitivityX = 0.25f;
    [SerializeField] private float mouseSensitivityY = 0.25f;
    [SerializeField] private float controllerSensitivityX = 200;
    [SerializeField] private float controllerSensitivityY = 150;
    [SerializeField] private float landingLerpSpeed = 5;
    [SerializeField] private float landingCameraHeight = 1.25f;
    [SerializeField] private float startingRotationY = -50f;

    [Header("Player Speed")]
    [SerializeField] private float speed = 7.5f;
    [SerializeField] private float movementLerpSpeed = 10;
    [SerializeField] private float airControl = 0.5f;
    [SerializeField] private float gravity = 1000;

    [Header("Jumping Variables")]
    [SerializeField] private bool canJump = true;
    [SerializeField] private float jumpForce = 7.5f;
    [SerializeField] private float jumpCheckStart = 0.25f;
    [SerializeField] private float jumpCheckDepth = 0.5f;

    [Header("Crouching Variables")]
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool toggleCrouch = false;
    [SerializeField] private float crouchModifier = 0.5f;
    [SerializeField] private float crouchSpeed = 0.75f;
    [SerializeField] private float crouchTime = 0.1f;

    [Header("Grabbing Variables")]
    [SerializeField] private bool grabAll = true;
    [SerializeField] private float grabDistance = 2;
    [SerializeField] private float grabLerpDistance = 2;
    [SerializeField] private float grabLerpSpeed = 2;
    [SerializeField] private float grabMaxDistance = 2;
    [SerializeField] private float throwForce = 5;

    [Header("Mantling Variables")]
    [SerializeField] private bool mantleAll = false;
    [SerializeField] private float mantleCheckHeight = 0.75f;
    [SerializeField] private Vector2 mantleCheckSize = new Vector2(0.5f, 0.5f);
    [SerializeField] private float mantleCheckDistance = 0.75f;
    [SerializeField] private float mantleVisionHeight = 0.25f;
    [SerializeField] private float mantleVisionWidth = 0.75f;
    [SerializeField] private float mantleForceModifier = 1.1f;

    [Header("Footsteps Sounds")]
    [SerializeField] private AudioSource footStepsSound;
    [SerializeField] private AudioClip snowSound;
    [SerializeField] private AudioClip woodSound;
    [SerializeField] private AudioClip stoneSound;

    [Header("Landing Sounds")]
    [SerializeField] private AudioSource landingSound;
    [SerializeField] private AudioClip snowLandingSound;
    [SerializeField] private AudioClip woodLandingSound;
    [SerializeField] private AudioClip stoneLandingSound;

    [Header("Extra")]
    public bool oxygen;
    public bool helmet;
    public bool suit;
    public TextScroll endText;
    public GameObject endWall;
    public TextScroll winText;
    public GameObject winFade;

    // Input Variables
    private Vector2 MOVE;
    private Vector2 LOOK_MOUSE;
    private Vector2 LOOK_CONTROLLER;
    private bool JUMP;
    private bool JUMP_UP = true;
    private bool CROUCH;
    private bool CROUCH_UP = true;
    [HideInInspector] public bool INTERACT;
    [HideInInspector] public bool INTERACT_UP = true;
    private bool GRAB;
    private bool GRAB_UP = true;
    private bool THROW;
    private bool THROW_UP = true;
    private bool MANTLE;
    private bool RESTART;
    private bool EXIT;

    // Movement Variables
    private float currentSpeed;
    private bool isGrounded;
    private Vector3 groundVelocity;
    private Vector3 oldPosition;
    private Vector2 lookDelta;
    private bool isCrouching;
    private float startingScale;
    private int landing;
    private bool mantling;

    // Grabbing Variables
    private bool isHoldingObject;
    private GameObject grabbedObject;
    private Rigidbody grabbedRigidbody;
    private int grabbedLayer;

    // Start
    [HideInInspector] public bool start = true;
    [HideInInspector] public bool lose = false;

    private void Start()
    {
        startingScale = transform.localScale.y;

        isGrounded = true;

        lookDelta.x = startingRotationY;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (start && !lose)
        {
            ReadInputs();
            CameraInput();
            CameraPosition();
            CameraRotation();
            Jumping();
            Crouching();
            Interacting();
            GrabAndThrow();
            HoldingObject();
            Mantling();
        }

        Exit();
    }

    private void FixedUpdate()
    {
        if (start && !lose)
        {
            Movement();
            Gravity();
        }
    }

    private void ReadInputs()
    {
        // Read Inputs
        MOVE = Input.actions.FindAction("Move").ReadValue<Vector2>();
        LOOK_MOUSE = Input.actions.FindAction("Look Mouse").ReadValue<Vector2>();
        LOOK_CONTROLLER = Input.actions.FindAction("Look Controller").ReadValue<Vector2>();
        JUMP = Button(Input.actions.FindAction("Jump").ReadValue<float>());
        CROUCH = Button(Input.actions.FindAction("Crouch").ReadValue<float>());
        INTERACT = Button(Input.actions.FindAction("Interact").ReadValue<float>());
        GRAB = Button(Input.actions.FindAction("Grab").ReadValue<float>());
        THROW = Button(Input.actions.FindAction("Throw").ReadValue<float>());
        MANTLE = Button(Input.actions.FindAction("Mantle").ReadValue<float>());
        //RESTART = Button(Input.actions.FindAction("Restart").ReadValue<float>());
        EXIT = Button(Input.actions.FindAction("Exit").ReadValue<float>());

        if (!JUMP)
        {
            JUMP_UP = true;
        }

        if (!CROUCH)
        {
            CROUCH_UP = true;
        }

        if (!INTERACT)
        {
            INTERACT_UP = true;
        }

        if (!GRAB)
        {
            GRAB_UP = true;
        }

        if (!THROW)
        {
            THROW_UP = true;
        }
    }

    private void Movement()
    {
        // Movement
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Camera.transform.eulerAngles.y, transform.eulerAngles.z);

        float fallingVelocity = Rigidbody.velocity.y;

        if (isGrounded)
        {
            Vector3 targetVelocity = transform.forward * currentSpeed * MOVE.y + transform.right * currentSpeed * MOVE.x;

            Rigidbody.velocity = Vector3.Lerp(Rigidbody.velocity, targetVelocity, movementLerpSpeed * Time.unscaledDeltaTime);

            groundVelocity = Rigidbody.velocity;
        }
        else
        {
            Vector3 targetVelocity = groundVelocity - groundVelocity * airControl;

            targetVelocity += transform.forward * currentSpeed * MOVE.y * airControl + transform.right * currentSpeed * MOVE.x * airControl;

            Rigidbody.velocity = Vector3.Lerp(Rigidbody.velocity, targetVelocity, movementLerpSpeed * Time.unscaledDeltaTime);
        }

        Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, fallingVelocity, Rigidbody.velocity.z);


        // Footsteps
        if (isGrounded && (int)(Mathf.Abs(Rigidbody.velocity.x) + Mathf.Abs(Rigidbody.velocity.z)) > 0)
        {
            footStepsSound.mute = false;
        }
        else
        {
            footStepsSound.mute = true;
        }
    }

    private void Jumping()
    {
        Vector3 jumpStart = new Vector3(transform.position.x, transform.position.y - transform.lossyScale.y / 2 + jumpCheckStart, transform.position.z);

        // Landing
        if (!isGrounded && Physics.BoxCast(jumpStart, new Vector3(transform.lossyScale.x / 2, 0, transform.lossyScale.z / 2), -Vector3.up, Quaternion.identity, jumpCheckDepth, ~((1 << 3) | (1 << 6))))
        {
            landing = 1;

            if (!landingSound.isPlaying && footStepsSound.clip != null)
            {
                switch (footStepsSound.clip.name)
                {
                    case "Walking Snow":

                        landingSound.clip = snowLandingSound;

                        break;

                    case "Walking Wood":

                        landingSound.clip = woodLandingSound;

                        break;

                    case "Walking Stone":

                        landingSound.clip = stoneLandingSound;

                        break;
                }

                landingSound.Play();
            }
        }


        // Jumping
        isGrounded = Physics.BoxCast(jumpStart, new Vector3(transform.lossyScale.x / 2, 0, transform.lossyScale.z / 2), -Vector3.up, Quaternion.identity, jumpCheckDepth, ~((1 << 3) | (1 << 6)));

        if (JUMP && JUMP_UP && isGrounded && canJump && !mantling)
        {
            JUMP_UP = false;

            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, 0, Rigidbody.velocity.z);
            Rigidbody.AddForce(new Vector3(0, jumpForce * Rigidbody.mass, 0), ForceMode.Impulse);
        }
    }

    private void Gravity()
    {
        // Override Gravity
        if (!isGrounded)
        {
            Rigidbody.AddForce(new Vector3(0, -gravity, 0), ForceMode.Acceleration);
        }


        // Cap Jump Height
        if (!mantling)
        {
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, Mathf.Clamp(Rigidbody.velocity.y, -Mathf.Abs(Rigidbody.velocity.y), jumpForce), Rigidbody.velocity.z);
        }
        else
        {
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, Mathf.Clamp(Rigidbody.velocity.y, -Mathf.Abs(Rigidbody.velocity.y), jumpForce * mantleForceModifier), Rigidbody.velocity.z);
        }
    }

    private void StopSlide()
    {
        // Used to Stop the Sliding Bug
        if (isGrounded && Mathf.Abs(Vector3.Distance(transform.position, oldPosition)) < 0.05f && MOVE == Vector2.zero)
        {
            transform.position = oldPosition;
        }

        oldPosition = transform.position;
    }

    private void Crouching()
    {
        // Crouching
        if (CROUCH && CROUCH_UP && toggleCrouch && canCrouch)
        {
            CROUCH_UP = false;

            isCrouching = !isCrouching;
        }
        else if (CROUCH && !toggleCrouch && canCrouch)
        {
            CROUCH_UP = false;

            isCrouching = true;
        }
        else if (!toggleCrouch || !canCrouch)
        {
            isCrouching = false;
        }

        if ((isCrouching && canCrouch) || mantling)
        {
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(transform.localScale.y, startingScale * crouchModifier, crouchTime * Time.unscaledDeltaTime), transform.localScale.z);
            currentSpeed = Mathf.Lerp(currentSpeed, speed * crouchSpeed, crouchTime * Time.unscaledDeltaTime);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(transform.localScale.y, startingScale, crouchTime * Time.unscaledDeltaTime), transform.localScale.z);
            currentSpeed = Mathf.Lerp(currentSpeed, speed, crouchTime * Time.unscaledDeltaTime);
        }
    }

    private void Interacting()
    {
        // Interacting with Object
        if (INTERACT && INTERACT_UP)
        {
            // INTERACT_UP = false;

            // Interact Logic
        }
    }

    private void GrabAndThrow()
    {
        // Throwing Object
        if (THROW && THROW_UP && isHoldingObject && GRAB_UP && INTERACT_UP)
        {
            THROW_UP = false;

            DropObject(true);
        }

        // Grab Object
        if (GRAB && GRAB_UP && THROW_UP && INTERACT_UP)
        {
            GRAB_UP = false;

            GrabObject();
        }
    }

    private void GrabObject()
    {
        if (!isHoldingObject)
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.transform.position, Camera.transform.TransformDirection(Vector3.forward), out hit, grabDistance, ~((1 << 3) | (1 << 6))))
            {
                if (hit.rigidbody && ((hit.collider.gameObject.tag != "Ungrabbable" && grabAll) || (hit.collider.gameObject.tag == "Grabbable" && !grabAll)))
                {
                    if ((hit.rigidbody.velocity != Vector3.zero || hit.collider.gameObject.tag == "Grabbable" || !grabAll) && !hit.rigidbody.isKinematic)
                    {
                        grabbedObject = hit.collider.gameObject;
                        grabbedRigidbody = hit.rigidbody;

                        grabbedLayer = grabbedObject.layer;

                        grabbedObject.layer = 6;

                        isHoldingObject = true;
                    }
                }
            }
        }
        else
        {
            DropObject(false);
        }
    }

    private void HoldingObject()
    {
        if (isHoldingObject && grabbedObject != null & grabbedRigidbody != null)
        {
            Vector3 targetPosition = Camera.transform.position + Camera.transform.forward * grabLerpDistance;

            if (Mathf.Abs(Vector3.Distance(grabbedObject.transform.position, targetPosition)) < grabMaxDistance)
            {
                grabbedObject.transform.LookAt(Camera.transform.position);

                grabbedRigidbody.MovePosition(Vector3.Lerp(grabbedObject.transform.position, targetPosition, grabLerpSpeed * Time.unscaledDeltaTime));
                grabbedRigidbody.velocity = Vector3.zero;
            }
            else
            {
                DropObject(false);
            }
        }
        else if (isHoldingObject)
        {
            isHoldingObject = false;
        }
    }

    private void DropObject(bool throwObject)
    {
        grabbedObject.layer = grabbedLayer;

        if (throwObject)
        {
            grabbedRigidbody.AddForce(-grabbedObject.transform.forward * throwForce * grabbedRigidbody.mass, ForceMode.Impulse);

            if (grabbedObject.tag == "Grabbable")
            {
                grabbedRigidbody.useGravity = true;
            }
        }

        grabbedObject = null;
        grabbedRigidbody = null;

        isHoldingObject = false;
    }

    private void Mantling()
    {
        if (MANTLE && !mantling)
        {
            RaycastHit[] hits;

            Vector3 castCenter = new Vector3(transform.position.x, transform.position.y - transform.lossyScale.y / 2 + mantleCheckHeight, transform.position.z);
            Vector3 castSize = new Vector3(mantleCheckSize.x, mantleCheckSize.y, 0);
            Vector3 castHeight = new Vector3(Camera.transform.position.x, Camera.transform.position.y + mantleVisionHeight, Camera.transform.position.z);

            if (Physics.BoxCast(castCenter, castSize, transform.TransformDirection(Vector3.forward), Quaternion.identity, mantleCheckDistance, ~((1 << 3) | (1 << 6)))
                && !Physics.BoxCast(castHeight, new Vector3(mantleCheckSize.x * mantleVisionWidth, 0, 0), transform.forward, Quaternion.identity, mantleCheckDistance, ~((1 << 3) | (1 << 6))))
            {
                hits = Physics.BoxCastAll(castCenter, castSize, transform.forward, Quaternion.identity, mantleCheckDistance, ~((1 << 3) | (1 << 6)));

                bool successful = false;

                if (mantleAll)
                {
                    successful = true;

                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.collider.tag == "Unmantleable")
                        {
                            successful = false;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.collider.tag == "Mantleable")
                        {
                            successful = true;
                            break;
                        }
                    }
                }

                if (successful)
                {
                    JUMP_UP = false;
                    mantling = true;

                    Rigidbody.velocity = Vector3.zero;

                    Rigidbody.AddForce(new Vector3(0, jumpForce * Rigidbody.mass * mantleForceModifier, 0), ForceMode.Impulse);

                    groundVelocity = Rigidbody.velocity;
                }
            }
        }

        // Finished Mantling
        if (isGrounded && transform.lossyScale.y - crouchTime * Time.unscaledDeltaTime <= startingScale * crouchModifier)
        {
            mantling = false;
        }
    }

    private void CameraInput()
    {
        // Camera Input
        lookDelta += new Vector2(LOOK_MOUSE.x * mouseSensitivityX, -LOOK_MOUSE.y * mouseSensitivityY);
        lookDelta += new Vector2(LOOK_CONTROLLER.x * controllerSensitivityX * Time.unscaledDeltaTime, -LOOK_CONTROLLER.y * controllerSensitivityY * Time.unscaledDeltaTime);

        lookDelta.y = Mathf.Clamp(lookDelta.y, cameraClamp.x, cameraClamp.y);
    }

    private void CameraPosition()
    {
        // Camera Position
        if (landing == 0 || isCrouching)
        {
            Camera.transform.position = new Vector3(transform.position.x, transform.position.y + transform.lossyScale.y * 0.25f, transform.position.z);
            landing = 0;
        }
        else if (landing == 1)
        {
            Camera.transform.position = new Vector3(transform.position.x, Mathf.Lerp(Camera.transform.position.y, transform.position.y - transform.lossyScale.y * 0.5f + landingCameraHeight, landingLerpSpeed * Time.unscaledDeltaTime), transform.position.z);

            if (Camera.transform.position.y <= transform.position.y - transform.lossyScale.y * 0.5f + landingCameraHeight + landingLerpSpeed * Time.unscaledDeltaTime)
            {
                landing = 2;
            }
        }
        else if (landing == 2)
        {
            Camera.transform.position = new Vector3(transform.position.x, Mathf.Lerp(Camera.transform.position.y, transform.position.y + transform.lossyScale.y * 0.25f, landingLerpSpeed * Time.unscaledDeltaTime), transform.position.z);

            if (Camera.transform.position.y >= transform.position.y + transform.lossyScale.y * 0.25f - landingLerpSpeed * Time.unscaledDeltaTime)
            {
                landing = 0;
            }
        }
    }

    private void CameraRotation()
    {
        // Camera Rotation
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, lookDelta.x, transform.eulerAngles.z);
        Camera.transform.eulerAngles = new Vector3(lookDelta.y, transform.eulerAngles.y, Camera.transform.eulerAngles.z);
    }

    private void Exit()
    {
        // Restart
        if (RESTART)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Exit
        if (EXIT)
        {
            Application.Quit();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Footsteps
        if (collision.collider.tag == "Snow" && footStepsSound.clip != snowSound)
        {
            footStepsSound.clip = snowSound;
            footStepsSound.Play();
        }
        else if (collision.collider.tag == "Wood" && footStepsSound.clip != woodSound)
        {
            footStepsSound.clip = woodSound;
            footStepsSound.Play();
        }
        else if (collision.collider.tag == "Stone" && footStepsSound.clip != stoneSound)
        {
            footStepsSound.clip = stoneSound;
            footStepsSound.Play();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.rigidbody)
        {
            Rigidbody.MovePosition(transform.position + collision.rigidbody.velocity * Time.unscaledDeltaTime);
        }
        else
        {
            StopSlide();
        }
    }

    public static bool Button(float input)
    {
        if (Mathf.Abs(input) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool Button(float input, float threshold)
    {
        if (Mathf.Abs(input) > Mathf.Abs(threshold))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
