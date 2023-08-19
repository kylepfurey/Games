using UnityEngine;

public class FinishLine : MonoBehaviour
{
    // Kart Data
    public KartSettings KartSettings;
    public TestKart Kart0;
    public Kart1 Kart1;
    public Kart2 Kart2;
    public Kart3 Kart3;
    public Kart4 Kart4;

    // All Checkpoints
    public int maxCheckpoints = 3;
    public int maxLaps = 3;
    public int map;

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

        GameObject Object5 = GameObject.FindWithTag("Settings");
        if (Object5 != null)
        {
            KartSettings = Object5.GetComponent<KartSettings>();
        }
    }

    // Entering Collision
    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        // Kart 0 
        if (other.tag == "Kart0")
        {
            if (Kart0.checkpoints == maxCheckpoints)
            {
                Kart0.checkpoints = 0;
                Kart0.completedLaps++;

                if (Kart0.completedLaps >= maxLaps)
                {
                    Kart0.win = true;
                }
            }
        }

        // Kart 1
        if (other.tag == "Kart1")
        {
            if (Kart1.checkpoints == maxCheckpoints)
            {
                Kart1.checkpoints = 0;
                Kart1.completedLaps++;

                if (Kart1.completedLaps >= maxLaps)
                {
                    Kart1.win = true;

                    if (Kart2.win == false && Kart3.win == false && Kart4.win == false)
                    {
                        Kart1.Audio.Play("Win");
                    }

                    if ((Kart2.win == false || KartSettings.maxPlayers < 2) && (Kart3.win == false || KartSettings.maxPlayers < 3) && (Kart4.win == false || KartSettings.maxPlayers < 4))
                    {
                        Kart1.Audio.Play("Finish");
                    }
                }
                else
                {
                    if (Kart1.completedLaps == maxLaps - 1)
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
            if (Kart2.checkpoints == maxCheckpoints)
            {
                Kart2.checkpoints = 0;
                Kart2.completedLaps++;

                if (Kart2.completedLaps >= maxLaps)
                {
                    Kart2.win = true;

                    if (Kart1.win == false && Kart3.win == false && Kart4.win == false)
                    {
                        Kart1.Audio.Play("Win");
                    }

                    if (Kart1.win == false && (Kart3.win == false || KartSettings.maxPlayers < 3) && (Kart4.win == false || KartSettings.maxPlayers < 4))
                    {
                        Kart1.Audio.Play("Finish");
                    }
                }
                else
                {
                    if (Kart2.completedLaps == maxLaps - 1)
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
            if (Kart3.checkpoints == maxCheckpoints)
            {
                Kart3.checkpoints = 0;
                Kart3.completedLaps++;

                if (Kart3.completedLaps >= maxLaps)
                {
                    Kart3.win = true;

                    if (Kart1.win == false && Kart2.win == false && Kart4.win == false)
                    {
                        Kart1.Audio.Play("Win");
                    }

                    if (Kart1.win == false && (Kart2.win == false || KartSettings.maxPlayers < 2) && (Kart4.win == false || KartSettings.maxPlayers < 4))
                    {
                        Kart1.Audio.Play("Finish");
                    }
                }
                else
                {
                    if (Kart3.completedLaps == maxLaps - 1)
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
            if (Kart4.checkpoints == maxCheckpoints)
            {
                Kart4.checkpoints = 0;
                Kart4.completedLaps++;

                if (Kart4.completedLaps >= maxLaps)
                {
                    Kart4.win = true;

                    if (Kart1.win == false && Kart2.win == false && Kart3.win == false)
                    {
                        Kart1.Audio.Play("Win");
                    }

                    if (Kart1.win == false && (Kart2.win == false || KartSettings.maxPlayers < 2) && (Kart3.win == false || KartSettings.maxPlayers < 3))
                    {
                        Kart1.Audio.Play("Finish");
                    }
                }
                else
                {
                    if (Kart4.completedLaps == maxLaps - 1)
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
