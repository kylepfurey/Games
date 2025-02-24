// .cs
// 2D Player Movement Component
// by Kyle Furey

using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Allows the attached game object to move in 2D by player input.
/// </summary>
[RequireComponent(typeof(ImageAnimator))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public sealed class PlayerMovement : MonoBehaviour
{
    [Header("Allows the attached game object to move in 2D by player input.\n")]

    [Header("The speed of which the player moves horizontally:")]
    [SerializeField] private float acceleration = 0.03f;

    [Header("The maximum speed the player can move horizontally:")]
    [SerializeField] private float speed = 0.06f;

    [Header("The speed of which the player stops moving:")]
    [SerializeField] private float deceleration = 0.2f;

    [Header("The force applied to the player when they jump:")]
    [SerializeField] private float jumpForce = 8;

    [Header("A multiplier applied to the player's movement when they are not grounded:")]
    [SerializeField] private float airControl = 0.2f;

    [Header("The number of frames to flicker when damaged:")]
    [SerializeField] private int damageFlickerCount = 8;

    [Header("The time to flicker each damage frame:")]
    [SerializeField] private float damageFlickerDelay = 0.1f;

    [Header("The time to flicker each damage frame:")]
    [SerializeField] private float damageForce = 13;

    [Header("Game objects used to clamp the player position:")]
    [SerializeField] private GameObject leftClamp = null;
    [SerializeField] private GameObject rightClamp = null;

    [Header("The footsteps audio source:")]
    [SerializeField] private AudioSource footsteps = null;


    // FIELDS

    /// <summary>
    /// The current velocity of the player's movement (not rigidbody's velocity).
    /// </summary>
    [HideInInspector] public float velocity = 0;

    /// <summary>
    /// The player's animation component.
    /// </summary>
    [HideInInspector] public ImageAnimator playerAnim = null;

    /// <summary>
    /// The 2D capsule collider of this player.
    /// </summary>
    [HideInInspector] public new Collider2D collider = null;

    /// <summary>
    /// The 2D rigidbody of this player.
    /// </summary>
    [HideInInspector] public new Rigidbody2D rigidbody = null;

    /// <summary>
    /// The circle collider used to check if the player is grounded.
    /// </summary>
    private CircleCollider2D groundCollider = null;

    /// <summary>
    /// Whether the player moved the previous frame.
    /// </summary>
    private bool moved = false;

    /// <summary>
    /// Whether the player is currently hurt.
    /// </summary>
    private bool hurt = false;

    /// <summary>
    /// The number of times the player has been hurt.
    /// </summary>
    private int hurtCount = 0;


    // PROPERTIES

    /// <summary>
    /// Returns whether the player is currently grounded.
    /// </summary>
    public bool IsGrounded
    {
        get
        {
            foreach (Collider2D collider in Physics2D.OverlapCircleAll((Vector2)transform.position + groundCollider.offset, groundCollider.radius))
            {
                if (collider.gameObject != gameObject)
                {
                    return true;
                }
            }

            return false;
        }
    }


    // UNITY EVENTS

    /// <summary>
    /// Awake() is called before Start().
    /// </summary>
    private void Awake()
    {
        playerAnim = GetComponent<ImageAnimator>();

        collider = GetComponent<CapsuleCollider2D>();

        rigidbody = GetComponent<Rigidbody2D>();

        groundCollider = GetComponent<CircleCollider2D>();
    }

    /// <summary>
    /// Update() is called once per frame.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    /// <summary>
    /// FixedUpdate() is called once per physics tick.
    /// </summary>
    private void FixedUpdate()
    {
        bool grounded = IsGrounded;

        if (!moved && grounded)
        {
            velocity = Mathf.Lerp(velocity, 0, deceleration);
        }

        moved = false;

        footsteps.volume = 0;

        Vector3 scale = transform.localScale;

        if (Input.GetKey(KeyCode.A))
        {
            scale.x = Mathf.Abs(scale.x);

            velocity -= ((velocity <= 0 ? acceleration : deceleration) * (grounded ? 1 : airControl));

            moved = true;

            if (grounded)
            {
                footsteps.volume = 1;
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            scale.x = -Mathf.Abs(scale.x);

            velocity += ((velocity >= 0 ? acceleration : deceleration) * (grounded ? 1 : airControl));

            moved = true;

            if (grounded)
            {
                footsteps.volume = 1;
            }
        }

        transform.localScale = scale;

        velocity = Mathf.Clamp(velocity, -speed, speed);

        Vector3 pos = rigidbody.transform.position;

        pos.x += velocity * Time.fixedDeltaTime * 60;

        pos.x = Mathf.Clamp(pos.x, leftClamp.transform.position.x, rightClamp.transform.position.x);

        rigidbody.transform.position = pos;

        if (!hurt)
        {
            if (rigidbody.velocity.y > 0)
            {
                playerAnim.Animation = "jump";
            }
            else if (grounded)
            {
                playerAnim.Animation = Mathf.Abs(velocity) > 0.05f ? "walk" : "idle";
            }
        }
    }

    /// <summary>
    /// Called when this collider enters a trigger.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HurtBox")
        {
            Damage(true);
        }
    }

    /// <summary>
    /// Called while this collider is a trigger.
    /// </summary>
    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerEnter2D(collision);
    }


    // METHODS

    /// <summary>
    /// Causes the player to jump.
    /// </summary>
    public void Jump()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.jump);
        if (IsGrounded)
        {
            playerAnim.Animation = "jump";

            Vector2 vel = rigidbody.velocity;

            vel.y = 0;

            rigidbody.velocity = vel;

            rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Damages the player.
    /// </summary>
    public async void Damage(bool launch = false, float forceScale = 1)
    {
        if (hurt)
        {
            return;
        }

        AudioManager.Instance.PlaySFX(AudioManager.Instance.pickUpBad);

        playerAnim.Animation = "hurt";

        ++hurtCount;

        int hurtIndex = hurtCount;

        CollectionManager.Instance.collectionScore -= CollectionManager.Instance.badScoreSubtraction;

        if (launch)
        {
            rigidbody.velocity = Vector3.zero;

            rigidbody.AddForce(new Vector2(1, 1).normalized * damageForce * forceScale, ForceMode2D.Impulse);
        }

        hurt = true;

        for (int i = 0; i < damageFlickerCount; ++i)
        {
            if (this == null || hurtCount != hurtIndex)
            {
                return;
            }

            playerAnim.image.enabled = !playerAnim.image.enabled;

            await Task.Delay((int)(1000 * damageFlickerDelay));
        }

        if (this != null)
        {
            playerAnim.image.enabled = true;

            hurt = false;
        }
    }
}
