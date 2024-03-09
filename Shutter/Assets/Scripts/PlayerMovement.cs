
// Player Movement Template Script
// by Kyle Furey

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

// Drop onto any object in a 3D Unity game to generate a Player.
public class PlayerMovement : MonoBehaviour
{
    [Header("Drop onto any object in a 3D Unity game to generate a Player.")]

    // Player Settings
    [Header("Third Person Variables")]
    [SerializeField] private Vector3 firstPersonCameraDistance = new Vector3(0, 0.5f, 0);

    [Header("Rotation Variables")]
    [SerializeField] private Vector2 firstPersonCameraClamp = new Vector2(-85, 85);

    [Header("Mouse Sensitivity")]
    [SerializeField] private float mouseSensitivityX = 3;
    [SerializeField] private float mouseSensitivityY = 3;
    [SerializeField] private bool invertX = false;
    [SerializeField] private bool invertY = false;

    [Header("Player Speed")]
    [SerializeField] private float speed = 7.5f;

    [Header("Camera Variables")]
    [SerializeField] private float shutterSpeed = 0.5f;
    [SerializeField] private float minSpeed = 1;
    [SerializeField] private float maxSpeed = 4;
    [SerializeField] private float minimumFog = 0;
    [SerializeField] private float maximumFog = 0.4f;
    [SerializeField] private float deltaScale = 0.05f;
    [SerializeField] private float lerpSpeed = 1;
    [SerializeField] private Slider slider = null;
    [SerializeField] private float bobHeight = 0.1f;
    [SerializeField] private float bobSpeed = 10;

    [Header("Active")]
    public bool active = true;

    // Variables
    public Rigidbody Rigidbody;
    private Camera Camera;
    private float currentSpeed;
    private Vector2 mouseDelta;
    private float bobTime;

    private void Start()
    {
        Initialize();

        AudioManager.Instance.Play("Footsteps");
        AudioManager.Instance.Play("Shutter");
    }

    private void Update()
    {
        CameraRotation();
        CameraPosition();

        if (active)
        {
            CameraInput();
            Movement();
            UpdateCamera();
        }
        else
        {
            Rigidbody.velocity = new Vector3(0, Rigidbody.velocity.y, 0);
        }

        AudioManager.Instance.Get("Footsteps").volume = Rigidbody.velocity.magnitude / 4;

        if (Rigidbody.velocity.x != 0 || Rigidbody.velocity.z != 0)
        {
            bobTime += Time.deltaTime;
        }

        Exit();
    }

    private void Initialize()
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
        currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime);

        float fallingVelocity = Rigidbody.velocity.y;

        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Rigidbody.velocity = transform.forward * currentSpeed * movement.y + transform.right * currentSpeed * movement.x;

        Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, fallingVelocity, Rigidbody.velocity.z);
        Rigidbody.angularVelocity = Vector3.zero;
    }

    private void CameraInput()
    {
        // Camera Input
        if (!invertX)
        {
            mouseDelta += new Vector2(Input.GetAxis("Mouse X") * mouseSensitivityX, 0);
        }
        else
        {
            mouseDelta -= new Vector2(Input.GetAxis("Mouse X") * mouseSensitivityX, 0);
        }

        if (!invertY)
        {
            mouseDelta += new Vector2(0, -Input.GetAxis("Mouse Y") * mouseSensitivityY);
        }
        else
        {
            mouseDelta -= new Vector2(0, -Input.GetAxis("Mouse Y") * mouseSensitivityY);
        }

        mouseDelta.y = Mathf.Clamp(mouseDelta.y, firstPersonCameraClamp.x, firstPersonCameraClamp.y);
    }

    private void CameraPosition()
    {
        // Camera Position
        Camera.transform.parent = transform;
        Camera.transform.localPosition = firstPersonCameraDistance;
        Camera.transform.localPosition += new Vector3(0, bobHeight * Mathf.Sin(bobTime * bobSpeed));
    }

    private void CameraRotation()
    {
        // Camera Rotation
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, mouseDelta.x, transform.eulerAngles.z);
        Camera.transform.eulerAngles = new Vector3(mouseDelta.y, transform.eulerAngles.y, Camera.transform.eulerAngles.z);
    }

    private void Exit()
    {
        // Restart and Exit
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void UpdateCamera()
    {
        if (Input.mouseScrollDelta.y != 0 || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            AudioManager.Instance.Get("Shutter").volume = 1;
        }
        else
        {
            AudioManager.Instance.Get("Shutter").volume = 0;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            shutterSpeed = Mathf.Clamp(shutterSpeed + 0.25f, 0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            shutterSpeed = Mathf.Clamp(shutterSpeed - 0.25f, 0, 1);
        }

        shutterSpeed = Mathf.Clamp(shutterSpeed + Input.mouseScrollDelta.y * deltaScale, 0, 1);

        RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, Value(shutterSpeed, minimumFog, maximumFog), Time.deltaTime * lerpSpeed);

        speed = Mathf.MoveTowards(speed, Value(shutterSpeed, minSpeed, maxSpeed), Time.deltaTime * lerpSpeed * 2);

        slider.value = shutterSpeed;
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

    public static float Percentage(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }

    public static float Value(float percentage, float min, float max)
    {
        return (max - min) * percentage + min;
    }
}
