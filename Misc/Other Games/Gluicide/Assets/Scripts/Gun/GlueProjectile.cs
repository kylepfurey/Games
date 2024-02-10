using UnityEngine;

public class GlueProjectile : MonoBehaviour
{
    // References
    [SerializeField] private Rigidbody2D ProjectileRigidbody;
    [SerializeField] private CircleCollider2D ProjectileCollider;
    [SerializeField] private MeshRenderer ProjectileRenderer;
    [SerializeField] private GameObject GlueSplat;
    [SerializeField] private BoxCollider2D GlueSplatCollider;
    [SerializeField] private MeshRenderer GlueSplatRenderer;

    // Physics Variables
    [SerializeField] private Vector2 startingPositionOffset;
    public float projectileSpeed;

    // Splat Variables
    private bool expand;
    private GameObject hitObject;
    [SerializeField] private float splatWidth;
    [SerializeField] private float splatHeight;
    [SerializeField] private float splatTime;
    [SerializeField] private bool glueDroops;
    [SerializeField] private bool correctRotation;
    [SerializeField] private bool objectSlidesOnGlue;

    private void Start()
    {
        // Check Rigidbody
        if (ProjectileRigidbody == null)
        {
            if (GetComponent<Rigidbody2D>() != null)
            {
                ProjectileRigidbody = GetComponent<Rigidbody2D>();
            }
            else
            {
                print("Projectile not set up properly! Missing Rigidbody2D!");
                Destroy(gameObject);
            }
        }

        // Check Projectile Collider
        if (ProjectileCollider == null)
        {
            if (GetComponent<CircleCollider2D>() != null)
            {
                ProjectileCollider = GetComponent<CircleCollider2D>();
            }
            else
            {
                print("Projectile not set up properly! Missing CircleCollider2D!");
                Destroy(gameObject);
            }
        }

        // Check Renderer
        if (ProjectileRenderer == null)
        {
            if (GetComponent<MeshRenderer>() != null)
            {
                ProjectileRenderer = GetComponent<MeshRenderer>();
            }
            else
            {
                print("Projectile not set up properly! Missing MeshRenderer!");
                Destroy(gameObject);
            }
        }

        // Check Glue Spread
        if (GlueSplat == null)
        {
            if (transform.GetChild(0) != null)
            {
                GlueSplat = transform.GetChild(0).gameObject;
            }
            else
            {
                print("Projectile not set up properly! Missing Child GameObject of Projectile!");
                Destroy(gameObject);
            }
        }

        // Check Glue Spread Collider
        if (GlueSplatCollider == null)
        {
            if (GlueSplat.GetComponent<BoxCollider2D>() != null)
            {
                GlueSplatCollider = GlueSplat.GetComponent<BoxCollider2D>();
            }
            else
            {
                print("Projectile not set up properly! Missing Child's BoxCollider2D!");
                Destroy(gameObject);
            }
        }

        // Check Glue Spread Renderer
        if (GlueSplatRenderer == null)
        {
            if (GlueSplat.GetComponent<MeshRenderer>() != null)
            {
                GlueSplatRenderer = GlueSplat.GetComponent<MeshRenderer>();
            }
            else
            {
                print("Projectile not set up properly! Missing Child's MeshRenderer!");
                Destroy(gameObject);
            }
        }


        // Shooting
        ProjectileRenderer.enabled = true;
        GlueSplatRenderer.enabled = false;

        transform.Translate(startingPositionOffset);
        ProjectileRigidbody.AddForce(transform.rotation * new Vector2(projectileSpeed, 0), ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        if (expand)
        {
            if (transform.localScale.x < hitObject.transform.lossyScale.x && ((transform.eulerAngles.z > 315 || transform.eulerAngles.z <= 45) || (transform.eulerAngles.z > 135 && transform.eulerAngles.z <= 215)))
            {
                Vector3 previousScale = transform.localScale;

                // Splat Width
                transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(splatWidth, transform.localScale.y, transform.localScale.z), splatWidth / (splatTime * 60));

                // Splat Height
                transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(transform.localScale.x, splatHeight, transform.localScale.z), splatHeight / (splatTime * 60));

                float droop = (transform.localScale.x - previousScale.x) / 2;

                if (glueDroops && droop > 0)
                {
                    if (transform.position.x + transform.localScale.x / 2 > hitObject.transform.position.x + hitObject.transform.lossyScale.x / 2)
                    {
                        transform.position -= new Vector3(droop, 0, 0);
                    }

                    if (transform.position.x - transform.localScale.x / 2 < hitObject.transform.position.x - hitObject.transform.lossyScale.x / 2)
                    {
                        transform.position += new Vector3(droop, 0, 0);
                    }
                }
            }
            else if (transform.localScale.x < hitObject.transform.lossyScale.y && ((transform.eulerAngles.z > 45 && transform.eulerAngles.z <= 135) || (transform.eulerAngles.z > 215 && transform.eulerAngles.z <= 315)))
            {
                Vector3 previousScale = transform.localScale;

                // Splat Width
                transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(splatWidth, transform.localScale.y, transform.localScale.z), splatWidth / (splatTime * 60));

                // Splat Height
                transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(transform.localScale.x, splatHeight, transform.localScale.z), splatHeight / (splatTime * 60));

                float droop = (transform.localScale.x - previousScale.x) / 2;

                if (transform.position.y + transform.localScale.x / 2 > hitObject.transform.position.y + hitObject.transform.lossyScale.y / 2)
                {
                    transform.position -= new Vector3(0, droop, 0);
                }

                if (transform.position.y - transform.localScale.x / 2 < hitObject.transform.position.y - hitObject.transform.lossyScale.y / 2)
                {
                    transform.position += new Vector3(0, droop, 0);
                }
            }
            else
            {
                expand = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        expand = true;

        ProjectileRenderer.enabled = false;
        GlueSplatRenderer.enabled = true;

        Destroy(ProjectileRigidbody);
        Destroy(ProjectileCollider);

        transform.position = collision.GetContact(0).point;
        transform.position += new Vector3(0, 0, Random.Range(0, .01f));

        transform.rotation = Quaternion.FromToRotation(transform.up, collision.GetContact(0).normal) * transform.rotation;

        gameObject.transform.parent.gameObject.transform.parent = collision.transform;
        Debug.LogWarning("HEADS UP: You need an empty parent GameObject for this projectile to work. This will allow it to stick to other GameObjects without deforming.");

        // Rotation Correction
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            correctRotation = false;
        }

        if (transform.eulerAngles.z / 90 != (int)(transform.eulerAngles.z / 90) && correctRotation)
        {
            float angle = transform.eulerAngles.z + 180;

            if (angle > 315 || angle <= 45)
            {
                angle = 0;
            }
            else if (angle > 45 && angle <= 135)
            {
                angle = 90;
            }
            else if (angle > 135 && angle <= 215)
            {
                angle = 180;
            }
            else if (angle > 215 && angle <= 315)
            {
                angle = 270;
            }

            transform.eulerAngles = new Vector3(0, 0, angle - 180);
        }

        hitObject = collision.gameObject;

        GlueSticking glueStick = transform.GetChild(0).gameObject.AddComponent<GlueSticking>();
        glueStick.objectSlidesOnGlue = objectSlidesOnGlue;
    }
}