using UnityEngine;

public class Aiming : MonoBehaviour // Script usually goes by 'Arm' in references
{
    // References
    [SerializeField] private Camera Camera;
    [SerializeField] private GameObject Arm;

    [SerializeField] private GameObject Crosshair;
    [SerializeField] private GameObject CrosshairLeft;
    [SerializeField] private GameObject CrosshairRight;
    [SerializeField] private GameObject CrosshairCenter;

    [SerializeField] private GameObject Ray;
    [SerializeField] private Material RayGreen;
    [SerializeField] private Material RayOrange;
    [SerializeField] private Material RayYellow;
    [SerializeField] private Material RayWhite;
    [SerializeField] private Material RayTransparent;

    // Camera Variables
    [SerializeField] private Vector3 cameraStart;
    [SerializeField] private Vector3 armStart;

    // Aiming Variables
    [SerializeField] private bool isMouse;
    public float armRotation;
    [SerializeField] private Vector2 cursorClamp;

    // Distance Variables
    [SerializeField] private bool calculateCrosshairDistance;
    [HideInInspector] public float crosshairDistance;
    public float defaultCrosshairDistance;

    // Muzzle Flash
    [SerializeField] private MeshRenderer muzzleFlash;
    [SerializeField] private MeshRenderer muzzleFlashInside;
    private float muzzleFlashTime;
    [SerializeField] private float muzzleFlashLength;

    private void Start()
    {
        // Get Player, or Remove This Component
        if (GameManager.Player == null)
        {
            print("No Player was found!");
            Destroy(this);
        }

        if (muzzleFlash != null && muzzleFlashInside != null)
        {
            muzzleFlash.enabled = false;
            muzzleFlashInside.enabled = false;

            if (GameManager.Player.GetComponent<GlueAndGravityGun>() != null)
            {
                GameManager.Player.GetComponent<GlueAndGravityGun>().AddCallback(MuzzleFlashCallback);
            }
        }
    }

    private void Update()
    {
        if (GameManager.Player.play)
        {
            MouseInput();
            RenderRay();
            CrosshairDistance();
            MuzzleFlash();
        }
    }

    private void LateUpdate()
    {
        if (GameManager.Player.play)
        {
            CameraMovement();
            AdjustArm();
            RenderRay();
        }
    }

    private void MouseInput()
    {
        // Mouse and Controller Input
        if (Mathf.Abs(GameManager.Player.MOUSE_X) > 0 || Mathf.Abs(GameManager.Player.MOUSE_Y) > 0)
        {
            isMouse = true;
        }
        else if (Mathf.Abs(GameManager.Player.LOOK.x) > 0 || Mathf.Abs(GameManager.Player.LOOK.y) > 0)
        {
            isMouse = false;
        }

        if (isMouse)
        {
            foreach (Transform child in Crosshair.transform)
            {
                child.GetComponent<MeshRenderer>().enabled = true;
            }

            Crosshair.transform.position += new Vector3(GameManager.Player.MOUSE_X, GameManager.Player.MOUSE_Y, 0);
            Crosshair.transform.localPosition = new Vector3(Mathf.Clamp(Crosshair.transform.localPosition.x, -cursorClamp.x, cursorClamp.x), Mathf.Clamp(Crosshair.transform.localPosition.y, -cursorClamp.y, cursorClamp.y), 5);

            // Rotate arm based on cursor angular position
            armRotation = -Mathf.Atan2(Crosshair.transform.position.x - Arm.transform.position.x, Crosshair.transform.position.y - Arm.transform.position.y) * Mathf.Rad2Deg + 90;
            Arm.transform.eulerAngles = new Vector3(0, 0, armRotation);
        }
        else
        {
            foreach (Transform child in Crosshair.transform)
            {
                child.GetComponent<MeshRenderer>().enabled = false;
            }

            // Rotate arm based on right stick rotation
            if (Mathf.Abs(GameManager.Player.LOOK.x) > 0 || Mathf.Abs(GameManager.Player.LOOK.y) > 0)
            {
                armRotation = -Mathf.Atan2(GameManager.Player.LOOK.x, GameManager.Player.LOOK.y) * Mathf.Rad2Deg + 90;
                Arm.transform.eulerAngles = new Vector3(0, 0, armRotation);
            }
        }

    }

    private void CameraMovement()
    {
        // Move Camera
        Camera.transform.position = new Vector3(GameManager.Player.transform.position.x + cameraStart.x, Camera.transform.position.y, GameManager.Player.transform.position.z + cameraStart.z);
    }

    private void AdjustArm()
    {
        // Move Arms
        if (GameManager.Player.isCrouching == false)
        {
            Arm.transform.position = GameManager.Player.transform.position + armStart;
        }
        else
        {
            Arm.transform.position = GameManager.Player.transform.position + armStart + new Vector3(0, -0.4f, 0);
        }

        // Flip Arm
        if (armRotation <= 90 || armRotation >= 270)
        {
            Arm.transform.localPosition = new Vector3(Arm.transform.localPosition.x, Arm.transform.localPosition.y, 0.035f);
            Arm.transform.localScale = new Vector3(Arm.transform.localScale.x, 1, Arm.transform.localScale.z);
            GameManager.Player.Animator.gameObject.transform.eulerAngles = new Vector3(0, -210, 0);

            if (GameManager.Player.Animator != null)
            {
                if (GameManager.Player.Rigidbody.velocity.x > 0)
                {
                    GameManager.Player.Animator.SetBool("Forward", true);
                }
                else
                {
                    GameManager.Player.Animator.SetBool("Forward", false);
                }
            }
        }
        else
        {
            Arm.transform.localPosition = new Vector3(Arm.transform.localPosition.x, Arm.transform.localPosition.y, 0.732f);
            Arm.transform.localScale = new Vector3(Arm.transform.localScale.x, -1, Arm.transform.localScale.z);
            GameManager.Player.Animator.gameObject.transform.eulerAngles = new Vector3(0, -55, 0);

            if (GameManager.Player.Animator != null)
            {
                if (GameManager.Player.Rigidbody.velocity.x > 0)
                {
                    GameManager.Player.Animator.SetBool("Forward", false);
                }
                else
                {
                    GameManager.Player.Animator.SetBool("Forward", true);
                }
            }
        }
    }

    private void RenderRay()
    {
        // Ray Length
        RaycastHit2D hitPoint = Physics2D.Raycast(Arm.transform.position, Arm.transform.right, 100, ~(1 << GameManager.Player.gameObject.layer) & ~(1 << 6));

        if (Physics2D.Raycast(Arm.transform.position, Arm.transform.right, 100, ~(1 << GameManager.Player.gameObject.layer) & ~(1 << 6)))
        {
            Ray.transform.localScale = new Vector3(hitPoint.distance - 0.6f, Ray.transform.localScale.y, 0.1f);
        }
        else
        {
            Ray.transform.localScale = new Vector3(100 - 0.6f, Ray.transform.localScale.y, 0.1f);
        }

        Ray.transform.localPosition = new Vector3(Ray.transform.localScale.x / 2 + 0.6f, 0, 0.2f);


        // Color Ray
        if (GameManager.Player.glueIsPrimary == false)
        {
            CrosshairLeft.GetComponent<MeshRenderer>().material = RayOrange;
            CrosshairRight.GetComponent<MeshRenderer>().material = RayGreen;
        }
        else
        {
            CrosshairLeft.GetComponent<MeshRenderer>().material = RayGreen;
            CrosshairRight.GetComponent<MeshRenderer>().material = RayOrange;
        }

        CrosshairCenter.GetComponent<MeshRenderer>().material = RayYellow;

        if ((GameManager.Player.primaryShot && GameManager.Player.glueIsPrimary) || (!GameManager.Player.primaryShot && !GameManager.Player.glueIsPrimary))
        {
            if (GameManager.Player.Inventory != null)
            {
                if (GameManager.Player.Inventory.hasGlueGun)
                {
                    Ray.GetComponent<MeshRenderer>().material = RayGreen;
                }
                else
                {
                    CrosshairLeft.GetComponent<MeshRenderer>().material = RayWhite;
                    CrosshairRight.GetComponent<MeshRenderer>().material = RayWhite;
                    CrosshairCenter.GetComponent<MeshRenderer>().material = RayWhite;

                    Ray.GetComponent<MeshRenderer>().material = RayTransparent;
                }
            }
            else
            {
                Ray.GetComponent<MeshRenderer>().material = RayGreen;
            }
        }
        else
        {
            if (GameManager.Player.Inventory != null)
            {
                if (GameManager.Player.Inventory.hasGravityGun)
                {
                    Ray.GetComponent<MeshRenderer>().material = RayOrange;
                }
                else
                {
                    CrosshairLeft.GetComponent<MeshRenderer>().material = RayWhite;
                    CrosshairRight.GetComponent<MeshRenderer>().material = RayWhite;
                    CrosshairCenter.GetComponent<MeshRenderer>().material = RayWhite;

                    Ray.GetComponent<MeshRenderer>().material = RayTransparent;
                }
            }
            else
            {
                Ray.GetComponent<MeshRenderer>().material = RayOrange;
            }
        }
    }

    private void CrosshairDistance()
    {
        if (calculateCrosshairDistance && isMouse)
        {
            crosshairDistance = Mathf.Abs(Vector2.Distance(transform.position, CrosshairCenter.transform.position));
        }
        else
        {
            crosshairDistance = defaultCrosshairDistance;
        }
    }

    private void MuzzleFlash()
    {
        if (muzzleFlashTime > 0)
        {
            muzzleFlashTime -= Time.deltaTime;
        }
        else
        {
            muzzleFlashTime = 0;

            if (muzzleFlash != null && muzzleFlashInside != null)
            {
                muzzleFlash.enabled = false;
                muzzleFlashInside.enabled = false;
            }
        }
    }

    public void MuzzleFlashGlue()
    {
        muzzleFlashTime = muzzleFlashLength;

        if (muzzleFlash != null && muzzleFlashInside != null)
        {
            muzzleFlash.enabled = true;
            muzzleFlashInside.enabled = true;

            muzzleFlash.material = RayGreen;
            muzzleFlashInside.material = RayWhite;
        }
    }

    public void MuzzleFlashGravity()
    {
        muzzleFlashTime = muzzleFlashLength;

        if (muzzleFlash != null && muzzleFlashInside != null)
        {
            muzzleFlash.enabled = true;
            muzzleFlashInside.enabled = true;

            muzzleFlash.material = RayOrange;
            muzzleFlashInside.material = RayWhite;
        }
    }

    private void MuzzleFlashCallback(bool isGlue)
    {
        if (isGlue)
        {
            MuzzleFlashGlue();
        }
        else
        {
            MuzzleFlashGravity();
        }
    }
}