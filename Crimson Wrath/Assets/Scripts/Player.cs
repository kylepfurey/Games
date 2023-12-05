using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Player Settings
    [Header("Third Person Variables")]
    [SerializeField] private Vector3 thirdPersonCameraDistance = new Vector3(0, 0, -5);

    [Header("Rotation Variables")]
    [SerializeField] private float playerRotateSpeed = 10;
    [SerializeField] private Vector2 thirdPersonCameraClamp = new Vector2(0, 85);

    [Header("Mouse Sensitivity")]
    [SerializeField] private float mouseSensitivityX = 3;
    [SerializeField] private float mouseSensitivityY = 3;

    [Header("Player Speed")]
    [SerializeField] private float speed = 7.5f;
    [SerializeField] private float airControl = 0.5f;

    [Header("Jumping Variables")]
    [SerializeField] private bool canJump = true;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private float jumpCheckHeight = 0.25f;
    [SerializeField] private float jumpCheckWidth = 1;
    [SerializeField] private float jumpCheckDepth = 0.25f;
    [SerializeField] private Vector3 gravity = new Vector3(0, -9.8f, 0);

    [Header("Active")]
    [SerializeField] private bool canRestart = true;
    [SerializeField] private bool canExit = true;
    public bool active = true;

    // Variables
    [HideInInspector] public Rigidbody Rigidbody;
    [HideInInspector] public Camera Camera;
    private float currentSpeed;
    private bool isGrounded;
    private Vector3 groundVelocity;
    private Vector2 mouseDelta;
    [SerializeField] private AudioSource music;

    private void Start()
    {
        Initalize();
    }

    private void Update()
    {
        CameraPosition();

        if (active)
        {
            CameraInput();
            Jumping();
        }

        RestartAndExit();

        if (Input.GetKeyDown(KeyCode.M))
        {
            music.mute = !music.mute;
        }
    }

    private void FixedUpdate()
    {
        CameraRotation();

        if (active)
        {
            Movement();
        }
    }

    private void Initalize()
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

        Rigidbody.constraints = ~RigidbodyConstraints.FreezeAll | RigidbodyConstraints.FreezeRotation;
        Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

        if (!GetComponent<Collider>().material)
        {
            PhysicMaterial material = new PhysicMaterial();
            material.name = "Player Material";
            material.staticFriction = 0;
            material.dynamicFriction = 0;
            material.frictionCombine = PhysicMaterialCombine.Minimum;

            GetComponent<Collider>().material = material;
        }
        else if (GetComponent<Collider>().material.name == "")
        {
            PhysicMaterial material = new PhysicMaterial();
            material.name = "Player Material";
            material.staticFriction = 0;
            material.dynamicFriction = 0;
            material.frictionCombine = PhysicMaterialCombine.Minimum;

            GetComponent<Collider>().material = material;
        }


        name = "Player";
        tag = "Player";
        Camera.name = "Player Camera";
        Camera.tag = "MainCamera";
        Camera.nearClipPlane = 0.01f;

        currentSpeed = speed;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Movement()
    {
        // Movement
        float fallingVelocity = Rigidbody.velocity.y;

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
        Rigidbody.angularVelocity = Vector3.zero;

        if (Mathf.Abs(movement.magnitude) > 0)
        {
            RotatePlayer();
        }
    }

    private void RotatePlayer()
    {
        Vector3 playerRotation = transform.eulerAngles;
        Vector3 cameraRotation = Camera.transform.eulerAngles;

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        Camera.transform.eulerAngles = new Vector3(0, Camera.transform.eulerAngles.y, 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, Camera.transform.rotation, playerRotateSpeed * Time.deltaTime);

        transform.eulerAngles = new Vector3(playerRotation.x, transform.eulerAngles.y, playerRotation.z);
        Camera.transform.eulerAngles = cameraRotation;
    }

    private void CameraInput()
    {
        // Camera Input
        mouseDelta += new Vector2(Input.GetAxis("Mouse X") * mouseSensitivityX, 0);
        mouseDelta += new Vector2(0, -Input.GetAxis("Mouse Y") * mouseSensitivityY);

        mouseDelta.y = Mathf.Clamp(mouseDelta.y, thirdPersonCameraClamp.x, thirdPersonCameraClamp.y);
    }

    private void CameraPosition()
    {
        // Camera Position
        Camera.transform.parent = null;
        Camera.transform.position = transform.position;
        Camera.transform.Translate(Camera.transform.rotation * new Vector3(0, transform.lossyScale.y / 8, 0) + thirdPersonCameraDistance);

        RaycastHit hit;

        Vector3 cameraRotation = Camera.transform.eulerAngles;
        Camera.transform.LookAt(transform.position);
        Vector3 direction = Camera.transform.forward;
        Camera.transform.eulerAngles = cameraRotation;

        if (Physics.Raycast(transform.position, -direction, out hit, Mathf.Abs(Vector3.Distance(Camera.transform.position, transform.position)), 1, QueryTriggerInteraction.Ignore))
        {
            Camera.transform.position = hit.point;
        }
    }

    private void CameraRotation()
    {
        // Camera Rotation
        Camera.transform.eulerAngles = new Vector3(mouseDelta.y, mouseDelta.x, transform.eulerAngles.z);
    }

    private void Jumping()
    {
        // Jumping
        Physics.gravity = gravity;

        isGrounded = Physics.BoxCast(new Vector3(transform.position.x, transform.position.y - transform.lossyScale.y / 2 + jumpCheckHeight, transform.position.z), new Vector3(transform.lossyScale.x / 2 * jumpCheckWidth, 0, transform.lossyScale.z / 2 * jumpCheckWidth), -Vector3.up, Quaternion.identity, jumpCheckHeight + jumpCheckDepth, 1, QueryTriggerInteraction.Ignore);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, 0, Rigidbody.velocity.z);
            Rigidbody.angularVelocity = Vector3.zero;
            Rigidbody.AddForce(new Vector3(0, jumpForce * Rigidbody.mass, 0), ForceMode.Impulse);

            GameManager.Audio.PlayOnce("Jump");
        }
    }

    private void RestartAndExit()
    {
        // Restart and Exit
        if (Input.GetKeyDown(KeyCode.Backspace) && canRestart)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && canExit)
        {
            Application.Quit();
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
        if (Mathf.Abs(input) >= Mathf.Abs(threshold))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool Trigger(float input, float threshold)
    {
        if (Mathf.Abs(input) >= Mathf.Abs(threshold))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
