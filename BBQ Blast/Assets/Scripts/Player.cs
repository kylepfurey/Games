using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // PUBLIC              is used for variables that have other scripts accessing it and need to be modified in the editor.
    // HIDE IN INSPECTOR   is used for variables that have other scripts accessing it and don't need to be modified in the editor.
    // SERIALIZED PRIVATE  is used for variables only used by this script that need to be modified in the editor.
    // PRIVATE             is used for variables only used by this script that don't need to be modified in the editor.


    // References
    public PlayerInput Input;
    public PlayerAnimation Animation;
    public Rigidbody Rigidbody;
    public Camera Camera;
    public Pointer Pointer;

    // Controls - CAPITALS are direct input values and behavior
    private Vector2 MOVE;

    private float LOOK_X;
    private float LOOK_Y;
    private float cameraRotationY;
    private float cameraRotationX;
    private float cameraYaw;

    private bool JUMP;
    private bool JUMP_UP;

    private bool DODGE;
    private bool DODGE_UP;
    [SerializeField] private float DODGE_COOLDOWN;
    [SerializeField] private float DODGE_COOLDOWN_RESET;
    private float DODGE_COOLDOWN_TIME;

    private bool CROUCH;
    private bool CROUCH_UP;
    [SerializeField] private float CROUCH_COOLDOWN;
    [SerializeField] private float CROUCH_COOLDOWN_RESET;
    private float CROUCH_COOLDOWN_TIME;

    private bool SHOOT;
    private bool SHOOT_UP;
    [SerializeField] private float SHOOT_COOLDOWN;
    [SerializeField] private float SHOOT_COOLDOWN_RESET;
    private float SHOOT_COOLDOWN_TIME;

    private bool AIM;
    private bool AIM_UP;
    [SerializeField] private float AIM_COOLDOWN;
    [SerializeField] private float AIM_COOLDOWN_RESET;
    private float AIM_COOLDOWN_TIME;

    private bool RELOAD;
    private bool RELOAD_UP;
    [SerializeField] private float RELOAD_COOLDOWN;
    [SerializeField] private float RELOAD_COOLDOWN_RESET;
    private float RELOAD_COOLDOWN_TIME;

    private bool WEAPON_LEFT;
    private bool WEAPON_LEFT_UP;
    [SerializeField] private float WEAPON_LEFT_COOLDOWN;
    [SerializeField] private float WEAPON_LEFT_COOLDOWN_RESET;
    private float WEAPON_LEFT_COOLDOWN_TIME;

    private bool WEAPON_RIGHT;
    private bool WEAPON_RIGHT_UP;
    [SerializeField] private float WEAPON_RIGHT_COOLDOWN;
    [SerializeField] private float WEAPON_RIGHT_COOLDOWN_RESET;
    private float WEAPON_RIGHT_COOLDOWN_TIME;

    private bool WEAPON_PREVIOUS;
    private bool WEAPON_PREVIOUS_UP;
    [SerializeField] private float WEAPON_PREVIOUS_COOLDOWN;
    [SerializeField] private float WEAPON_PREVIOUS_COOLDOWN_RESET;
    private float WEAPON_PREVIOUS_COOLDOWN_TIME;

    private bool PRIMARY;
    private bool PRIMARY_UP;

    private bool SECONDARY;
    private bool SECONDARY_UP;

    private bool MELEE;
    private bool MELEE_UP;

    private bool SCOREBOARD;

    private bool RESTART;
    private bool EXIT;

    // Movement Variables
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveSpeedModifier;
    private Vector3 movement;

    private bool isGrounded;
    private float airTime;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool canDoubleJump;
    private int currentJump;
    private int maxJumps;

    [SerializeField] private float airControl;
    private float airDeltaX;
    private float airDeltaZ;
    private Vector3 airVelocity;
    private Vector3 highestVelocity;
    [SerializeField] private float rotationVelocityCap;

    [SerializeField] private bool canBunnyHop;
    private bool bunnyHopFrame;
    [SerializeField] private float bunnyHopModifier;
    private bool hasBunnyHopped;
    [SerializeField] private float bunnyHopCap;

    // Camera Variables
    public bool thirdPerson;
    public Vector3 cameraStart;
    public Vector3 cameraDistance;
    private bool isMouse;
    [SerializeField] private bool forceController;
    [SerializeField] private float lookSpeedMouse;
    [SerializeField] private float lookSpeedX;
    [SerializeField] private float lookSpeedY;
    [SerializeField] private float lookSpeedModifier;
    [SerializeField] private bool flickStick;
    private float flickStickRotation;
    [SerializeField] private float flickStickDeadzone;

    // Weapon Variables
    public int weapon;

    // Player Variables
    private int playerNumber;
    public bool play;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        playerNumber = Input.playerIndex + 1;
    }

    private void Update()
    {
        if (play)
        {
            GetControls();
            Jumping();
            CameraPosition();
            DebugVelocity();
        }

        ExitGame();
    }

    private void FixedUpdate()
    {
        if (play)
        {
            Movement();
            CameraRotation();
            RotateVelocity();
        }
    }

    // Entering Collision
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            isGrounded = true;
            currentJump = 0;

            bunnyHopFrame = true;
        }
    }

    // In Collision
    void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            isGrounded = true;

            if (bunnyHopFrame)
            {
                bunnyHopFrame = false;
            }
        }
    }

    // Exiting Collision
    void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            isGrounded = false;

            if (canDoubleJump)
            {
                currentJump = 1;
            }

            LimitAirSpeed();
        }
    }

    private void GetControls()
    {
        // Controls
        MOVE = Input.actions.FindAction("Move").ReadValue<Vector2>();
        JUMP = Button(Input.actions.FindAction("Jump").ReadValue<float>());
        DODGE = Button(Input.actions.FindAction("Dodge").ReadValue<float>());
        CROUCH = Button(Input.actions.FindAction("Crouch").ReadValue<float>());
        SHOOT = Button(Input.actions.FindAction("Shoot").ReadValue<float>());
        AIM = Button(Input.actions.FindAction("Aim").ReadValue<float>());
        RELOAD = Button(Input.actions.FindAction("Reload").ReadValue<float>());
        WEAPON_LEFT = Button(Input.actions.FindAction("Weapon Left").ReadValue<float>());
        WEAPON_RIGHT = Button(Input.actions.FindAction("Weapon Right").ReadValue<float>());
        WEAPON_PREVIOUS = Button(Input.actions.FindAction("Previous Weapon").ReadValue<float>());
        PRIMARY = Button(Input.actions.FindAction("Primary").ReadValue<float>());
        SECONDARY = Button(Input.actions.FindAction("Secondary").ReadValue<float>());
        MELEE = Button(Input.actions.FindAction("Melee").ReadValue<float>());
        SCOREBOARD = Button(Input.actions.FindAction("Scoreboard").ReadValue<float>());


        // Button Behavior
        if (!JUMP)
        {
            JUMP_UP = true;
        }

        if (!DODGE)
        {
            DODGE_UP = true;
        }

        if (DODGE_COOLDOWN != 0)
        {
            if (DODGE_UP && DODGE_COOLDOWN_TIME > DODGE_COOLDOWN_RESET)
            {
                DODGE_COOLDOWN_TIME = DODGE_COOLDOWN_RESET;
            }

            if (DODGE_COOLDOWN_TIME > 0)
            {
                DODGE_COOLDOWN_TIME -= Time.deltaTime;
            }
            else
            {
                DODGE_COOLDOWN_TIME = 0;
            }
        }

        if (!CROUCH)
        {
            CROUCH_UP = true;
        }

        if (CROUCH_COOLDOWN != 0)
        {
            if (CROUCH_UP && CROUCH_COOLDOWN_TIME > CROUCH_COOLDOWN_RESET)
            {
                CROUCH_COOLDOWN_TIME = CROUCH_COOLDOWN_RESET;
            }

            if (CROUCH_COOLDOWN_TIME > 0)
            {
                CROUCH_COOLDOWN_TIME -= Time.deltaTime;
            }
            else
            {
                CROUCH_COOLDOWN_TIME = 0;
            }
        }

        if (!SHOOT)
        {
            SHOOT_UP = true;
        }

        if (SHOOT_COOLDOWN != 0)
        {
            if (SHOOT_UP && SHOOT_COOLDOWN_TIME > SHOOT_COOLDOWN_RESET)
            {
                SHOOT_COOLDOWN_TIME = SHOOT_COOLDOWN_RESET;
            }

            if (SHOOT_COOLDOWN_TIME > 0)
            {
                SHOOT_COOLDOWN_TIME -= Time.deltaTime;
            }
            else
            {
                SHOOT_COOLDOWN_TIME = 0;
            }
        }

        if (!AIM)
        {
            AIM_UP = true;
        }

        if (AIM_COOLDOWN != 0)
        {
            if (AIM_UP && AIM_COOLDOWN_TIME > AIM_COOLDOWN_RESET)
            {
                AIM_COOLDOWN_TIME = AIM_COOLDOWN_RESET;
            }

            if (AIM_COOLDOWN_TIME > 0)
            {
                AIM_COOLDOWN_TIME -= Time.deltaTime;
            }
            else
            {
                AIM_COOLDOWN_TIME = 0;
            }
        }

        if (!RELOAD)
        {
            RELOAD_UP = true;
        }

        if (RELOAD_COOLDOWN != 0)
        {
            if (RELOAD_UP && RELOAD_COOLDOWN_TIME > RELOAD_COOLDOWN_RESET)
            {
                RELOAD_COOLDOWN_TIME = RELOAD_COOLDOWN_RESET;
            }

            if (RELOAD_COOLDOWN_TIME > 0)
            {
                RELOAD_COOLDOWN_TIME -= Time.deltaTime;
            }
            else
            {
                RELOAD_COOLDOWN_TIME = 0;
            }
        }

        if (!WEAPON_LEFT)
        {
            WEAPON_LEFT_UP = true;
        }

        if (WEAPON_LEFT_COOLDOWN != 0)
        {
            if (WEAPON_LEFT_UP && WEAPON_LEFT_COOLDOWN_TIME > WEAPON_LEFT_COOLDOWN_RESET)
            {
                WEAPON_LEFT_COOLDOWN_TIME = WEAPON_LEFT_COOLDOWN_RESET;
            }

            if (WEAPON_LEFT_COOLDOWN_TIME > 0)
            {
                WEAPON_LEFT_COOLDOWN_TIME -= Time.deltaTime;
            }
            else
            {
                WEAPON_LEFT_COOLDOWN_TIME = 0;
            }
        }

        if (!WEAPON_RIGHT)
        {
            WEAPON_RIGHT_UP = true;
        }

        if (WEAPON_RIGHT_COOLDOWN != 0)
        {
            if (WEAPON_RIGHT_UP && WEAPON_RIGHT_COOLDOWN_TIME > WEAPON_RIGHT_COOLDOWN_RESET)
            {
                WEAPON_RIGHT_COOLDOWN_TIME = WEAPON_RIGHT_COOLDOWN_RESET;
            }

            if (WEAPON_RIGHT_COOLDOWN_TIME > 0)
            {
                WEAPON_RIGHT_COOLDOWN_TIME -= Time.deltaTime;
            }
            else
            {
                WEAPON_RIGHT_COOLDOWN_TIME = 0;
            }
        }

        if (!WEAPON_PREVIOUS)
        {
            WEAPON_PREVIOUS_UP = true;
        }

        if (WEAPON_PREVIOUS_COOLDOWN != 0)
        {
            if (WEAPON_PREVIOUS_UP && WEAPON_PREVIOUS_COOLDOWN_TIME > WEAPON_PREVIOUS_COOLDOWN_RESET)
            {
                WEAPON_PREVIOUS_COOLDOWN_TIME = WEAPON_PREVIOUS_COOLDOWN_RESET;
            }

            if (WEAPON_PREVIOUS_COOLDOWN_TIME > 0)
            {
                WEAPON_PREVIOUS_COOLDOWN_TIME -= Time.deltaTime;
            }
            else
            {
                WEAPON_PREVIOUS_COOLDOWN_TIME = 0;
            }
        }

        if (!PRIMARY)
        {
            PRIMARY_UP = true;
        }

        if (!SECONDARY)
        {
            SECONDARY_UP = true;
        }

        if (!MELEE)
        {
            MELEE_UP = true;
        }


        // Button Pressed

        // CROUCH_UP = false;
        // CROUCH_COOLDOWN_TIME = CROUCH_COOLDOWN;

        // SHOOT_UP = false;
        // SHOOT_COOLDOWN_TIME = SHOOT_COOLDOWN;

        // AIM_UP = false;
        // AIM_COOLDOWN_TIME = AIM_COOLDOWN;

        // WEAPON_LEFT_UP = false;
        // WEAPON_LEFT_COOLDOWN_TIME = WEAPON_RIGHT_COOLDOWN;

        // WEAPON_RIGHT_UP = false;
        // WEAPON_RIGHT_COOLDOWN_TIME = WEAPON_RIGHT_COOLDOWN;


        // Mouse and Controller Input
        if (Input.GetDevice<Mouse>() != null && forceController == false)
        {
            isMouse = true;

            // Mouse
            LOOK_X = Input.actions.FindAction("Look X").ReadValue<float>() * lookSpeedMouse * lookSpeedModifier;
            LOOK_Y = Input.actions.FindAction("Look Y").ReadValue<float>() * lookSpeedMouse * lookSpeedModifier;
        }
        else
        {
            isMouse = false;

            if (flickStick == false)
            {
                // Controller
                LOOK_X = Input.actions.FindAction("Look X").ReadValue<float>() * lookSpeedX * lookSpeedModifier * Time.deltaTime;
                LOOK_Y = Input.actions.FindAction("Look Y").ReadValue<float>() * lookSpeedY * lookSpeedModifier * Time.deltaTime;
            }
            else
            {
                // Controller (Flick Stick)
                LOOK_X = Input.actions.FindAction("Look X").ReadValue<float>();
                LOOK_Y = Input.actions.FindAction("Look Y").ReadValue<float>();
            }
        }


        // Movement Input
        Vector3 forward = MOVE.y * transform.forward * moveSpeed * moveSpeedModifier;
        Vector3 right = MOVE.x * transform.right * moveSpeed * moveSpeedModifier;
        movement = forward + right;
    }

    private void CameraPosition()
    {
        // Camera Position
        Camera.transform.position = transform.position + cameraStart;

        if (thirdPerson)
        {
            Camera.transform.Translate(cameraDistance);
        }


        // Camera Clamp
        cameraRotationX += LOOK_X;
        cameraRotationY -= LOOK_Y;
        cameraRotationY = Mathf.Clamp(cameraRotationY, -85, 85);
    }

    private void CameraRotation()
    {
        // Camera Rotation
        cameraYaw = Camera.transform.eulerAngles.y;

        if (flickStick == false)
        {
            Camera.transform.rotation = Quaternion.Euler(cameraRotationY, cameraRotationX, 0);
        }
        else
        {
            // Flick Stick
            if (Mathf.Abs(LOOK_X) >= flickStickDeadzone || Mathf.Abs(LOOK_Y) >= flickStickDeadzone)
            {
                Camera.transform.eulerAngles = new Vector3(0, flickStickRotation + Mathf.Atan2(LOOK_X, LOOK_Y) * Mathf.Rad2Deg, 0);
            }
            else
            {
                flickStickRotation = Camera.transform.eulerAngles.y;
            }
        }
    }

    private void Movement()
    {
        // Movement
        if (isGrounded)
        {
            // Ground Movement
            Rigidbody.velocity = new Vector3(movement.x, Rigidbody.velocity.y, movement.z);

            airDeltaX = 0;
            airDeltaZ = 0;
        }
        else
        {
            // Air Movement
            if (hasBunnyHopped == false)
            {
                airVelocity = new Vector3(Rigidbody.velocity.x - airDeltaX, Rigidbody.velocity.y, Rigidbody.velocity.z - airDeltaZ);

                airDeltaX = movement.x * airControl;
                airDeltaZ = movement.z * airControl;

                Rigidbody.velocity = airVelocity + new Vector3(airDeltaX, 0, airDeltaZ);
            }
            else
            {
                hasBunnyHopped = false;

                Rigidbody.velocity = new Vector3(Mathf.Clamp(airVelocity.x * bunnyHopModifier, -bunnyHopCap, bunnyHopCap), Rigidbody.velocity.y, Mathf.Clamp(airVelocity.z * bunnyHopModifier, -bunnyHopCap, bunnyHopCap));
            }
        }
    }

    private void Jumping()
    {
        // Double Jump
        if (canDoubleJump)
        {
            maxJumps = 2;
        }
        else
        {
            maxJumps = 1;
        }

        // Jumping
        if (JUMP && JUMP_UP && (isGrounded || (airTime <= coyoteTime && currentJump == 0) || (currentJump < maxJumps && canDoubleJump)))
        {
            JUMP_UP = false;

            // Bunny Hopping
            if (canBunnyHop && bunnyHopFrame)
            {
                hasBunnyHopped = true;
            }
            else
            {
                Rigidbody.velocity = new Vector3(movement.x, 0, movement.z);
            }

            Rigidbody.AddRelativeForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);

            airTime = 0;

            currentJump += 1;
        }

        // Airtime
        if (isGrounded)
        {
            airTime = 0;
        }
        else
        {
            airTime += Time.deltaTime;
        }
    }

    private void LimitAirSpeed()
    {
        // Limit Air Speed so players are not faster while jumping.
        Vector3 airSpeedControl = new Vector3(moveSpeed * moveSpeedModifier * airControl, 0, moveSpeed * moveSpeedModifier * airControl);

        if (Mathf.Abs(Rigidbody.velocity.x) >= airSpeedControl.x)
        {
            if (Rigidbody.velocity.x < 0)
            {
                airSpeedControl.x *= -1;
            }
        }
        else
        {
            airSpeedControl.x = Mathf.Abs(Rigidbody.velocity.x);

            if (Rigidbody.velocity.x < 0)
            {
                airSpeedControl.x *= -1;
            }
        }

        if (Mathf.Abs(Rigidbody.velocity.z) >= airSpeedControl.z)
        {
            if (Rigidbody.velocity.z < 0)
            {
                airSpeedControl.z *= -1;
            }
        }
        else
        {
            airSpeedControl.z = Mathf.Abs(Rigidbody.velocity.z);

            if (Rigidbody.velocity.z < 0)
            {
                airSpeedControl.z *= -1;
            }
        }

        Rigidbody.velocity -= airSpeedControl;
    }

    private void RotateVelocity()
    {
        // Rotate Velocity
        if (isGrounded == false && Camera.transform.eulerAngles.y - cameraYaw != 0 && airControl < 1)
        {
            Rigidbody.velocity = Quaternion.Euler(0, Camera.transform.eulerAngles.y - cameraYaw, 0) * Rigidbody.velocity;
            Rigidbody.velocity = new Vector3(Mathf.Clamp(Rigidbody.velocity.x, -rotationVelocityCap, rotationVelocityCap), Rigidbody.velocity.y, Mathf.Clamp(Rigidbody.velocity.z, -rotationVelocityCap, rotationVelocityCap));
        }
    }

    private void ExitGame()
    {
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

    private void DebugVelocity()
    {
        // Record the Greatest Velocity and Print It
        if (Mathf.Abs(Rigidbody.velocity.x) > highestVelocity.x)
        {
            highestVelocity = new Vector3(Mathf.Abs(Rigidbody.velocity.x), highestVelocity.y, highestVelocity.z);
        }

        if (Mathf.Abs(Rigidbody.velocity.y) > highestVelocity.y)
        {
            highestVelocity = new Vector3(highestVelocity.x, Mathf.Abs(Rigidbody.velocity.y), highestVelocity.z);
        }

        if (Mathf.Abs(Rigidbody.velocity.z) > highestVelocity.z)
        {
            highestVelocity = new Vector3(highestVelocity.x, highestVelocity.y, Mathf.Abs(Rigidbody.velocity.z));
        }

        print(highestVelocity);
    }

    private bool Button(float input)
    {
        if (input > 0)
        {
            return true;
        }

        return false;
    }
}