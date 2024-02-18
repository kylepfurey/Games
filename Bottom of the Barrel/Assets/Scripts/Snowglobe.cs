using System.Collections.Generic;
using UnityEngine;

public class Snowglobe : MonoBehaviour
{
    [SerializeField] private GameObject center = null;
    [SerializeField] private float lerpSpeed = 5;
    public List<Rigidbody> globeObjects = null;
    [SerializeField] private float movementMultiplier = 0.25f;
    private Rigidbody Rigidbody = null;
    [SerializeField] private Vector2 maxVelocity = new Vector3(40, 20);
    [SerializeField] private float offScreenDistance = 20;
    [SerializeField] private float shakeCounter = 2;
    private float shake = 0;
    private AudioSource audio = null;
    [SerializeField] private float shakeVolumeScale = 0.05f;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();

        globeObjects.Clear();

        Rigidbody[] Rigidbodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

        for (int i = 0; i < Rigidbodies.Length; i++)
        {
            globeObjects.Add(Rigidbodies[i]);
        }

        globeObjects.Remove(Rigidbody);

        GameManager.Audio.Play("Sleighbells");

        audio = GameManager.Audio.Get("Shake");

        audio.volume = 0;

        audio.Play();
    }

    private void Update()
    {
        if (!GameManager.Player.isGrabbing)
        {
            for (int i = 0; i < globeObjects.Count; i++)
            {
                globeObjects[i].MovePosition(globeObjects[i].transform.position - (transform.position - Vector3.Lerp(transform.position, center.transform.position, lerpSpeed * Time.deltaTime)));
            }

            Rigidbody.MovePosition(Vector3.Lerp(transform.position, center.transform.position, lerpSpeed * Time.deltaTime));
        }
        else if (shake >= shakeCounter)
        {
            shake = 0;

            for (int i = 0; i < globeObjects.Count; i++)
            {
                globeObjects[i].velocity += (GameManager.Player.mousePosition - transform.position) * movementMultiplier;

                if (globeObjects[i].velocity.x > maxVelocity.x)
                {
                    globeObjects[i].velocity = new Vector3(maxVelocity.x, globeObjects[i].velocity.y, 0);
                }
                else if (globeObjects[i].velocity.x < -maxVelocity.x)
                {
                    globeObjects[i].velocity = new Vector3(-maxVelocity.x, globeObjects[i].velocity.y, 0);
                }

                if (globeObjects[i].velocity.y > maxVelocity.y)
                {
                    globeObjects[i].velocity = new Vector3(globeObjects[i].velocity.x, maxVelocity.y, 0);
                }
                else if (globeObjects[i].velocity.y < -maxVelocity.y)
                {
                    globeObjects[i].velocity = new Vector3(globeObjects[i].velocity.x, -maxVelocity.y, 0);
                }
            }
        }
        else
        {
            shake += Time.deltaTime;
        }

        for (int i = 0; i < globeObjects.Count; i++)
        {
            if (DistanceSquared(transform.position, globeObjects[i].transform.position) > offScreenDistance * offScreenDistance)
            {
                globeObjects[i].velocity = Vector3.zero;
                globeObjects[i].transform.position = new Vector3(transform.position.x, transform.position.y, globeObjects[i].transform.position.z);
            }
        }

        audio.volume = Mathf.Min(DistanceSquared(transform.position, center.transform.position) * shakeVolumeScale, 0.3f);
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
