using UnityEngine;

public class GlueAndGravityGun : MonoBehaviour
{
    // Glue Variables
    [HideInInspector] public int glueSplatCount;
    [HideInInspector] public GameObject[] glueSplats;
    [SerializeField] private GameObject glueProjectile;
    [SerializeField] private float playerSpeedXMultiplier;
    [SerializeField] private float playerSpeedYMultiplier;
    public bool autoRecoverGlue;

    // Gravity Variables
    [SerializeField] private float gravityDistance;
    [HideInInspector] public bool hasGrabbedObject;
    [HideInInspector] public GameObject grabbedObject;
    private Rigidbody2D grabbedRigidbody;
    private int grabbedLayer;
    private float grabbedRotation;
    [SerializeField] private GameObject gravityPivot;
    [SerializeField] private float gravitySpeed;
    [SerializeField] private float maxDistance;
    [SerializeField] private float gravityForce;
    [HideInInspector] public int gravityCharges;
    public int maxGravityCharges;
    [SerializeField] private float gravityChargeScale;

    // HUD Update Delegate
    // NOTE: My HUD script manages all UI elements through a function, so I only need one event to carry that data to the HUD.
    public delegate void UpdateHUDEvent(StatSystem.StatType type, HUD.SettingsType settings, int value);
    private static UpdateHUDEvent updateHUD;

    // Muzzle Flash Delegate
    public delegate void MuzzleFlashEvent(bool isGlue);
    private static MuzzleFlashEvent muzzleFlash;

    private void Start()
    {
        // Get GameManager.Player, or Remove This Component
        if (GameManager.Player == null)
        {
            // Check if GameManager.Player is Missing
            print("No Player was found!");
            Destroy(this);
        }
    }

    public void ShootGlue()
    {
        if (GameManager.Player.Stats != null && GameManager.Player.Arm != null)
        {
            if (GameManager.Player.Stats.GetStat(StatSystem.StatType.Glue) > 0)
            {
                if (glueProjectile != null)
                {
                    muzzleFlash(true);

                    GlueProjectile projectile = Instantiate(glueProjectile).transform.GetChild(0).GetComponent<GlueProjectile>();

                    if (projectile != null)
                    {
                        GameManager.Player.Stats.AddStat(StatSystem.StatType.Glue, -1);

                        glueSplatCount++;

                        AddGlueSplat(projectile.gameObject.transform.parent.gameObject);

                        projectile.transform.position = GameManager.Player.Arm.transform.position;
                        projectile.transform.eulerAngles = GameManager.Player.Arm.transform.eulerAngles;
                        projectile.projectileSpeed *= GameManager.Player.Arm.crosshairDistance / GameManager.Player.Arm.defaultCrosshairDistance;
                        projectile.projectileSpeed += Mathf.Abs(GameManager.Player.Rigidbody.velocity.x * playerSpeedXMultiplier) + Mathf.Abs(GameManager.Player.Rigidbody.velocity.y * playerSpeedYMultiplier);
                    }
                    else
                    {
                        print("Projectile is broken!");
                    }
                }
                else
                {
                    print("Projectile is missing!");
                }
            }
            else
            {
                print("Out of ammo!");

                if (autoRecoverGlue && GameManager.Player.Inventory != null)
                {
                    GameManager.Player.RecoverGlue(true);

                    ShootGlue();
                }
            }
        }
    }

    public void ShootGravity()
    {
        if (GameManager.Player.Arm != null)
        {
            if (hasGrabbedObject == false)
            {
                if (Physics2D.Raycast(GameManager.Player.Arm.transform.position, GameManager.Player.Arm.transform.right, gravityDistance, ~(1 << GameManager.Player.gameObject.layer) & ~(1 << 6)))
                {
                    RaycastHit2D hitPoint = Physics2D.Raycast(GameManager.Player.Arm.transform.position, GameManager.Player.Arm.transform.right, gravityDistance, ~(1 << GameManager.Player.gameObject.layer) & ~(1 << 6));

                    if (hitPoint.rigidbody != null && hitPoint.transform.gameObject.tag != "Ungrabbable")
                    {
                        print("Grabbing object!");

                        muzzleFlash(false);

                        hasGrabbedObject = true;

                        grabbedObject = hitPoint.rigidbody.gameObject;

                        grabbedRigidbody = hitPoint.rigidbody;

                        grabbedLayer = grabbedObject.layer;

                        grabbedRigidbody.velocity = Vector3.zero;
                        grabbedRigidbody.angularVelocity = 0;

                        grabbedObject.layer = 7;

                        grabbedRotation = grabbedObject.transform.eulerAngles.z;

                        if (GameManager.Player.Arm.armRotation > 90 && GameManager.Player.Arm.armRotation < 270)
                        {
                            grabbedRotation += 180;
                        }

                        updateHUD(StatSystem.StatType.NULL, HUD.SettingsType.Charges, gravityCharges);
                    }
                    else
                    {
                        print("No grabbable object found!");
                    }
                }
                else
                {
                    print("No grabbable object found!");
                }
            }
            else
            {
                print("Shooting grabbed object!");

                muzzleFlash(false);

                hasGrabbedObject = false;

                if (grabbedObject != null)
                {
                    grabbedObject.layer = grabbedLayer;
                }

                if (grabbedRigidbody != null)
                {
                    grabbedRigidbody.velocity = Vector3.zero;
                    grabbedRigidbody.angularVelocity = 0;

                    grabbedRigidbody.AddForce(GameManager.Player.Arm.transform.rotation * new Vector2(gravityForce + gravityCharges * gravityChargeScale, 0), ForceMode2D.Impulse);
                }

                gravityCharges = 0;

                updateHUD(StatSystem.StatType.NULL, HUD.SettingsType.Charges, gravityCharges);
            }
        }
    }

    private void FixedUpdate()
    {
        // Object Affected by Gravity
        if (GameManager.Player != null)
        {
            if (hasGrabbedObject && GameManager.Player.Arm != null)
            {
                if (grabbedObject != null && grabbedRigidbody != null)
                {
                    grabbedRigidbody.MovePosition(Vector2.MoveTowards(grabbedObject.transform.position, gravityPivot.transform.position, gravitySpeed));

                    grabbedObject.transform.eulerAngles = new Vector3(grabbedObject.transform.eulerAngles.x, grabbedObject.transform.eulerAngles.y, GameManager.Player.Arm.transform.eulerAngles.z + grabbedRotation);

                    if (Mathf.Abs(Vector2.Distance(grabbedObject.transform.position, gravityPivot.transform.position)) > maxDistance)
                    {
                        DropObject();
                    }
                }
                else
                {
                    DropObject();
                }
            }
        }
    }

    public void AddGlueSplat(GameObject newGlueSplat)
    {
        GameObject[] newArray = new GameObject[glueSplats.Length + 1];

        int index = 0;
        foreach (GameObject glueSplat in glueSplats)
        {
            newArray[index] = glueSplat;

            index++;
        }

        newArray[newArray.Length - 1] = newGlueSplat;

        glueSplats = newArray;
    }

    public void RemoveFirstGlueSplat()
    {
        GameObject[] newArray = new GameObject[glueSplats.Length - 1];

        for (int i = 1; i < glueSplats.Length; i++)
        {
            newArray[i - 1] = glueSplats[i];
        }

        glueSplats = newArray;
    }

    public void DropObject()
    {
        print("Dropped grabbed object!");

        hasGrabbedObject = false;

        if (grabbedObject != null)
        {
            grabbedObject.layer = grabbedLayer;
        }

        if (grabbedRigidbody != null)
        {
            grabbedRigidbody.velocity = Vector3.zero;
            grabbedRigidbody.angularVelocity = 0;
        }

        if (GameManager.Player.Stats != null)
        {
            GameManager.Player.Stats.AddStat(StatSystem.StatType.Money, gravityCharges);
        }

        gravityCharges = 0;

        updateHUD(StatSystem.StatType.NULL, HUD.SettingsType.Charges, gravityCharges);
    }

    public void AddCallback(UpdateHUDEvent function)
    {
        updateHUD += function;
    }

    public void AddCallback(MuzzleFlashEvent function)
    {
        muzzleFlash += function;
    }
}