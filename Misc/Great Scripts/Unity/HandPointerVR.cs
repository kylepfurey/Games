using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

// Used to detect when the player is pointing using hand tracking in VR, as well as specific interactions through pointing.
public class HandPointerVR : MonoBehaviour
{
    [Header("Used to detect when the player is pointing using hand tracking in VR, as well as specific interactions through pointing.")]

    [Header("POINTING SETTINGS")]

    [Header("The player's body GameObject:")]
    [SerializeField] private GameObject player = null;

    [Header("The parent object of the player's tracked hands:")]
    [SerializeField] private GameObject handParentObject = null;

    [Header("The names of the hand objects to be stored as the player's tracked hands:")]
    [SerializeField] private string leftHandObjectName = "L_Wrist";
    [SerializeField] private string rightHandObjectName = "R_Wrist";
    [HideInInspector] public GameObject leftHand = null;
    [HideInInspector] public GameObject rightHand = null;

    // Whether the player's hands are currently in a pointing state
    private bool isPointingRight = false;
    private bool isPointingLeft = false;

    // What the player is pointing at
    private RaycastHit hit;

    [Header("TELEPORTATION SETTINGS")]

    // Whether the player is in the middle of teleporting
    private bool isTeleporting = false;

    [Header("Whether the player can teleport with their right hand or left hand:")]
    [SerializeField] private bool canTeleportRight = true;
    [SerializeField] private bool canTeleportLeft = true;

    [Header("The time required to teleport via pointing at a teleporter and the delay after a successful teleport (should be synced with the fade):")]
    [SerializeField] private float teleportTimer = 0;
    [SerializeField] private float teleportTime = 1.5f;
    [SerializeField] private float teleportDelay = 0.9f;

    [Header("The tag a trigger must have to be a teleporter:")]
    [SerializeField] private string teleportTag = "Anchor";

    [Header("Events triggered by teleporting (fading the screen should be one):")]
    [SerializeField] private UnityEvent OnTeleport;

    [Header("TELEPORTATION UI")]

    [Header("The slider and its components indicating when the player is currently teleporting:")]
    [SerializeField] private Slider slider = null;
    [SerializeField] private Image[] sliderImages = null;
    [SerializeField] private float sliderFadeSpeed = 300;

    // Sets the left hand object (call when the left hand spawns)
    public void SetLeftHand()
    {
        leftHand = handParentObject.transform.Find(leftHandObjectName).gameObject;

        print("Left hand spawned! Object name is " + leftHand.name + ".");
    }

    // Sets the right hand object (call when the right hand spawns)
    public void SetRightHand()
    {
        rightHand = handParentObject.transform.Find(rightHandObjectName).gameObject;

        print("Right hand spawned! Object name is " + rightHand.name + ".");
    }

    private void Start()
    {
        // Hide the slider
        for (int i = 0; i < sliderImages.Length; i++)
        {
            sliderImages[i].color = new Vector4(sliderImages[i].color.r, sliderImages[i].color.g, sliderImages[i].color.b, 0);
        }
    }

    private void Update()
    {
        // Check the player's hands
        if (leftHand != null && rightHand != null)
        {
            // DEBUGGING KEYS

            isPointingLeft = Input.GetKey(KeyCode.Alpha1);
            isPointingRight = Input.GetKey(KeyCode.Alpha2);


            // TELEPORTATION

            // Teleportation with right hand
            if (canTeleportRight && isPointingRight)
            {
                CheckTeleport(isPointingRight, rightHand.transform.position, rightHand.transform.rotation);
            }
            // Teleportation with left hand
            else if (canTeleportLeft)
            {
                CheckTeleport(isPointingLeft, leftHand.transform.position, leftHand.transform.rotation);
            }
        }
    }

    // Checking if the player is teleporting
    private void CheckTeleport(bool isPointing, Vector3 aimPosition, Quaternion aimRotation)
    {
        // Set the slider value
        slider.value = teleportTimer / teleportTime;

        // Is the player pointing
        if (isPointing && !isTeleporting)
        {
            // Draw the pointing direction
            Debug.DrawRay(aimPosition, aimRotation * Vector3.forward, Color.green);

            // Does the player hit something
            if (Physics.Raycast(aimPosition, aimRotation * Vector3.forward, out hit, 100, ~(0 >> 1), QueryTriggerInteraction.Collide))
            {
                // Is the player hitting a teleport anchor
                if (hit.transform.tag == teleportTag)
                {
                    // Increment timer
                    teleportTimer += Time.deltaTime;

                    // Has the player pointed at the anchor for teleportTime number of seconds
                    if (teleportTimer > teleportTime)
                    {
                        // Teleport the player and reset the teleport timer
                        OnTeleport.Invoke();

                        Invoke("Teleport", teleportDelay);

                        isTeleporting = true;

                        return;
                    }

                    // Fade the slider in
                    for (int i = 0; i < sliderImages.Length; i++)
                    {
                        sliderImages[i].color = new Vector4(sliderImages[i].color.r, sliderImages[i].color.g, sliderImages[i].color.b, Mathf.Max(sliderImages[i].color.a + (sliderFadeSpeed * Time.deltaTime / 255), 0));
                    }

                    return;
                }
            }
        }
        else if (isTeleporting)
        {
            teleportTimer = teleportTime;

            // Fade the slider out
            for (int i = 0; i < sliderImages.Length; i++)
            {
                sliderImages[i].color = new Vector4(sliderImages[i].color.r, sliderImages[i].color.g, sliderImages[i].color.b, Mathf.Min(sliderImages[i].color.a - (sliderFadeSpeed * Time.deltaTime / 255), 1));
            }

            return;
        }

        // Reset the teleport timer
        teleportTimer -= Time.deltaTime;
        teleportTimer = Mathf.Max(teleportTimer, 0);

        // Fade the slider out
        for (int i = 0; i < sliderImages.Length; i++)
        {
            sliderImages[i].color = new Vector4(sliderImages[i].color.r, sliderImages[i].color.g, sliderImages[i].color.b, Mathf.Min(sliderImages[i].color.a - (sliderFadeSpeed * Time.deltaTime / 255), 1));
        }
    }

    // Teleport to the current hit object
    private void Teleport()
    {
        player.transform.position = new Vector3(hit.collider.transform.position.x, player.transform.position.y, hit.collider.transform.position.z);

        teleportTimer = 0;

        isTeleporting = false;

        // Hide the slider
        for (int i = 0; i < sliderImages.Length; i++)
        {
            sliderImages[i].color = new Vector4(sliderImages[i].color.r, sliderImages[i].color.g, sliderImages[i].color.b, 0);
        }
    }
}
