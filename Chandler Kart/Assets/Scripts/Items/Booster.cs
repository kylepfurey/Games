using UnityEngine;

public class Booster : MonoBehaviour
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
        if (other.tag == "Kart0" && Kart0.hit == false)
        {
            Kart0.boost = true;
            Kart0.timer = 75;
        }

        // Kart 1
        if (other.tag == "Kart1" && Kart1.hit == false)
        {
            Kart1.boost = true;
            Kart1.timer = 75;

            Kart1.Audio.StartSound("Booster 1", 0);
        }

        // Kart 2
        if (other.tag == "Kart2" && Kart2.hit == false)
        {
            Kart2.boost = true;
            Kart2.timer = 75;

            Kart1.Audio.StartSound("Booster 2", 0);
        }

        // Kart 3
        if (other.tag == "Kart3" && Kart3.hit == false)
        {
            Kart3.boost = true;
            Kart3.timer = 75;

            Kart1.Audio.StartSound("Booster 3", 0);
        }

        // Kart 4
        if (other.tag == "Kart4" && Kart4.hit == false)
        {
            Kart4.boost = true;
            Kart4.timer = 75;

            Kart1.Audio.StartSound("Booster 4", 0);
        }
    }

    // In Collision
    public void OnTriggerStay(UnityEngine.Collider other)
    {
        // Kart 0 
        if (other.tag == "Kart0" && Kart0.hit == false)
        {
            if (Kart0.boost == false)
            {
                Kart0.boost = true;
                Kart0.timer = 75;
            }
        }

        // Kart 1
        if (other.tag == "Kart1" && Kart1.hit == false)
        {
            if (Kart1.boost == false)
            {
                Kart1.boost = true;
                Kart1.timer = 75;
            }
        }

        // Kart 2
        if (other.tag == "Kart2" && Kart2.hit == false)
        {
            if (Kart2.boost == false)
            {
                Kart2.boost = true;
                Kart2.timer = 75;
            }
        }

        // Kart 3
        if (other.tag == "Kart3" && Kart3.hit == false)
        {
            if (Kart3.boost == false)
            {
                Kart3.boost = true;
                Kart3.timer = 75;
            }
        }

        // Kart 4
        if (other.tag == "Kart4" && Kart4.hit == false)
        {
            if (Kart4.boost == false)
            {
                Kart4.boost = true;
                Kart4.timer = 75;
            }
        }
    }
}