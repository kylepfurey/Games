using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Placement : MonoBehaviour
{
    // Kart Data
    public KartSettings KartSettings;
    public Kart1 Kart1;
    public Kart2 Kart2;
    public Kart3 Kart3;
    public Kart4 Kart4;
    public bool good = false;

    public GameObject[] Kart = new GameObject[4];
    public bool[] win = new bool[4];
    public int[] completedLaps = new int[4];
    public int[] checkpoints = new int[4];

    public int[] order = new int[4];
    public int[] sortedOrder = new int[4];

    public GameObject[] All;
    public Checkpoint[] Checkpoint;
    public FinishLine[] FinishLine;

    public int[] previousPlacement = new int[4];
    public int[] currentPlacement = new int[4];

    public float timer = 0;

    void Start()
    {
        All = GameObject.FindGameObjectsWithTag("Checkpoint");
        Checkpoint = new Checkpoint[All.Length];
        FinishLine = new FinishLine[All.Length];

        for (int i = 0; i < All.Length; i++)
        {
            Checkpoint[i] = All[i].GetComponent<Checkpoint>();

            if (Checkpoint[i] == null)
            {
                FinishLine[i] = All[i].GetComponent<FinishLine>();
            }
        }
    }

    void FixedUpdate()
    {
        if (good == false)
        {
            // Initalize Classes
            GameObject Object0 = GameObject.FindWithTag("Settings");
            GameObject Object1 = GameObject.FindWithTag("Kart1");
            GameObject Object2 = GameObject.FindWithTag("Kart2");
            GameObject Object3 = GameObject.FindWithTag("Kart3");
            GameObject Object4 = GameObject.FindWithTag("Kart4");

            if (Object0 != null)
            {
                KartSettings = Object0.GetComponent<KartSettings>();
            }
            if (Object1 != null)
            {
                Kart1 = Object1.GetComponent<Kart1>();
            }
            if (Object2 != null)
            {
                Kart2 = Object2.GetComponent<Kart2>();
            }
            if (Object3 != null)
            {
                Kart3 = Object3.GetComponent<Kart3>();
            }
            if (Object4 != null)
            {
                Kart4 = Object4.GetComponent<Kart4>();
            }

            if (KartSettings != null && Kart1 != null && Kart2 != null && Kart3 != null && Kart4 != null)
            {
                good = true;
            }
        }


        if (good)
        {
            Kart[0] = Kart1.gameObject;
            Kart[1] = Kart2.gameObject;
            Kart[2] = Kart3.gameObject;
            Kart[3] = Kart4.gameObject;

            completedLaps[0] = Kart1.completedLaps;
            completedLaps[1] = Kart2.completedLaps;
            completedLaps[2] = Kart3.completedLaps;
            completedLaps[3] = Kart4.completedLaps;

            checkpoints[0] = Kart1.checkpoints;
            checkpoints[1] = Kart2.checkpoints;
            checkpoints[2] = Kart3.checkpoints;
            checkpoints[3] = Kart4.checkpoints;

            win[0] = Kart1.win;
            win[1] = Kart2.win;
            win[2] = Kart3.win;
            win[3] = Kart4.win;

            previousPlacement[0] = Kart1.placement;
            previousPlacement[1] = Kart2.placement;
            previousPlacement[2] = Kart3.placement;
            previousPlacement[3] = Kart4.placement;


            for (int currentKart = 0; currentKart < 4; currentKart++)
            {
                // If a player has won, save their order.
                if (win[currentKart] == false)
                {
                    order[currentKart] = 0;
                }

                for (int selectedKart = 0; selectedKart < 4; selectedKart++)
                {
                    if (currentKart == selectedKart && selectedKart != 3)
                    {
                        selectedKart++;
                    }
                    else if (currentKart == selectedKart)
                    {
                        break;
                    }


                    // Sort placement of Karts based on Laps, than Checkpoints, than who is closest to the nearest Checkpoint
                    if (completedLaps[currentKart] > completedLaps[selectedKart])
                    {
                        order[currentKart]++;
                    }
                    else if (completedLaps[currentKart] == completedLaps[selectedKart])
                    {
                        if (checkpoints[currentKart] > checkpoints[selectedKart])
                        {
                            order[currentKart]++;
                        }
                        else if (checkpoints[currentKart] == checkpoints[selectedKart])
                        {
                            for (int check = 0; check < All.Length; check++)
                            {
                                if (Checkpoint[check] != null)
                                {
                                    if (Checkpoint[check].checkpointNumber == checkpoints[currentKart] + 1 && KartSettings.map == Checkpoint[check].map)
                                    {
                                        if (Mathf.Abs(Vector3.Distance(Kart[currentKart].transform.position, Checkpoint[check].transform.position)) <= Mathf.Abs(Vector3.Distance(Kart[selectedKart].transform.position, Checkpoint[check].transform.position)))
                                        {
                                            order[currentKart]++;
                                        }
                                        break;
                                    }
                                }

                                if (FinishLine[check] != null)
                                {
                                    if (FinishLine[check].maxCheckpoints == checkpoints[currentKart] && KartSettings.map == FinishLine[check].map)
                                    {
                                        if (Mathf.Abs(Vector3.Distance(Kart[currentKart].transform.position, FinishLine[check].transform.position)) <= Mathf.Abs(Vector3.Distance(Kart[selectedKart].transform.position, FinishLine[check].transform.position)))
                                        {
                                            order[currentKart]++;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }


                    if (currentKart + 1 > KartSettings.maxPlayers)
                    {
                        order[currentKart] = -1;
                    }
                }

                sortedOrder[currentKart] = order[currentKart];
            }


            Array.Sort(sortedOrder);

            for (int currentKart = 0; currentKart < 4; currentKart++)
            {
                for (int selectedKart = 0; selectedKart < 4; selectedKart++)
                {
                    if (order[currentKart] == sortedOrder[selectedKart])
                    {
                        switch (currentKart)
                        {
                            case 0:
                                Kart1.placement = selectedKart + 1;

                                switch (Kart1.placement)
                                {
                                    case 4:
                                        Kart1.placement = 1;
                                        break;
                                    case 3:
                                        Kart1.placement = 2;
                                        break;
                                    case 2:
                                        Kart1.placement = 3;
                                        break;
                                    case 1:
                                        Kart1.placement = 4;
                                        break;
                                }

                                break;

                            case 1:
                                Kart2.placement = selectedKart + 1;

                                switch (Kart2.placement)
                                {
                                    case 4:
                                        Kart2.placement = 1;
                                        break;
                                    case 3:
                                        Kart2.placement = 2;
                                        break;
                                    case 2:
                                        Kart2.placement = 3;
                                        break;
                                    case 1:
                                        Kart2.placement = 4;
                                        break;
                                }

                                break;

                            case 2:
                                Kart3.placement = selectedKart + 1;

                                switch (Kart3.placement)
                                {
                                    case 4:
                                        Kart3.placement = 1;
                                        break;
                                    case 3:
                                        Kart3.placement = 2;
                                        break;
                                    case 2:
                                        Kart3.placement = 3;
                                        break;
                                    case 1:
                                        Kart3.placement = 4;
                                        break;
                                }

                                break;

                            case 3:
                                Kart4.placement = selectedKart + 1;

                                switch (Kart4.placement)
                                {
                                    case 4:
                                        Kart4.placement = 1;
                                        break;
                                    case 3:
                                        Kart4.placement = 2;
                                        break;
                                    case 2:
                                        Kart4.placement = 3;
                                        break;
                                    case 1:
                                        Kart4.placement = 4;
                                        break;
                                }

                                break;
                        }
                    }
                }
            }

            currentPlacement[0] = Kart1.placement;
            currentPlacement[1] = Kart2.placement;
            currentPlacement[2] = Kart3.placement;
            currentPlacement[3] = Kart4.placement;


            // Play Passing Sound
            for (int i = 0; i < 4; i++)
            {
                if (currentPlacement[i] > previousPlacement[i] && KartSettings.start)
                {
                    KartSettings.Audio.Play("Pass");
                }
                else if (currentPlacement[i] < previousPlacement[i] && KartSettings.start)
                {
                    KartSettings.Audio.Play("Passed");
                }
            }


            // End the game when all players Finish
            if (Kart1.win && (Kart2.win || KartSettings.maxPlayers < 2) && (Kart3.win || KartSettings.maxPlayers < 3) && (Kart4.win || KartSettings.maxPlayers < 4))
            {
                if (timer == 0)
                {
                    KartSettings.Audio.StopSound("Chandler Music Day");
                    KartSettings.Audio.StopSound("Chandler Music Night");
                    KartSettings.Audio.StopSound("Belknap Music Day");
                    KartSettings.Audio.StopSound("Belknap Music Night");
                    KartSettings.Audio.StopSound("Furey Music Day");
                    KartSettings.Audio.StopSound("Furey Music Night");

                    KartSettings.Audio.Play("End");
                }

                if (timer > 10)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                else
                {
                    timer += Time.deltaTime;
                }
            }
        }
    }
}