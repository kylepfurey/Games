using UnityEngine;

public class FallBoundary : MonoBehaviour
{
    // Kart Data
    public TestKart Kart0;
    public Kart1 Kart1;
    public Kart2 Kart2;
    public Kart3 Kart3;
    public Kart4 Kart4;

    void Update()
    {
        // Initialize Classes
        GameObject Object0 = GameObject.FindWithTag("Kart0");
        if (Object0 != null)
        {
            Kart0 = Object0.GetComponent<TestKart>();
        }

        GameObject Object1 = GameObject.FindWithTag("Kart1");
        if (Object1 != null)
        {
            Kart1 = Object1.GetComponent<Kart1>();
        }

        GameObject Object2 = GameObject.FindWithTag("Kart2");
        if (Object2 != null)
        {
            Kart2 = Object2.GetComponent<Kart2>();
        }

        GameObject Object3 = GameObject.FindWithTag("Kart3");
        if (Object3 != null)
        {
            Kart3 = Object3.GetComponent<Kart3>();
        }

        GameObject Object4 = GameObject.FindWithTag("Kart4");
        if (Object4 != null)
        {
            Kart4 = Object4.GetComponent<Kart4>();
        }
    }

    // Entering Collision
    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        // Kart 0
        if (other.tag == "Kart0")
        {
            Kart0.transform.position = new Vector3(Kart0.lastPosition.x, Kart0.lastPosition.y + 5, Kart0.lastPosition.z);
            Kart0.transform.eulerAngles = new Vector3(0, Kart0.transform.eulerAngles.y, 0);
            Kart0.speed = 0;
            Kart0.timer = 0;
            Kart0.Rigidbody.velocity = new Vector3(0, 0, 0);
        }

        // Kart 1
        if (other.tag == "Kart1")
        {
            Kart1.transform.position = new Vector3(Kart1.lastPosition.x, Kart1.lastPosition.y + 5, Kart1.lastPosition.z);
            Kart1.transform.eulerAngles = new Vector3(0, Kart1.transform.eulerAngles.y, 0);
            Kart1.speed = 0;
            Kart1.timer = 0;
            Kart1.Rigidbody.velocity = new Vector3(0, 0, 0);
        }

        // Kart 2
        if (other.tag == "Kart2")
        {
            Kart2.transform.position = new Vector3(Kart2.lastPosition.x, Kart2.lastPosition.y + 5, Kart2.lastPosition.z);
            Kart2.transform.eulerAngles = new Vector3(0, Kart2.transform.eulerAngles.y, 0);
            Kart2.speed = 0;
            Kart2.timer = 0;
            Kart2.Rigidbody.velocity = new Vector3(0, 0, 0);
        }

        // Kart 3
        if (other.tag == "Kart3")
        {
            Kart3.transform.position = new Vector3(Kart3.lastPosition.x, Kart3.lastPosition.y + 5, Kart3.lastPosition.z);
            Kart3.transform.eulerAngles = new Vector3(0, Kart3.transform.eulerAngles.y, 0);
            Kart3.speed = 0;
            Kart3.timer = 0;
            Kart3.Rigidbody.velocity = new Vector3(0, 0, 0);
        }

        // Kart 4
        if (other.tag == "Kart4")
        {
            Kart4.transform.position = new Vector3(Kart4.lastPosition.x, Kart4.lastPosition.y + 5, Kart4.lastPosition.z);
            Kart4.transform.eulerAngles = new Vector3(0, Kart4.transform.eulerAngles.y, 0);
            Kart4.speed = 0;
            Kart4.timer = 0;
            Kart4.Rigidbody.velocity = new Vector3(0, 0, 0);
        }
    }
}
