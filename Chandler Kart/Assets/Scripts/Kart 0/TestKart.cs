using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TestKart : MonoBehaviour
{
    public PlayerInput Input;

    public KartSettings KartSettings;
    public TestCamera Camera;
    public TestLight Light;
    public Rigidbody Rigidbody;
    public int kartNumber = 0;

    // Staple Variables
    public float maxSpeed;
    public float acceleration;
    public float traction;
    public float endurance;

    // Speed and Turning Variables
    public float speed;
    public float speedPercent;
    public float turnSpeed;
    public float turnPercent;
    public bool drift = false;
    public bool driftUp = true;

    // Position and Rotation Variables
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 lastPosition;

    // Timer and Go Variables
    public float timer = 0;
    public bool start;

    // Jump Variables
    public bool isGrounded = true;
    public bool jumpUp = true;
    public bool slope = false;

    // Collision Variables
    public bool wallBump = false;
    public bool flipped = false;
    public bool hit = false;
    public float spinTime;
    public float bumpX;
    public float bumpZ;
    public bool offRoad = false;
    public bool onIce = false;

    // Item Variables
    public int item = 0;
    public bool itemUp = true;
    public bool boost = false;
    public int bananaState = 0;
    public GameObject Banana;
    public GameObject recentBanana;
    public int shellState = 0;
    public GameObject Shell;
    public GameObject recentShell;
    public GameObject bouncerBoom;

    // Lap Variables
    public int checkpoints;
    public int completedLaps = 0;
    public bool win = false;
    public int placement;
    public float elapsedFrames;
    public float elapsedSeconds;
    public int elapsedMinutes;

    // Character Model
    public GameObject Character;
    public GameObject CharacterHead;
    public GameObject Empty;

    public bool go;

    void Start()
    {
        go = false;

        transform.position = new Vector3(0, -100, 0);

        GameObject Object0 = GameObject.FindWithTag("Settings");
        KartSettings = Object0.GetComponent<KartSettings>();

        kartNumber = 0;
    }

    void FixedUpdate()
    {
        start = KartSettings.start;
        bool EXIT = Button(Input.actions.FindAction("Exit").ReadValue<float>());
        bool RESTART = Button(Input.actions.FindAction("Restart").ReadValue<float>());

        if (KartSettings.maxPlayers == 0)
        {
            Input.enabled = true;

            if (start)
            {
                // Controls
                float TURN = Input.actions.FindAction("Turn").ReadValue<Vector2>().x;
                bool ACCELERATE = Button(Input.actions.FindAction("Accelerate").ReadValue<float>());
                bool REVERSE = Button(Input.actions.FindAction("Reverse").ReadValue<float>());
                bool JUMP = Button(Input.actions.FindAction("Jump").ReadValue<float>());
                bool ITEM = Button(Input.actions.FindAction("Item").ReadValue<float>());

                position = transform.position;
                rotation = transform.eulerAngles;

                if (isGrounded)
                {
                    lastPosition = transform.position;
                }


                // Timer
                timer = timer + 1;

                if (timer >= 100)
                {
                    timer = timer - 100;
                    boost = false;
                }


                // Variables
                maxSpeed = KartSettings.maxSpeed[kartNumber];
                acceleration = KartSettings.acceleration[kartNumber];
                traction = KartSettings.traction[kartNumber];
                endurance = KartSettings.endurance[kartNumber];

                // Variable Constant Modifiers
                maxSpeed = maxSpeed * 0.175f;
                acceleration = acceleration * 0.013f;
                traction = traction * 0.1f;


                // Flipped Over?
                if ((transform.eulerAngles.x >= 45 && transform.eulerAngles.x <= 315) || (transform.eulerAngles.z >= 45 && transform.eulerAngles.z <= 315))
                {
                    flipped = true;
                }
                else
                {
                    flipped = false;
                }


                // Spinning Out
                spinTime = 15 * (6 - endurance);

                if (timer >= spinTime && hit)
                {
                    hit = false;
                    Camera.lockCamera = false;

                    transform.eulerAngles += new Vector3(0, 360 / spinTime * 3, 0);
                }

                if (hit)
                {
                    transform.eulerAngles += new Vector3(0, 360 / spinTime * 3, 0);
                }


                // Accelerate
                if (win == false && go)
                {
                    if (ACCELERATE && (flipped == false) && (hit == false))
                    {
                        if (speed < maxSpeed && onIce == false)
                        {
                            speed = speed + acceleration;
                        }
                        else if (speed < maxSpeed)
                        {
                            speed = speed + 0.2f * 0.05f;
                        }
                    }
                    else if (REVERSE && (flipped == false) && (hit == false))
                    {
                        // Decelerate
                        traction = traction * 2;

                        // Reverse
                        if (speed <= 0)
                        {
                            speed = -0.125f;
                        }
                    }
                }


                // Turning Speed Calculations
                if (isGrounded && onIce)
                {
                    turnSpeed = 2 * 0.5f * TURN / maxSpeed * speed;
                }
                else if (isGrounded)
                {
                    turnSpeed = (6 - traction) * 0.5f * TURN / maxSpeed * speed;
                }
                else
                {
                    turnSpeed = (6 - 0.1f) * 0.5f * TURN;
                }


                // Drifting
                if (JUMP && isGrounded)
                {
                    if (speed < 0.5f * maxSpeed || Mathf.Abs(TURN) < 0.05f)
                    {
                        driftUp = false;
                    }
                }
                else
                {
                    driftUp = true;
                }

                if (JUMP && isGrounded && speed > 0.5f * maxSpeed && driftUp && offRoad == false && Mathf.Abs(TURN) > 0.05f)
                {
                    turnSpeed = 1.75f * (6 - traction) * 0.5f * TURN;
                    speed = 0.75f * maxSpeed;
                    drift = true;
                }
                else
                {
                    drift = false;
                }

                if (isGrounded || drift == false)
                {
                    turnSpeed /= 1.25f;
                }

                
                // Turning
                if ((turnSpeed >= 0.1f || turnSpeed <= -0.1f) && go && win == false)
                {
                    transform.eulerAngles += new Vector3(0, turnSpeed, 0);
                }


                // Jump
                if (slope == true)
                {
                    isGrounded = true;
                }

                if (JUMP && jumpUp && isGrounded && (hit == false))
                {
                    // Rotation Reset
                    if (flipped)
                    {
                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                    }

                    Rigidbody.AddForce(new Vector3(0, 5000, 0), ForceMode.Impulse);
                    jumpUp = false;
                    drift = false;
                }

                if (go == false || win)
                {
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                }

                // Gravity
                Physics.gravity = new Vector3(0, -50, 0);

                // Jump Released
                if (JUMP == false)
                {
                    jumpUp = true;
                }


                // Using Item
                if (win == false)
                {
                    if (ITEM && itemUp)
                    {
                        if (item == 1 && flipped == false && drift == false)
                        {
                            item = 0;
                            boost = true;
                            timer = 75;
                        }

                        if (item == 2 && bananaState == 1)
                        {
                            bananaState = 0;
                            item = 0;
                        }

                        if (item == 3 && shellState == 1)
                        {
                            if (slope == false || speed <= 0.75f)
                            {
                                shellState = 0;
                                item = 0;
                            }
                        }

                        if (item == 4 && ((isGrounded && jumpUp) || isGrounded == false))
                        {
                            item = 0;

                            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, 0, Rigidbody.velocity.z);
                            Rigidbody.AddForce(new Vector3(0, 8000, 0), ForceMode.Impulse);

                            var boom = bouncerBoom;
                            bouncerBoom = Instantiate(bouncerBoom);
                            bouncerBoom.transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
                            bouncerBoom.transform.eulerAngles = transform.eulerAngles;
                            bouncerBoom = boom;
                        }

                        itemUp = false;
                    }
                }

                // Item Abilities
                if (boost && (wallBump == false) && isGrounded)
                {
                    speed = 1.2f;
                }

                if (item == 2 && (bananaState == 0))
                {
                    bananaState = 1;
                    recentBanana = Instantiate(Banana, position, transform.rotation);
                }
                else if (item != 2 && (bananaState == 1))
                {
                    bananaState = 0;
                    Destroy(recentBanana.gameObject);
                }

                if (item == 3 && (shellState == 0))
                {
                    shellState = 1;
                    recentShell = Instantiate(Shell, position, transform.rotation);
                }
                else if (item != 3 && (shellState == 1))
                {
                    shellState = 0;
                    Destroy(recentShell.gameObject);
                }

                if (item == 2 && recentBanana.gameObject.IsDestroyed())
                {
                    item = 0;
                }

                if (item == 3 && recentShell.gameObject.IsDestroyed())
                {
                    item = 0;
                }

                // Item Released
                if (ITEM == false)
                {
                    itemUp = true;
                }


                // Movement
                if (go)
                {
                    if (offRoad && (boost == false))
                    {
                        transform.Translate(new Vector3(0, 0, speed * 0.5f));
                    }
                    else if (drift)
                    {
                        transform.Translate(new Vector3(0, 0, speed * 0.75f));
                    }
                    else
                    {
                        transform.Translate(new Vector3(0, 0, speed));
                    }
                }


                // Friction
                if (speed > 0 && onIce)
                {
                    speed = speed - 0.1f * 0.05f;
                }
                else if (speed > 0)
                {
                    speed = speed - traction * 0.05f;
                }

                if (speed < 0)
                {
                    speed = 0;
                }


                // Speed Percentage
                if (offRoad)
                {
                    speedPercent = speed / maxSpeed * 50;
                }
                else
                {
                    speedPercent = speed / maxSpeed * 100;
                }

                // Turn Percentage
                turnPercent = TURN * 100;
            }

            // Character Logic
            if (Character != null)
            {
                Character.transform.parent = transform.GetChild(0);
                Character.transform.position = transform.position;
                Character.transform.localPosition = new Vector3(-0.31f, 1.43f, 0);
                Character.transform.localEulerAngles = Vector3.zero;
                Character.transform.localScale = new Vector3(1.1f, 4.54f, 1.49f);

                CharacterHead = Character.transform.GetChild(1).gameObject;
            }
        }
        else
        {
            Input.enabled = false;
            Camera.on = false;
            transform.position = new Vector3(0, -50, 0);
        }


        // Restart and Close Game
        if (RESTART && KartSettings.Menu.menu == 5)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (EXIT)
        {
            Application.Quit();
        }


        // Elapsed Timer
        if (go && win == false)
        {
            elapsedFrames += Time.deltaTime;
        }

        elapsedSeconds = (int)((elapsedFrames - (int)elapsedFrames) * 100);
        elapsedSeconds /= 100;
        elapsedSeconds += (int)elapsedFrames;

        if (elapsedSeconds > 60)
        {
            elapsedMinutes++;
            elapsedFrames = 0;
        }
    }


    // Entering Collision
    public void OnCollisionEnter(Collision collision)
    {
        // Touching Ground
        if (collision.collider.tag == "Ground")
        {
            isGrounded = true;
        }

        // Touching Slope
        if (collision.collider.tag == "Slope")
        {
            slope = true;
            isGrounded = true;
        }

        // Bumping Wall
        if (collision.collider.tag == "Wall")
        {
            if (speed >= 0)
            {
                speed = 0;
                boost = false;

                Rigidbody.AddRelativeForce(new Vector3(0, 0, -20000 * (KartSettings.maxSpeed[kartNumber] / 5) * (.01f * speedPercent)), ForceMode.Impulse);
            }
            else
            {
                speed = 0;
                boost = false;

                Rigidbody.AddRelativeForce(new Vector3(0, 0, 20000), ForceMode.Impulse);
            }

            wallBump = true;
        }

        // Bumping Karts
        if (collision.collider.tag == "Bumper" || collision.collider.tag == "Kart0" || collision.collider.tag == "Kart1" || collision.collider.tag == "Kart2" || collision.collider.tag == "Kart3" || collision.collider.tag == "Kart4")
        {
            speed = speed - speed / endurance;

            // Calculate Bump Direction and Force
            if (collision.transform.position.x > transform.position.x)
            {
                if (Mathf.Abs(collision.transform.position.x - transform.position.x) > 0)
                {
                    bumpX = -KartSettings.bumpModifier / Mathf.Sqrt(endurance);
                }
                else
                {
                    bumpX = 0;
                }
            }
            else
            {
                if (Mathf.Abs(transform.position.x - collision.transform.position.x) > 0)
                {
                    bumpX = KartSettings.bumpModifier / Mathf.Sqrt(endurance);
                }
                else
                {
                    bumpX = 0;
                }
            }

            if (collision.transform.position.z > transform.position.z)
            {
                if (Mathf.Abs(collision.transform.position.z - transform.position.z) > 0)
                {
                    bumpZ = -KartSettings.bumpModifier / Mathf.Sqrt(endurance);
                }
                else
                {
                    bumpZ = 0;
                }
            }
            else
            {
                if (Mathf.Abs(transform.position.z - collision.transform.position.z) > 0)
                {
                    bumpZ = KartSettings.bumpModifier / Mathf.Sqrt(endurance);
                }
                else
                {
                    bumpZ = 0;
                }
            }

            Rigidbody.AddForce(new Vector3(bumpX, 0, bumpZ), ForceMode.Impulse);
        }
    }

    // In Collision
    public void OnCollisionStay(Collision collision)
    {
        // Touching Ground
        if (collision.collider.tag == "Ground")
        {
            isGrounded = true;
        }

        // Touching Slope
        if (collision.collider.tag == "Slope")
        {
            slope = true;
            isGrounded = true;
        }
    }

    // Exiting Collision
    public void OnCollisionExit(Collision collision)
    {
        // No Longer Touching Ground
        if (collision.collider.tag == "Ground")
        {
            isGrounded = false;
        }

        // No Longer Touching Slope
        if (collision.collider.tag == "Slope")
        {
            slope = false;
            isGrounded = false;
        }

        // No Longer Bumping Wall
        if (collision.collider.tag == "Wall")
        {
            wallBump = false;
        }
    }

    // Entering Collision
    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        // Touching Offroad
        if (other.tag == "Offroad" && KartSettings.character[0] != "Kelley")
        {
            offRoad = true;
        }

        // Touching Ice
        if (other.tag == "Ice" && KartSettings.character[0] != "Justin")
        {
            onIce = true;
        }

        // Touching Bouncer
        if (other.tag == "Bouncer")
        {
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, 0, Rigidbody.velocity.z);
            Rigidbody.AddForce(new Vector3(0, 5000, 0), ForceMode.Impulse);
        }

        // Bumping Obstacle
        if (other.tag == "Obstacle")
        {
            hit = true;
            boost = false;
            item = 0;
            speed = 0;
            timer = 0;

            Camera.lockCamera = true;
            Camera.transform.position = transform.position;

            Destroy(other.transform.parent.gameObject);
        }
    }

    // In Collision
    public void OnTriggerStay(UnityEngine.Collider other)
    {
        // Touching Offroad
        if (other.tag == "Offroad" && KartSettings.character[0] != "Kelley")
        {
            offRoad = true;
        }

        // Touching Ice
        if (other.tag == "Ice" && KartSettings.character[0] != "Justin")
        {
            onIce = true;
        }
    }

    // Exiting Collision
    public void OnTriggerExit(UnityEngine.Collider other)
    {
        // No Longer Touching Offroad
        if (other.tag == "Offroad")
        {
            offRoad = false;
        }

        // No Longer Touching Ice
        if (other.tag == "Ice")
        {
            onIce = false;
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