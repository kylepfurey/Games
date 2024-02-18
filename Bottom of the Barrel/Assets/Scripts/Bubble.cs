using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 0.1f;
    [SerializeField] private Vector2 spawnRange = new Vector2(-0.478f, 0.4601f);
    [SerializeField] private float spawnHeight = -0.5264f;
    [SerializeField] private float resetHeight = 0.5187f;
    [SerializeField] private float floatForce = 100;

    // Note: X and Y values are swapped
    private void Start()
    {
        //transform.localPosition = new Vector3(transform.localPosition.x, Random.Range(spawnRange.x, spawnRange.y), transform.localPosition.z);
    }

    private void Update()
    {
        transform.localPosition += new Vector3(floatSpeed * Time.deltaTime, 0, 0);

        if (transform.localPosition.x >= resetHeight)
        {
            transform.localPosition = new Vector3(spawnHeight, Random.Range(spawnRange.x, spawnRange.y), transform.localPosition.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fabio")
        {
            other.attachedRigidbody.velocity = new Vector3(0, 0, 0);

            other.attachedRigidbody.AddForce(new Vector3(0, floatForce, 0), ForceMode.Force);

            transform.localPosition = new Vector3(Random.Range(spawnHeight, spawnRange.x), spawnRange.y, transform.localPosition.z);

            GameManager.Audio.Play("Pop", 0.2f);

            Invoke("StopPop", 0.1f);
        }
    }

    private void StopPop()
    {
        GameManager.Audio.Stop("Pop");
    }
}
