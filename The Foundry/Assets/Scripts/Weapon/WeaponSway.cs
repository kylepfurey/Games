using FPSGame;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private PlayerMovement Player;
    [SerializeField] private PlayerInput Input;
    [SerializeField] private float movement;
    [SerializeField] private bool swayRight;
    [SerializeField] private Vector3 originalPosition;

    [SerializeField] private bool sway;
    [SerializeField] private float swaySpeed;
    [SerializeField] private float swayLength;
    [SerializeField] private float timer;

    void Start()
    {
        if (GameObject.Find("FPSCharacter") != null)
        {
            Player = GameObject.Find("FPSCharacter").GetComponent<PlayerMovement>();
        }

        if (Player != null)
        {
            Input = Player.transform.parent.GetComponentInChildren<PlayerInput>();
        }

        originalPosition = transform.localPosition;
    }

    void FixedUpdate()
    {
        if (Player == null)
        {
            if (GameObject.Find("FPSCharacter") != null)
            {
                Player = GameObject.Find("FPSCharacter").GetComponent<PlayerMovement>();
            }
        }

        if (Input != null)
        {
            if (Player != null)
            {
                Input = Player.transform.parent.GetComponentInChildren<PlayerInput>();
            }
        }

        if (Player && Input != null)
        {
            movement = Mathf.Abs(Input.actions.FindAction("Move").ReadValue<Vector2>().normalized.magnitude);

            if (swayRight)
            {
                movement *= -1;
            }

            if (sway)
            {
                if (Mathf.Abs(movement) > 0 && Player.Motor.GroundingStatus.IsStableOnGround)
                {
                    timer += Time.deltaTime * movement;

                    transform.localPosition = new Vector3(originalPosition.x + Mathf.Sin(timer / swayLength) * swaySpeed * 3, transform.localPosition.y, transform.localPosition.z);
                }
                else
                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, originalPosition, Time.deltaTime / 4);

                    timer = 0;
                }
            }
            else
            {
                timer = 0;
            }
        }
    }
}