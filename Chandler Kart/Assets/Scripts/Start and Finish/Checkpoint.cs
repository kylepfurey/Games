using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Visibility
    public Renderer Renderer;

    // Kart Data
    public TestKart Kart0;
    public Kart1 Kart1;
    public Kart2 Kart2;
    public Kart3 Kart3;
    public Kart4 Kart4;

    // Checkpoint Number
    public int checkpointNumber;
    public int map;

    void Update()
    {
        // Invisible
        Renderer.enabled = false;

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
            if (Kart0.checkpoints == checkpointNumber - 1)
            {
                Kart0.checkpoints++;
            }
        }

        // Kart 1
        if (other.tag == "Kart1")
        {
            if (Kart1.checkpoints == checkpointNumber - 1)
            {
                Kart1.checkpoints++;

                if (Kart1.KartSettings.map == 3)
                {
                    if (checkpointNumber == 2)
                    {
                        Kart1.Audio.Play("Final Lap");
                    }
                    else
                    {
                        Kart1.Audio.Play("Lap");
                    }
                }
            }
        }

        // Kart 2
        if (other.tag == "Kart2")
        {
            if (Kart2.checkpoints == checkpointNumber - 1)
            {
                Kart2.checkpoints++;

                if (Kart1.KartSettings.map == 3)
                {
                    if (checkpointNumber == 2)
                    {
                        Kart1.Audio.Play("Final Lap");
                    }
                    else
                    {
                        Kart1.Audio.Play("Lap");
                    }
                }
            }
        }

        // Kart 3
        if (other.tag == "Kart3")
        {
            if (Kart3.checkpoints == checkpointNumber - 1)
            {
                Kart3.checkpoints++;

                if (Kart1.KartSettings.map == 3)
                {
                    if (checkpointNumber == 2)
                    {
                        Kart1.Audio.Play("Final Lap");
                    }
                    else
                    {
                        Kart1.Audio.Play("Lap");
                    }
                }
            }
        }

        // Kart 4
        if (other.tag == "Kart4")
        {
            if (Kart4.checkpoints == checkpointNumber - 1)
            {
                Kart4.checkpoints++;

                if (Kart1.KartSettings.map == 3)
                {
                    if (checkpointNumber == 2)
                    {
                        Kart1.Audio.Play("Final Lap");
                    }
                    else
                    {
                        Kart1.Audio.Play("Lap");
                    }
                }
            }
        }
    }
}
