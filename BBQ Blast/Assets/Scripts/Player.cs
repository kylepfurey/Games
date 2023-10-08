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
    public float cameraYaw;

    public bool JUMP;
    public bool JUMP_UP;

    public bool DODGE;
    public bool DODGE_UP;
    public float DODGE_COOLDOWN_TIME;
    public float DODGE_COOLDOWN_RESET;
    public float DODGE_COOLDOWN;

    public bool CROUCH;
    public bool CROUCH_UP;
    public float CROUCH_COOLDOWN_TIME;
    public float CROUCH_COOLDOWN_RESET;
    public float CROUCH_COOLDOWN;

    public bool SHOOT;
    public bool SHOOT_UP;
    public float SHOOT_COOLDOWN_TIME;
    public float SHOOT_COOLDOWN_RESET;
    public float SHOOT_COOLDOWN;

    public bool AIM;
    public bool AIM_UP;
    public float AIM_COOLDOWN_TIME;
    public float AIM_COOLDOWN_RESET;
    public float AIM_COOLDOWN;

    public bool RELOAD;
    public bool RELOAD_UP;
    public float RELOAD_COOLDOWN_TIME;
    public float RELOAD_COOLDOWN_RESET;
    public float RELOAD_COOLDOWN;

    public bool WEAPON_LEFT;
    public bool WEAPON_LEFT_UP;
    public float WEAPON_LEFT_COOLDOWN_TIME;
    public float WEAPON_LEFT_COOLDOWN_RESET;
    public float WEAPON_LEFT_COOLDOWN;

    public bool WEAPON_RIGHT;
    public bool WEAPON_RIGHT_UP;
    public float WEAPON_RIGHT_COOLDOWN_TIME;
    public float WEAPON_RIGHT_COOLDOWN_RESET;
    public float WEAPON_RIGHT_COOLDOWN;

    public bool WEAPON_PREVIOUS;
    public bool WEAPON_PREVIOUS_UP;
    public float WEAPON_PREVIOUS_COOLDOWN_TIME;
    public float WEAPON_PREVIOUS_COOLDOWN_RESET;
    public float WEAPON_PREVIOUS_COOLDOWN;

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
    public Vector3 movement;

    public bool isGrounded;
    public float airTime;
    public float coyoteTime;
    public float jumpForce;
    public bool canDoubleJump;
    public int currentJump;
    public int maxJumps;

    public float airSpeed;
    public float airDeltaX;
    public float airDeltaZ;
    public Vector3 airVelocity;
    public Vector3 highestVelocity;

    public bool canBunnyHop;
    public bool bunnyHopFrame;
    public float bunnyHopModifier;

    // Camera Variables
    public bool thirdPerson;
    public Vector3 cameraStart;
    public Vector3 cameraDistance;
    public bool isMouse;
    public bool forceController;
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

    void Update()
    {
        if (play)
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
                if (DODGE_COOLDOWN_TIME > DODGE_COOLDOWN_RESET)
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
                if (CROUCH_COOLDOWN_TIME > CROUCH_COOLDOWN_RESET)
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
                if (SHOOT_COOLDOWN_TIME > SHOOT_COOLDOWN_RESET)
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
                if (AIM_COOLDOWN_TIME > AIM_COOLDOWN_RESET)
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
                if (RELOAD_COOLDOWN_TIME > RELOAD_COOLDOWN_RESET)
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
                if (WEAPON_LEFT_COOLDOWN_TIME > WEAPON_LEFT_COOLDOWN_RESET)
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
                if (WEAPON_RIGHT_COOLDOWN_TIME > WEAPON_RIGHT_COOLDOWN_RESET)
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
                if (WEAPON_PREVIOUS_COOLDOWN_TIME > WEAPON_PREVIOUS_COOLDOWN_RESET)
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


            // Movement
            Vector3 forward = MOVE.y * transform.forward * moveSpeed * moveSpeedModifier;
            Vector3 right = MOVE.x * transform.right * moveSpeed * moveSpeedModifier;
            movement = forward + right;


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
                    // BUNNY HOP LOGIC
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

    void FixedUpdate()
    {
        if (play)
        {
            // Velocity
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
                airVelocity = new Vector3(Rigidbody.velocity.x - airDeltaX, Rigidbody.velocity.y, Rigidbody.velocity.z - airDeltaZ);
                airDeltaX = movement.x * airSpeed;
                airDeltaZ = movement.z * airSpeed;

                Rigidbody.velocity = airVelocity + new Vector3(airDeltaX, 0, airDeltaZ);
            }


            // Velocity Debug
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


            // Rotate Velocity
            if (isGrounded == false)
            {
                Rigidbody.velocity = Quaternion.Euler(0, Camera.transform.eulerAngles.y - cameraYaw, 0) * Rigidbody.velocity;
            }
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

            // Limit Air Speed so players are not faster while jumping.
            Vector3 airSpeedControl = new Vector3(moveSpeed * moveSpeedModifier * airSpeed, 0, moveSpeed * moveSpeedModifier * airSpeed);

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