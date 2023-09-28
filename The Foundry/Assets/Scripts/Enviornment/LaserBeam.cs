using FPSGame;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] private PlayerMovement Player;
    [SerializeField] private Health Health;
    [SerializeField] private int Damage;
    [SerializeField] private float DamageBufferStart;

    [SerializeField] private GameObject RayStartingPoint;
    [SerializeField] private GameObject RayBeam;
    [SerializeField] private float RayMaxDistance = 100;
    private float previousDistance;
    [SerializeField] private bool On = true;

    [SerializeField] private int MovementState;             // Which position state the beam is moving towards.
    [SerializeField] private GameObject[] MovementPoints;   // Positions the beam will move to.
    [SerializeField] private float[] MovementSpeed;         // The speed the beam will move to the corresponding state.

    [SerializeField] private Vector3 AutoRotateSpeed;       // The speed the beam will automatically rotate every frame.

    [SerializeField] private int RotationState;             // Which rotation state the beam is currently rotating towards.
    [SerializeField] private GameObject[] RotationPoints;   // Rotations the beam will rotate to.
    [SerializeField] private float[] RotationSpeed;         // The speed the beam will rotate to the corresponding state.

    [SerializeField] private GameObject BurnMark;
    [SerializeField] private float renderDistance;
    [SerializeField] private GameObject Particles;

    void Start()
    {
        // References
        if (GameObject.Find("FPSCharacter") != null)
        {
            Player = GameObject.Find("FPSCharacter").GetComponent<PlayerMovement>();
            Health = Player.GetComponent<Health>();
        }


        // Set Ray Pivot
        if (RayStartingPoint == null)
        {
            RayStartingPoint = gameObject;
            RayBeam = RayStartingPoint.transform.GetChild(0).gameObject;
        }
    }

    void Update()
    {
        if (Player == null)
        {
            if (GameObject.Find("FPSCharacter") != null)
            {
                Player = GameObject.Find("FPSCharacter").GetComponent<PlayerMovement>();
            }
        }

        if (Player != null && Health == null)
        {
            Health = Player.GetComponent<Health>();
        }

        if (Player != null && Health != null)
        {
            // Drawing Beam
            if (On)
            {
                RaycastHit hitPoint;
                hitPoint = RenderBeam(RayMaxDistance);

                if (Physics.Raycast(RayStartingPoint.transform.position, RayStartingPoint.transform.right, RayMaxDistance, ~0, QueryTriggerInteraction.Ignore))
                {
                    // Create Burn Decal
                    if (hitPoint.collider != null)
                    {
                        if (hitPoint.collider.attachedRigidbody == null && Mathf.Abs(Vector3.Distance(Player.transform.position, hitPoint.point)) < renderDistance)
                        {
                            GameObject burnMark = Instantiate(BurnMark);
                            burnMark.transform.position = hitPoint.point;
                            burnMark.transform.rotation = Quaternion.FromToRotation(burnMark.transform.forward, hitPoint.normal);

                            if (hitPoint.collider.transform.parent != null)
                            {
                                burnMark.transform.parent = hitPoint.collider.transform.parent;
                            }
                        }

                        // Create Particles
                        if (hitPoint.collider.attachedRigidbody == null && Mathf.Abs(Vector3.Distance(Player.transform.position, hitPoint.point)) < renderDistance && Particles != null)
                        {
                            Particles.transform.position = hitPoint.point;
                            Particles.transform.eulerAngles = RayStartingPoint.transform.eulerAngles;
                        }
                    }
                }
                else
                {
                    RenderBeam(RayMaxDistance);

                    if (Particles != null)
                    {
                        Particles.transform.eulerAngles = RayStartingPoint.transform.eulerAngles;
                        Particles.transform.Translate(new Vector3(0, 0, RayMaxDistance));
                    }
                }
            }
            else
            {
                RenderBeam(0);

                if (Particles != null)
                {
                    Particles.transform.position = new Vector3(0, -200, 0);
                }
            }
        }
    }

    void FixedUpdate()
    {
        BeamMovement();
        BeamRotation();
    }

    RaycastHit RenderBeam(float maxDistance)
    {
        previousDistance = RayBeam.transform.localScale.x + 0.1f;

        // Adjust Length of Ray
        if (Physics.Raycast(RayStartingPoint.transform.position, RayStartingPoint.transform.right, out RaycastHit RayHitPoint, maxDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            RayBeam.transform.localScale = new Vector3(RayHitPoint.distance, RayBeam.transform.localScale.y, RayBeam.transform.localScale.z);
        }
        else
        {
            RayBeam.transform.localScale = new Vector3(maxDistance, RayBeam.transform.localScale.y, RayBeam.transform.localScale.z);
        }

        // Prevent Flickering
        if (RayBeam.transform.localScale.x < 0.5f)
        {
            RayBeam.transform.localScale = new Vector3(previousDistance, RayBeam.transform.localScale.y, RayBeam.transform.localScale.z);
        }

        RayBeam.transform.localPosition = new Vector3(RayBeam.transform.localScale.x / 2, RayBeam.transform.localPosition.y, RayBeam.transform.localPosition.z);

        // Hit Player
        if (RayHitPoint.collider == Player.GetComponent<CapsuleCollider>())
        {
            if (Health.damageBuffer <= 0)
            {
                Health.damageBuffer = DamageBufferStart;
                Health.OnTakeDamage(Damage, null);

                // PLAY SOUND HERE
            }
        }

        return RayHitPoint;
    }

    void BeamMovement()
    {
        // Beam Movement
        if (MovementPoints.Length > 0)
        {
            if (MovementState > MovementPoints.Length - 1)
            {
                MovementState = 0;
            }

            GameObject Beam = transform.parent.gameObject;

            if (Mathf.Abs(Vector3.Distance(Beam.transform.position, MovementPoints[MovementState].transform.position)) > MovementSpeed[MovementState])
            {
                Beam.transform.position = Vector3.MoveTowards(Beam.transform.position, MovementPoints[MovementState].transform.position, MovementSpeed[MovementState]);
            }
            else
            {
                Beam.transform.position = MovementPoints[MovementState].transform.position;
                MovementState++;
            }
        }
    }

    void BeamRotation()
    {
        // Beam Rotation
        if (RotationPoints.Length > 0 && AutoRotateSpeed == Vector3.zero)
        {
            if (RotationState > RotationPoints.Length - 1)
            {
                RotationState = 0;
            }

            GameObject Beam = transform.parent.gameObject;

            if (Mathf.Abs(Vector3.Distance(Beam.transform.eulerAngles, RotationPoints[RotationState].transform.eulerAngles)) > RotationSpeed[RotationState])
            {
                Beam.transform.rotation = Quaternion.Euler(Vector3.MoveTowards(Beam.transform.eulerAngles, RotationPoints[RotationState].transform.eulerAngles, RotationSpeed[RotationState]));
            }
            else
            {
                Beam.transform.rotation = Quaternion.Euler(RotationPoints[RotationState].transform.eulerAngles);
                RotationState++;
            }
        }
        else if (AutoRotateSpeed != Vector3.zero)
        {
            GameObject Beam = transform.parent.gameObject;

            Beam.transform.Rotate(AutoRotateSpeed);
        }
    }
}