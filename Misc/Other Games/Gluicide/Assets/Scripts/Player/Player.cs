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
    public Rigidbody2D Rigidbody;
    public Aiming Arm;
    public Health Health;
    public StatSystem Stats;
    public Inventory Inventory;
    public HUD HUD;
    public Animator Animator;
    public GlueAndGravityGun Gun;

    // Controls - CAPITALS are direct input values and behavior
    private Vector2 MOVE;
    [SerializeField] private float moveSpeed;

    [HideInInspector] public bool isCrouching;
    [SerializeField] private float crouchDeadZone;
    private float crouchTime;
    [SerializeField] private float slideTime;
    private float slideVelocity;

    [HideInInspector] public Vector2 LOOK;
    public float lookSpeed;
    [HideInInspector] public float MOUSE_X;
    [HideInInspector] public float MOUSE_Y;

    private bool JUMP;
    private bool JUMP_UP;
    [SerializeField] private float JUMP_COOLDOWN;
    [SerializeField] private float JUMP_COOLDOWN_RESET;
    private float JUMP_COOLDOWN_TIME;

    private bool SHOOT_PRIMARY;
    private bool SHOOT_PRIMARY_UP;

    private bool SHOOT_SECONDARY;
    private bool SHOOT_SECONDARY_UP;

    private bool TOGGLE_RECOVER_GLUE;
    private bool TOGGLE_RECOVER_GLUE_UP;

    private bool GLUE_SPECIAL;
    private bool GLUE_SPECIAL_UP;

    private bool GRAVITY_SPECIAL;
    private bool GRAVITY_SPECIAL_UP;

    private bool SWAP;
    private bool SWAP_UP;

    [SerializeField] private bool canDropItem;
    private bool DROP;
    private bool DROP_UP;

    private bool RESTART;
    private bool EXIT;

    // Movement Variables
    private bool isGrounded;
    private float airTime;

    [SerializeField] private float coyoteTime;    // Thank you Preston
    private int currentJump;

    [SerializeField] private float jumpForce;

    // HUD Update Delegate
    // NOTE: My HUD script manages all UI elements through a function, so I only need one event to carry that data to the HUD.
    public delegate void UpdateHUDEvent(StatSystem.StatType type, HUD.SettingsType settings, int value);
    private static UpdateHUDEvent updateHUD;
    public delegate void RemoveHUDEvent(StatSystem.StatType type);
    private static RemoveHUDEvent removeHUD;

    // Player Variables
    [HideInInspector] public bool primaryShot;
    [HideInInspector] public bool glueIsPrimary;
    public bool play;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        glueIsPrimary = true;

        // Animator
        if (Animator == null)
        {
            if (GetComponentInChildren<Animator>() != null)
            {
                Animator = GetComponentInChildren<Animator>();
            }
            else
            {
                print("No Animator found!");
            }
        }

        // Player HUD
        if (HUD == null)
        {
            if (GameObject.Find("HUD") != null)
            {
                if (GameObject.Find("HUD").GetComponent<HUD>() != null)
                {
                    HUD = GameObject.Find("HUD").GetComponent<HUD>();
                }
                else
                {
                    print("No HUD found!");
                }
            }
            else
            {
                print("No HUD found!");
            }
        }

        // Player Death Logic
        if (Health == null)
        {
            if (GetComponent<HUD>() != null)
            {
                Health = GetComponent<Health>();
            }
            else
            {
                print("No Health found!");

                RemoveHUD(StatSystem.StatType.Health);
            }
        }

        // Player Stat Logic
        if (Stats == null)
        {
            if (GetComponent<StatSystem>() != null)
            {
                Stats = GetComponent<StatSystem>();
            }
            else
            {
                print("No Stats found!");

                RemoveHUD(StatSystem.StatType.Money);
                RemoveHUD(StatSystem.StatType.Glue);
            }
        }

        // Player Inventory Logic
        if (Inventory == null)
        {
            if (GetComponent<Inventory>() != null)
            {
                Inventory = GetComponent<Inventory>();
            }
            else
            {
                print("No Inventory found!");
            }
        }

        // Player Gun Logic
        if (Gun == null)
        {
            if (GetComponent<GlueAndGravityGun>() != null)
            {
                Gun = GetComponent<GlueAndGravityGun>();
            }
            else
            {
                print("No Gun found!");
            }
        }
    }

    private void Update()
    {
        if (play)
        {
            GetControls();
            Jumping();
            Shooting();
            RecoverGlue(false);
            ChargeGravity();
            DropItem();
        }

        ExitGame();
    }

    private void FixedUpdate()
    {
        if (play)
        {
            Movement();
            Crouching();
        }
    }

    // Entering Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground" || collision.transform.tag == "Ungrabbable")
        {
            Animator.SetTrigger("Land");

            isGrounded = true;

            airTime = 0;

            currentJump = 0;
        }
    }

    // In Collision
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground" || collision.transform.tag == "Ungrabbable")
        {
            isGrounded = true;

            airTime = 0;

            currentJump = 0;
        }
    }

    // Exiting Collision
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground" || collision.transform.tag == "Ungrabbable")
        {
            isGrounded = false;
        }
    }

    private void GetControls()
    {
        if (Input != null)
        {
            // Controls
            MOVE = Input.actions.FindAction("Move").ReadValue<Vector2>();
            LOOK = Input.actions.FindAction("Aim Controller").ReadValue<Vector2>();
            MOUSE_X = Input.actions.FindAction("Aim Mouse X").ReadValue<float>() * lookSpeed;
            MOUSE_Y = Input.actions.FindAction("Aim Mouse Y").ReadValue<float>() * lookSpeed;
            JUMP = Button(Input.actions.FindAction("Jump").ReadValue<float>());
            SHOOT_PRIMARY = Button(Input.actions.FindAction("Shoot Primary").ReadValue<float>());
            SHOOT_SECONDARY = Button(Input.actions.FindAction("Shoot Secondary").ReadValue<float>());
            GLUE_SPECIAL = Button(Input.actions.FindAction("Glue Special").ReadValue<float>());
            GRAVITY_SPECIAL = Button(Input.actions.FindAction("Gravity Special").ReadValue<float>());
            TOGGLE_RECOVER_GLUE = Button(Input.actions.FindAction("Toggle Recover").ReadValue<float>());
            SWAP = Button(Input.actions.FindAction("Swap").ReadValue<float>());
            DROP = Button(Input.actions.FindAction("Drop").ReadValue<float>());


            // Button Behavior
            if (!JUMP)
            {
                JUMP_UP = true;
            }

            if (JUMP_COOLDOWN != 0)
            {
                if (JUMP_UP && JUMP_COOLDOWN_TIME > JUMP_COOLDOWN_RESET)
                {
                    JUMP_COOLDOWN_TIME = JUMP_COOLDOWN_RESET;
                }

                if (JUMP_COOLDOWN_TIME > 0)
                {
                    JUMP_COOLDOWN_TIME -= Time.deltaTime;
                }
                else
                {
                    JUMP_COOLDOWN_TIME = 0;
                }
            }

            if (!SHOOT_PRIMARY)
            {
                SHOOT_PRIMARY_UP = true;
            }

            if (!SHOOT_SECONDARY)
            {
                SHOOT_SECONDARY_UP = true;
            }

            if (!GLUE_SPECIAL)
            {
                GLUE_SPECIAL_UP = true;
            }

            if (!GRAVITY_SPECIAL)
            {
                GRAVITY_SPECIAL_UP = true;
            }

            if (!TOGGLE_RECOVER_GLUE)
            {
                TOGGLE_RECOVER_GLUE_UP = true;
            }

            if (!SWAP)
            {
                SWAP_UP = true;
            }

            if (!DROP)
            {
                DROP_UP = true;
            }


            // Swap Primary and Secondary
            if (SWAP && SWAP_UP && Inventory != null)
            {
                SWAP_UP = false;

                glueIsPrimary = !glueIsPrimary;

                if (Inventory.hasGlueGun == false || Inventory.hasGravityGun == false)
                {
                    primaryShot = !primaryShot;
                }
            }
        }
        else
        {
            print("No input detected!");
        }
    }

    private void Movement()
    {
        // Movement
        if (isCrouching == false)
        {
            if (Animator != null)
            {
                Animator.SetBool("Slide", false);
            }

            Rigidbody.velocity = new Vector2(MOVE.x * moveSpeed, Rigidbody.velocity.y);

            // Sliding
            slideVelocity = Rigidbody.velocity.x * 1.5f;
            crouchTime = 0;
        }
        else if (crouchTime < slideTime)
        {
            if (Mathf.Abs(Rigidbody.velocity.x) > 0)
            {
                if (Animator != null)
                {
                    Animator.SetBool("Slide", true);
                }
            }

            Rigidbody.velocity = new Vector2(slideVelocity, Rigidbody.velocity.y);
            crouchTime += Time.deltaTime;
        }
        else
        {
            if (Animator != null)
            {
                Animator.SetBool("Slide", false);
            }

            Rigidbody.velocity = new Vector2(MOVE.x * moveSpeed / 2, Rigidbody.velocity.y);
        }

        if (Animator != null)
        {
            Animator.SetFloat("Speed", Mathf.Abs(Rigidbody.velocity.x / moveSpeed));
        }
    }

    private void Jumping()
    {
        // Jumping
        if (JUMP && JUMP_UP && (isGrounded || (airTime <= coyoteTime && currentJump == 0)) && JUMP_COOLDOWN_TIME <= 0)
        {
            if (Animator != null)
            {
                Animator.SetTrigger("Jump");
            }

            JUMP_UP = false;

            JUMP_COOLDOWN_TIME = JUMP_COOLDOWN;

            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0);

            if (isCrouching == false)
            {
                Rigidbody.AddForce(new Vector2(0, jumpForce));
            }
            else
            {
                Rigidbody.AddForce(new Vector2(0, jumpForce * 0.75f));
            }

            airTime = 0;

            currentJump++;
        }

        // Airtime
        if (isGrounded == false)
        {
            airTime += Time.deltaTime;
        }

        if (Animator != null)
        {
            Animator.SetBool("Grounded", isGrounded);
        }
    }

    private void Crouching()
    {
        // Crouching
        if (MOVE.y <= -crouchDeadZone)
        {
            isCrouching = true;
        }
        else if (MOVE.y > -crouchDeadZone)
        {
            isCrouching = false;
        }

        if (isCrouching)
        {
            transform.localScale = new Vector3(1, 1.5f, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 2, 1);
        }
    }

    private void Shooting()
    {
        // Shooting
        if (SHOOT_PRIMARY && SHOOT_PRIMARY_UP)
        {
            SHOOT_PRIMARY_UP = false;

            primaryShot = true;

            if (glueIsPrimary)
            {
                if (Inventory != null)
                {
                    if (Inventory.hasGlueGun)
                    {
                        ShootGlue();
                    }
                    else
                    {
                        primaryShot = false;

                        if (Inventory.hasGravityGun)
                        {
                            ShootGravity();
                        }
                    }
                }
                else
                {
                    ShootGlue();
                }
            }
            else
            {
                if (Inventory != null)
                {
                    if (Inventory.hasGravityGun)
                    {
                        ShootGravity();
                    }
                    else
                    {
                        primaryShot = false;

                        if (Inventory.hasGlueGun)
                        {
                            ShootGlue();
                        }
                    }
                }
                else
                {
                    ShootGravity();
                }
            }
        }

        if (SHOOT_SECONDARY && SHOOT_SECONDARY_UP)
        {
            SHOOT_SECONDARY_UP = false;

            primaryShot = false;

            if (glueIsPrimary == false)
            {
                if (Inventory != null)
                {
                    if (Inventory.hasGlueGun)
                    {
                        ShootGlue();
                    }
                    else
                    {
                        primaryShot = true;

                        if (Inventory.hasGravityGun)
                        {
                            ShootGravity();
                        }
                    }
                }
                else
                {
                    ShootGlue();
                }
            }
            else
            {
                if (Inventory != null)
                {
                    if (Inventory.hasGravityGun)
                    {
                        ShootGravity();
                    }
                    else
                    {
                        primaryShot = true;

                        if (Inventory.hasGlueGun)
                        {
                            ShootGlue();
                        }
                    }
                }
                else
                {
                    ShootGravity();
                }
            }
        }
    }

    private void ShootGlue()
    {
        // Glue Logic
        if (Animator != null)
        {
            Animator.SetTrigger("Shoot");
        }

        if (Gun != null)
        {
            Gun.ShootGlue();
        }

        UpdateHUD(StatSystem.StatType.Glue, HUD.SettingsType.NULL, Stats.GetStat(StatSystem.StatType.Glue));
    }

    private void ShootGravity()
    {
        // Gravity Logic
        if (Animator != null)
        {
            Animator.SetTrigger("Shoot");
        }

        if (Gun != null)
        {
            Gun.ShootGravity();
        }
    }

    public void RecoverGlue(bool forceInput)
    {
        if (Inventory != null && Gun != null)
        {
            // Recovering Glue
            if ((GLUE_SPECIAL && GLUE_SPECIAL_UP && Inventory.hasGlueGun) || forceInput)
            {
                GLUE_SPECIAL_UP = false;

                if (Gun.glueSplatCount > 0)
                {
                    Destroy(Gun.glueSplats[0]);

                    Gun.RemoveFirstGlueSplat();

                    Gun.glueSplatCount--;

                    Stats.AddStat(StatSystem.StatType.Glue, 1);

                    UpdateHUD(StatSystem.StatType.Glue, HUD.SettingsType.NULL, Stats.GetStat(StatSystem.StatType.Glue));

                    print("Recovered Glue!");
                }
                else
                {
                    print("No Glue to recover!");
                }
            }

            // Toggle Auto Recovery
            if (TOGGLE_RECOVER_GLUE && TOGGLE_RECOVER_GLUE_UP && Inventory.hasGlueGun)
            {
                TOGGLE_RECOVER_GLUE_UP = false;

                Gun.autoRecoverGlue = !Gun.autoRecoverGlue;

                if (Gun.autoRecoverGlue)
                {
                    UpdateHUD(StatSystem.StatType.NULL, HUD.SettingsType.Automatic, 0);
                }
                else
                {
                    UpdateHUD(StatSystem.StatType.NULL, HUD.SettingsType.SemiAutomatic, 0);
                }
            }
        }
    }

    public void ChargeGravity()
    {
        if (Inventory != null && Gun != null)
        {
            // Gravity Charges
            if (GRAVITY_SPECIAL && GRAVITY_SPECIAL_UP && Gun.hasGrabbedObject && Inventory.hasGravityGun && Inventory.hasWallet)
            {
                GRAVITY_SPECIAL_UP = false;

                if (Stats.GetStat(StatSystem.StatType.Money) > 0 && Gun.gravityCharges < Gun.maxGravityCharges)
                {
                    Stats.AddStat(StatSystem.StatType.Money, -1);

                    Gun.gravityCharges++;

                    UpdateHUD(StatSystem.StatType.NULL, HUD.SettingsType.Charges, Gun.gravityCharges);
                }
                else if (Gun.gravityCharges == Gun.maxGravityCharges || (Stats.GetStat(StatSystem.StatType.Money) == 0 && Gun.gravityCharges > 0 && Gun.gravityCharges < Gun.maxGravityCharges))
                {
                    Stats.AddStat(StatSystem.StatType.Money, Gun.gravityCharges);

                    Gun.gravityCharges = 0;

                    UpdateHUD(StatSystem.StatType.NULL, HUD.SettingsType.Charges, Gun.gravityCharges);
                }
            }
        }
    }

    private void DropItem()
    {
        if (DROP && DROP_UP && canDropItem)
        {
            DROP_UP = false;

            if (Inventory.hasGlueGun && ((primaryShot && glueIsPrimary) || (primaryShot == false && glueIsPrimary == false)))
            {
                if (Gun != null)
                {
                    for (int i = 0; i < Stats.GetMax(StatSystem.StatType.Glue); i++)
                    {
                        RecoverGlue(true);
                    }
                }

                Inventory.DropItem(Inventory.ItemType.GlueGun);

                primaryShot = !primaryShot;
            }
            else if (Inventory.hasGravityGun && ((primaryShot && glueIsPrimary == false) || (primaryShot == false && glueIsPrimary)))
            {
                if (Gun != null)
                {
                    Gun.DropObject();
                }

                Inventory.DropItem(Inventory.ItemType.GravityGun);

                primaryShot = !primaryShot;
            }
            else if (Inventory.hasWallet)
            {
                Inventory.DropItem(Inventory.ItemType.Wallet);
            }/*
            else if (Stats.GetStat(PlayerStats.Stat.Glue) > 0)
            {
                Inventory.DropItem("Glue");
            }*/
            else if (Stats.GetStat(StatSystem.StatType.Money) > 0)
            {
                Inventory.DropItem(Inventory.ItemType.Money);
            }
            else
            {
                print("You have no items!");
            }
        }
    }

    private void ExitGame()
    {
        // Restart and Exit
        if (Input != null)
        {
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
        else
        {
            print("No input detected!");
        }
    }

    public void PlayerDeath()
    {
        // Inform user that the player was killed.
        print("Game Over!");

        // Death Logic
        if (Animator != null)
        {
            Animator.SetTrigger("Death");
        }

        if (Input != null)
        {
            MOVE = Vector2.zero;
            Destroy(Input);
        }

        if (Arm != null)
        {
            Destroy(Arm.gameObject);
        }

        Invoke("ResetGame", 1);
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateHUD(StatSystem.StatType type, HUD.SettingsType settings, int value)
    {
        if (HUD != null)
        {
            // Update the HUD
            updateHUD(type, settings, value);
        }
        else
        {
            print("No HUD found!");
        }
    }

    public void RemoveHUD(StatSystem.StatType type)
    {
        if (HUD != null)
        {
            // Update the HUD
            removeHUD(type);
        }
        else
        {
            print("No HUD found!");
        }
    }

    public void AddCallback(UpdateHUDEvent function)
    {
        updateHUD += function;
    }

    public void AddCallback(RemoveHUDEvent function)
    {
        removeHUD += function;
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