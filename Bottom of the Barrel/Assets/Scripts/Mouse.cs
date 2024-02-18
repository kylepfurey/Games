using UnityEngine;

// The mouse's interactive functionality.
public class Mouse : MonoBehaviour
{
    [Header("The mouse's interactive functionality.")]

    [Header("The custom gravity:")]
    [SerializeField] private Vector3 gravity = new Vector3(0, -15, 0);

    [Header("The distance to raycast from the camera to the mouse position:")]
    [SerializeField] private float mouseDistance = 15;

    // Whether the player is currently grabbing something
    [HideInInspector] public bool isGrabbing = false;

    // The player's current grabbed object
    private Rigidbody grabbedObject = null;

    // The mouse's current position in world space (when grabbing)
    [HideInInspector] public Vector3 mousePosition = Vector3.zero;

    [Header("The speed grabbed objects should lerp to the mouse:")]
    public float lerpSpeed = 0.05f;

    [Header("The throw speed multiplier of an object:")]
    [SerializeField] private float throwSpeed = 4;

    [Header("Music volume:")]
    [SerializeField] private float volume = 0.1f;

    private void Start()
    {
        Physics.gravity = gravity;
    }

    private void Update()
    {
        // Check if the mouse is down
        if (Input.GetMouseButtonDown(0))
        {
            // Get mouse position
            Ray mouseRaycast = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Check if we hit something
            if (Physics.Raycast(mouseRaycast, out RaycastHit hit, mouseDistance, (1 << 0), QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.tag != "Fabio" && hit.transform.tag != "Ungrabbable" && hit.rigidbody != null)
                {
                    GameManager.Audio.Play("Hit");

                    isGrabbing = true;

                    grabbedObject = hit.rigidbody;

                    if (hit.transform.tag != "Physics Object without Gravity")
                    {
                        grabbedObject.useGravity = false;
                    }
                }
            }
        }
        else
        // Check if we are grabbing something
        if (isGrabbing && Input.GetMouseButton(0))
        {
            // Get mouse position
            Ray mouseRaycast = Camera.main.ScreenPointToRay(Input.mousePosition);

            mousePosition = mouseRaycast.GetPoint(mouseDistance);

            mousePosition.z = 0;

            grabbedObject.MovePosition(Vector3.Lerp(grabbedObject.transform.position, mousePosition, lerpSpeed));

            grabbedObject.velocity = Vector3.zero;
        }
        else if (isGrabbing)
        {
            isGrabbing = false;

            if (grabbedObject.transform.tag != "Physics Object without Gravity")
            {
                grabbedObject.useGravity = true;
            }

            if (!grabbedObject.isKinematic)
            {
                grabbedObject.velocity += (mousePosition - grabbedObject.transform.position) * throwSpeed;
            }

            if (grabbedObject.velocity.magnitude > 10)
            {
                GameManager.Audio.Play("Whoosh");
            }

            grabbedObject = null;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioSource source = GameManager.Audio.Get("Title");

            if (source.volume == 0)
            {
                source.volume = volume;
            }
            else
            {
                source.volume = 0;
            }
        }
    }
}
