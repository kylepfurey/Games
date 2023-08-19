using UnityEngine;

public class BouncerBoom : MonoBehaviour
{
    public Vector3 startSize = new Vector3(0.25f, 0.25f, 0.25f);
    public Vector3 growSpeed = new Vector3(0.05f, 0.05f, 0.05f);
    public Vector3 endSize = new Vector3(1, 1, 1);

    void Start()
    {
        transform.localScale = startSize;
    }

    void FixedUpdate()
    {
        if (transform.localScale.x >= endSize.x && transform.localScale.y >= endSize.y && transform.localScale.z >= endSize.z)
        {
            Destroy(transform.gameObject);
        }

        transform.localScale += growSpeed;
    }
}