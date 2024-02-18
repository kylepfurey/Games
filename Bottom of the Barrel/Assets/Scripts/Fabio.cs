using UnityEngine;

public class Fabio : MonoBehaviour
{
    private Rigidbody Rigidbody;

    [Header("Bounds of the teleporter's position (x is minimum, y is maximum):")]
    [SerializeField] private bool useBoundsX = false;
    [SerializeField] private Vector2 boundsX = new Vector2(-10, 10);
    [SerializeField] private bool useBoundsY = false;
    [SerializeField] private Vector2 boundsY = new Vector2(-10, 10);
    [SerializeField] private bool useBoundsZ = false;
    [SerializeField] private Vector2 boundsZ = new Vector2(-10, 10);

    [Header("Eye gameobjects to face towards the camera")]
    [SerializeField] private GameObject leftEye = null;
    [SerializeField] private GameObject rightEye = null;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (useBoundsX)
        {
            if (transform.position.x > boundsX.y)
            {
                transform.position = new Vector3(boundsX.y, transform.position.y, transform.position.z);
            }
            else if (transform.position.x < boundsX.x)
            {
                transform.position = new Vector3(boundsX.x, transform.position.y, transform.position.z);
            }
        }

        if (useBoundsY)
        {
            if (transform.position.y > boundsY.y)
            {
                transform.position = new Vector3(transform.position.x, boundsY.y, transform.position.z);
            }
            else if (transform.position.y < boundsY.x)
            {
                transform.position = new Vector3(transform.position.x, boundsX.x, transform.position.z);
            }
        }

        if (useBoundsZ)
        {
            if (transform.position.z > boundsZ.y)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, boundsZ.y);
            }
            else if (transform.position.z < boundsZ.x)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, boundsZ.y);
            }
        }

        if (leftEye != null && rightEye != null)
        {
            leftEye.transform.LookAt(Camera.main.transform.position);

            rightEye.transform.LookAt(Camera.main.transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            Rigidbody.velocity += collision.rigidbody.velocity;
        }
    }
}
