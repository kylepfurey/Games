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
    public float airSpeed;

    public bool isGrounded;
    public float airTime;
    public float coyoteTime;
    public float jumpForce;
    public bool canDoubleJump;
    public int currentJumps;
    public int maxJumps;
    public bool canBunnyHop;

    public bool canDodge;
    public float dodgeDeadzone;
    public bool isDodging;
    public float dodgeForce;
    public float dodgeForceAir;
    public bool canLunge;
    public float lungeCap;
    public float lungeCapLow;

    // Camera Variables
    public bool thirdPerson;
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

    void Update()
    {
        if (play)
        {
            // Airtime
            if (isGrounded)
            {
                airTime = 0;
            }
            else
            {
                airTime += Time.deltaTime;
            }


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
            if (Input.GetDevice<Mouse>() != null || flickStick)
            {
                isMouse = true;

                // Mouse
                LOOK_X = Input.actions.FindAction("Look X").ReadValue<float>() * lookSpeedMouse * lookSpeedModifier * Time.deltaTime;
                LOOK_Y = Input.actions.FindAction("Look Y").ReadValue<float>() * lookSpeedMouse * lookSpeedModifier * Time.deltaTime;
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


            // Camera Clamp
            cameraRotationX += LOOK_X;
            cameraRotationY -= LOOK_Y;
            cameraRotationY = Mathf.Clamp(cameraRotationY, -85, 85);
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

    void LateUpdate()
    {
        if (play)
        {
            // Camera Position
            Camera.transform.position = transform.position + cameraStart;

            if (thirdPerson)
            {
                Camera.transform.Translate(cameraDistance);
            }


            // Camera Rotation
            float cameraYaw = Camera.transform.eulerAngles.y;

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


            // Rotate Movement
            Rigidbody.velocity = Quaternion.AngleAxis(Camera.transform.eulerAngles.y - cameraYaw, Vector3.up) * Rigidbody.velocity;
        }
    }

    void FixedUpdate()
    {
        if (play)
        {
            // Movement
            Vector3 forward = MOVE.y * transform.forward * moveSpeed * moveSpeedModifier;
            Vector3 right = MOVE.x * transform.right * moveSpeed * moveSpeedModifier;
            Vector3 movement = forward + right;
            float maxSpeed = moveSpeed * moveSpeedModifier;

            movement.x -= Mathf.Clamp(Rigidbody.velocity.x, -maxSpeed, maxSpeed);
            movement.z -= Mathf.Clamp(Rigidbody.velocity.z, -maxSpeed, maxSpeed);

            Vector2 absoluteMovement = MOVE;
            if (absoluteMovement.x > dodgeDeadzone) { absoluteMovement.x = 1; } else if (absoluteMovement.x < -dodgeDeadzone) { absoluteMovement.x = -1; }
            if (absoluteMovement.y > dodgeDeadzone) { absoluteMovement.y = 1; } else if (absoluteMovement.y < -dodgeDeadzone) { absoluteMovement.y = -1; }


            // Velocity
            if (isGrounded)
            {
                Rigidbody.velocity += new Vector3(movement.x, 0, movement.z);
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

            if (JUMP && JUMP_UP && ((isGrounded || airTime <= coyoteTime) || (currentJumps < maxJumps && canDoubleJump)) && play)
            {
                JUMP_UP = false;

                // Bunny Hopping
                if (canBunnyHop || isGrounded == false)
                {
                    Rigidbody.velocity += new Vector3(movement.x, -Rigidbody.velocity.y, movement.z);
                }

                Rigidbody.AddRelativeForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);

                currentJumps += 1;
            }


            // Dodging
            if (DODGE && DODGE_UP && DODGE_COOLDOWN_TIME <= 0 && canDodge && isDodging == false)
            {
                DODGE_UP = false;
                DODGE_COOLDOWN_TIME = DODGE_COOLDOWN;

                Rigidbody.velocity = Vector3.zero;

                if (isGrounded)
                {
                    Rigidbody.AddRelativeForce(new Vector3(0, 2, 0), ForceMode.Impulse);

                    if (absoluteMovement == Vector2.zero)
                    {
                        Rigidbody.AddRelativeForce(new Vector3(0, 0, dodgeForce * 1.5f), ForceMode.Impulse);
                    }
                    else
                    {
                        Rigidbody.AddRelativeForce(new Vector3(absoluteMovement.x * dodgeForce, 0, absoluteMovement.y * dodgeForce), ForceMode.Impulse);
                    }

                    isDodging = true;
                }
                else
                {
                    if (absoluteMovement == Vector2.zero)
                    {
                        Rigidbody.AddRelativeForce(new Vector3(0, 0, dodgeForceAir), ForceMode.Impulse);
                    }
                    else
                    {
                        Rigidbody.AddRelativeForce(new Vector3(absoluteMovement.x * dodgeForceAir, 0, absoluteMovement.y * dodgeForceAir), ForceMode.Impulse);
                    }

                    isDodging = true;
                }
            }


            // Lunging Forward
            if (canLunge == false && isDodging)
            {
                Rigidbody.velocity = new Vector3(Mathf.Clamp(Rigidbody.velocity.x, -lungeCapLow, lungeCapLow), Rigidbody.velocity.y, Mathf.Clamp(Rigidbody.velocity.z, -lungeCapLow, lungeCapLow));
            }
            else if (canLunge && isDodging)
            {
                Rigidbody.velocity = new Vector3(Mathf.Clamp(Rigidbody.velocity.x, -lungeCap, lungeCap), Rigidbody.velocity.y, Mathf.Clamp(Rigidbody.velocity.z, -lungeCap, lungeCap));
            }
        }
    }

    // Entering Collision
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            isGrounded = true;
            currentJumps = 0;

            if (DODGE_COOLDOWN_TIME <= 0)
            {
                isDodging = false;
            }
        }
    }

    // In Collision
    void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            isGrounded = true;
            currentJumps = 0;

            if (DODGE_COOLDOWN_TIME <= 0)
            {
                isDodging = false;
            }
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