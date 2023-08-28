using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // References
    public PlayerInput Input;
    public PlayerAnimation Animation;
    public Rigidbody Rigidbody;
    public Camera Camera;
    public Pointer Pointer;

    // Controls
    public Vector2 MOVE;
    public float LOOK_X;
    public float LOOK_Y;
    public float cameraRotationY;
    public float cameraRotationX;

    public bool JUMP;
    public bool JUMP_UP;
    public float JUMP_COOLDOWN;
    public float JUMP_COOLDOWN_TIME;
    public float JUMP_COOLDOWN_RESET;

    public bool DODGE;
    public bool DODGE_UP;
    public float DODGE_COOLDOWN;
    public float DODGE_COOLDOWN_TIME;
    public float DODGE_COOLDOWN_RESET;

    public bool CROUCH;
    public bool CROUCH_UP;
    public float CROUCH_COOLDOWN;
    public float CROUCH_COOLDOWN_TIME;
    public float CROUCH_COOLDOWN_RESET;

    public bool SHOOT;
    public bool SHOOT_UP;
    public float SHOOT_COOLDOWN;
    public float SHOOT_COOLDOWN_TIME;
    public float SHOOT_COOLDOWN_RESET;

    public bool AIM;
    public bool AIM_UP;
    public float AIM_COOLDOWN;
    public float AIM_COOLDOWN_TIME;
    public float AIM_COOLDOWN_RESET;

    public bool RELOAD;
    public bool RELOAD_UP;
    public float RELOAD_COOLDOWN;
    public float RELOAD_COOLDOWN_TIME;
    public float RELOAD_COOLDOWN_RESET;

    public bool WEAPON_LEFT;
    public bool WEAPON_LEFT_UP;
    public float WEAPON_LEFT_COOLDOWN;
    public float WEAPON_LEFT_COOLDOWN_TIME;
    public float WEAPON_LEFT_COOLDOWN_RESET;

    public bool WEAPON_RIGHT;
    public bool WEAPON_RIGHT_UP;
    public float WEAPON_RIGHT_COOLDOWN;
    public float WEAPON_RIGHT_COOLDOWN_TIME;
    public float WEAPON_RIGHT_COOLDOWN_RESET;

    public bool WEAPON_PREVIOUS;
    public bool WEAPON_PREVIOUS_UP;
    public float WEAPON_PREVIOUS_COOLDOWN;
    public float WEAPON_PREVIOUS_COOLDOWN_TIME;
    public float WEAPON_PREVIOUS_COOLDOWN_RESET;

    public bool PRIMARY;
    public bool PRIMARY_UP;

    public bool SECONDARY;
    public bool SECONDARY_UP;

    public bool MELEE;
    public bool MELEE_UP;

    public bool SCOREBOARD;
    public bool RESTART;
    public bool EXIT;

    // Movement Variables
    public float moveSpeed;
    public float moveSpeedModifier;

    public float jumpForce;
    public bool isGrounded;
    public int currentJumps;
    public int maxJumps;
    public bool canDoubleJump;

    // Looking Variables
    public Vector3 cameraStart;
    public Vector3 cameraDistance;
    public bool isMouse;
    public float lookSpeedMouse;
    public float lookSpeedX;
    public float lookSpeedY;
    public float lookSpeedModifier;
    public bool flickStick;
    public float flickStickRotation;
    public float flickStickDeadzone;

    // Weapon Variables
    public int weapon;

    // Player Variables
    public int playerNumber;
    public bool play;

    void Start()
    {
        playerNumber = Input.playerIndex + 1;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        // Controls
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
        RESTART = Button(Input.actions.FindAction("Restart").ReadValue<float>());
        EXIT = Button(Input.actions.FindAction("Exit").ReadValue<float>());


        // Restart and Exit
        if (EXIT)
        {
            Application.Quit();
        }
        else if (RESTART && play)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


        // Button Behavior
        if (!JUMP)
        {
            JUMP_UP = true;

            if (JUMP_COOLDOWN_RESET != 0)
            {
                if (JUMP_COOLDOWN > JUMP_COOLDOWN_RESET)
                {
                    JUMP_COOLDOWN = JUMP_COOLDOWN_RESET;
                }

                if (JUMP_COOLDOWN > 0)
                {
                    JUMP_COOLDOWN -= Time.deltaTime;
                }
                else
                {
                    JUMP_COOLDOWN = 0;
                }
            }
        }

        if (!DODGE)
        {
            DODGE_UP = true;

            if (DODGE_COOLDOWN_RESET != 0)
            {
                if (DODGE_COOLDOWN > DODGE_COOLDOWN_RESET)
                {
                    DODGE_COOLDOWN = DODGE_COOLDOWN_RESET;
                }

                if (DODGE_COOLDOWN > 0)
                {
                    DODGE_COOLDOWN -= Time.deltaTime;
                }
                else
                {
                    DODGE_COOLDOWN = 0;
                }
            }
        }

        if (!CROUCH)
        {
            CROUCH_UP = true;

            if (CROUCH_COOLDOWN_RESET != 0)
            {
                if (CROUCH_COOLDOWN > DODGE_COOLDOWN_RESET)
                {
                    CROUCH_COOLDOWN = CROUCH_COOLDOWN_RESET;
                }

                if (CROUCH_COOLDOWN > 0)
                {
                    CROUCH_COOLDOWN -= Time.deltaTime;
                }
                else
                {
                    CROUCH_COOLDOWN = 0;
                }
            }
        }

        if (!SHOOT)
        {
            SHOOT_UP = true;

            if (SHOOT_COOLDOWN_RESET != 0)
            {
                if (SHOOT_COOLDOWN > SHOOT_COOLDOWN_RESET)
                {
                    SHOOT_COOLDOWN = SHOOT_COOLDOWN_RESET;
                }

                if (SHOOT_COOLDOWN > 0)
                {
                    SHOOT_COOLDOWN -= Time.deltaTime;
                }
                else
                {
                    SHOOT_COOLDOWN = 0;
                }
            }
        }

        if (!AIM)
        {
            AIM_UP = true;

            if (AIM_COOLDOWN_RESET != 0)
            {
                if (AIM_COOLDOWN > AIM_COOLDOWN_RESET)
                {
                    AIM_COOLDOWN = AIM_COOLDOWN_RESET;
                }

                if (AIM_COOLDOWN > 0)
                {
                    AIM_COOLDOWN -= Time.deltaTime;
                }
                else
                {
                    AIM_COOLDOWN = 0;
                }
            }
        }

        if (!RELOAD)
        {
            RELOAD_UP = true;

            if (RELOAD_COOLDOWN_RESET != 0)
            {
                if (RELOAD_COOLDOWN > RELOAD_COOLDOWN_RESET)
                {
                    RELOAD_COOLDOWN = RELOAD_COOLDOWN_RESET;
                }

                if (RELOAD_COOLDOWN > 0)
                {
                    RELOAD_COOLDOWN -= Time.deltaTime;
                }
                else
                {
                    RELOAD_COOLDOWN = 0;
                }
            }
        }

        if (!WEAPON_LEFT)
        {
            WEAPON_LEFT_UP = true;

            if (WEAPON_LEFT_COOLDOWN_RESET != 0)
            {
                if (WEAPON_LEFT_COOLDOWN > WEAPON_RIGHT_COOLDOWN_RESET)
                {
                    WEAPON_LEFT_COOLDOWN = WEAPON_LEFT_COOLDOWN_RESET;
                }

                if (WEAPON_LEFT_COOLDOWN > 0)
                {
                    WEAPON_LEFT_COOLDOWN -= Time.deltaTime;
                }
                else
                {
                    WEAPON_LEFT_COOLDOWN = 0;
                }
            }
        }

        if (!WEAPON_RIGHT)
        {
            WEAPON_RIGHT_UP = true;

            if (WEAPON_RIGHT_COOLDOWN_RESET != 0)
            {
                if (WEAPON_RIGHT_COOLDOWN > WEAPON_RIGHT_COOLDOWN_RESET)
                {
                    WEAPON_RIGHT_COOLDOWN = WEAPON_RIGHT_COOLDOWN_RESET;
                }

                if (WEAPON_RIGHT_COOLDOWN > 0)
                {
                    WEAPON_RIGHT_COOLDOWN -= Time.deltaTime;
                }
                else
                {
                    WEAPON_RIGHT_COOLDOWN = 0;
                }
            }
        }

        if (!WEAPON_PREVIOUS)
        {
            WEAPON_PREVIOUS_UP = true;

            if (WEAPON_PREVIOUS_COOLDOWN_RESET != 0)
            {
                if (WEAPON_PREVIOUS_COOLDOWN > WEAPON_PREVIOUS_COOLDOWN_RESET)
                {
                    WEAPON_PREVIOUS_COOLDOWN = WEAPON_PREVIOUS_COOLDOWN_RESET;
                }

                if (WEAPON_PREVIOUS_COOLDOWN > 0)
                {
                    WEAPON_PREVIOUS_COOLDOWN -= Time.deltaTime;
                }
                else
                {
                    WEAPON_PREVIOUS_COOLDOWN = 0;
                }
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
        // DODGE_UP = false;
        // DODGE_COOLDOWN = DODGE_COOLDOWN_TIME;

        // CROUCH_UP = false;
        // CROUCH_COOLDOWN = CROUCH_COOLDOWN_TIME;

        // SHOOT_UP = false;
        // SHOOT_COOLDOWN = SHOOT_COOLDOWN_TIME;

        // AIM_UP = false;
        // AIM_COOLDOWN = AIM_COOLDOWN_TIME;

        // WEAPON_LEFT_UP = false;
        // WEAPON_LEFT_COOLDOWN = WEAPON_RIGHT_COOLDOWN_TIME;

        // WEAPON_RIGHT_UP = false;
        // WEAPON_RIGHT_COOLDOWN = WEAPON_RIGHT_COOLDOWN_TIME;


        if (play)
        {
            // Movement
            MOVE = Input.actions.FindAction("Move").ReadValue<Vector2>();

            Vector3 forward = MOVE.y * Camera.transform.forward;
            Vector3 right = MOVE.x * Camera.transform.right;
            Vector3 movement = forward + right;

            if (isGrounded)
            {
                Rigidbody.velocity = new Vector3(movement.x * moveSpeed * moveSpeedModifier, Rigidbody.velocity.y, movement.z * moveSpeed * moveSpeedModifier);
            }


            // Camera
            Camera.transform.position = transform.position + cameraStart;
            Camera.transform.Translate(cameraDistance);

            Camera.transform.rotation = Quaternion.Euler(cameraRotationY, cameraRotationX, 0);


            // Flick Stick
            if (flickStick)
            {
                if (Mathf.Abs(LOOK_X) >= flickStickDeadzone || Mathf.Abs(LOOK_Y) >= flickStickDeadzone)
                {
                    Camera.transform.eulerAngles = new Vector3(0, flickStickRotation + Mathf.Atan2(LOOK_X, LOOK_Y) * Mathf.Rad2Deg, 0);
                }
                else
                {
                    flickStickRotation = Camera.transform.eulerAngles.y;
                }
            }


            // Jumping
            if (canDoubleJump)
            {
                maxJumps = 2;
            }
            else
            {
                maxJumps = 1;
            }

            if (JUMP && JUMP_UP && (isGrounded || (currentJumps < maxJumps && canDoubleJump)) && play)
            {
                JUMP_UP = false;
                Rigidbody.velocity = new Vector3(movement.x * moveSpeed * moveSpeedModifier, 0, movement.z * moveSpeed * moveSpeedModifier);
                Rigidbody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
                currentJumps += 1;
            }
        }
    }

    void Update()
    {
        if (play)
        {
            // Mouse and Controller Input
            if (Input.GetDevice<Mouse>() != null)
            {
                isMouse = true;

                // Mouse
                LOOK_X = Input.actions.FindAction("Look X").ReadValue<float>() * lookSpeedMouse * lookSpeedModifier * Time.smoothDeltaTime;
                LOOK_Y = Input.actions.FindAction("Look Y").ReadValue<float>() * lookSpeedMouse * lookSpeedModifier * Time.smoothDeltaTime;
            }
            else
            {
                isMouse = false;

                if (flickStick == false)
                {
                    // Controller
                    LOOK_X = Input.actions.FindAction("Look X").ReadValue<float>() * lookSpeedX * lookSpeedModifier * Time.smoothDeltaTime;
                    LOOK_Y = Input.actions.FindAction("Look Y").ReadValue<float>() * lookSpeedY * lookSpeedModifier * Time.smoothDeltaTime;
                }
                else
                {
                    // Controller (Flick Stick)
                    LOOK_X = Input.actions.FindAction("Look X").ReadValue<float>();
                    LOOK_Y = Input.actions.FindAction("Look Y").ReadValue<float>();
                }
            }


            // Camera Clamp
            cameraRotationX += LOOK_X;
            cameraRotationY -= LOOK_Y;
            cameraRotationY = Mathf.Clamp(cameraRotationY, -90, 90);
        }
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
            currentJumps = 0;
        }
    }

    // Exiting Collision
    void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            isGrounded = false;
            currentJumps = 1;
        }
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