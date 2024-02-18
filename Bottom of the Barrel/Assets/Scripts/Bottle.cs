using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField] private float lerpSpeed = 5;
    [SerializeField] private float lerpSpeedMultiplier = 0.005f;
    [SerializeField] private float distanceMultiplier = 0.001f;
    [SerializeField] private Vector2 maxRotation = new Vector2(115, 65);
    private Rigidbody Rigidbody = null;
    private Vector3 mouseDownPosition = Vector3.zero;
    private Quaternion startingRotation = Quaternion.identity;
    [SerializeField] private Rigidbody Player = null;
    [SerializeField] private float floatForce = 13;
    [SerializeField] private float movementForceScale = 0.5f;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        startingRotation = transform.rotation;

        GameManager.Audio.Play("Bubbles");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            if (mouseDownPosition.x > Input.mousePosition.x)
            {
                Rigidbody.MoveRotation(Quaternion.Euler(Vector3.Lerp(transform.eulerAngles, new Vector3(startingRotation.x, startingRotation.y, transform.eulerAngles.z + DistanceSquared(Input.mousePosition, mouseDownPosition) * distanceMultiplier), lerpSpeed * lerpSpeedMultiplier * Time.deltaTime)));
            }
            else
            {
                Rigidbody.MoveRotation(Quaternion.Euler(Vector3.Lerp(transform.eulerAngles, new Vector3(startingRotation.x, startingRotation.y, transform.eulerAngles.z - DistanceSquared(Input.mousePosition, mouseDownPosition) * distanceMultiplier), lerpSpeed * lerpSpeedMultiplier * Time.deltaTime)));
            }

            transform.eulerAngles = new Vector3(startingRotation.x, startingRotation.y, Mathf.Clamp(transform.eulerAngles.z, maxRotation.y, maxRotation.x));
        }
        else
        {
            Rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, startingRotation, lerpSpeed * Time.deltaTime));
        }
    }

    private void FixedUpdate()
    {
        Player.AddForce(new Vector3((startingRotation.eulerAngles.z - transform.eulerAngles.z) * movementForceScale, floatForce, 0), ForceMode.Force);
    }

    // Returns the squared distance between two vector 3s
    private static float DistanceSquared(Vector3 pointA, Vector3 pointB)
    {
        float xDistance = pointA.x - pointB.x;
        float yDistance = pointA.y - pointB.y;
        float zDistance = pointA.z - pointB.z;

        return xDistance * xDistance + yDistance * yDistance + zDistance * zDistance;
    }
}
