using UnityEngine;
using UnityEngine.InputSystem;

public class KartSettings : MonoBehaviour
{
    // Menu Control
    public MenuControl Menu;

    // Player Manager
    public int maxPlayers;
    public int joinedPlayers = 0;

    public PlayerInputManager Players;
    public AudioManager Audio;
    public GameObject Kart0;
    public GameObject Kart1;
    public GameObject Kart2;
    public GameObject Kart3;
    public GameObject Kart4;

    public bool allPlayersSpawned = false;

    // Karts will have different stats based on selected Character
    public string[] character = new string[5];

    // Four changeable stats, will range from 1-5
    public float[] maxSpeed = new float[5];         // Max Speed     =  How fast the Kart can possibly go
    public float[] acceleration = new float[5];     // Acceleration  =  How fast the Kart can climb to Max Speed
    public float[] traction = new float[5];         // Traction      =  How much resistance from the ground
    public float[] endurance = new float[5];        // Endurance     =  How much speed is lost when hitting something

    public float bumpModifier = 10000;

    // Map and Daytime
    public int map = 0;
    public bool night = false;

    public bool start = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        GameObject Object0 = GameObject.FindWithTag("Menu");
        if (Object0 != null)
        {
            Menu = Object0.GetComponent<MenuControl>();
        }

        GameObject Object1 = GameObject.FindWithTag("Audio");
        if (Object0 != null)
        {
            Audio = Object1.GetComponent<AudioManager>();
        }

        character[0] = "none";
        character[1] = "none";
        character[2] = "none";
        character[3] = "none";
        character[4] = "none";

        maxPlayers = 1;
    }

    void Update()
    {
        joinedPlayers = Players.playerCount;

        // Create Other Players
        if (allPlayersSpawned == false && Menu.menu == 2)
        {
            switch (maxPlayers)
            {
                case 1:
                    switch (joinedPlayers)
                    {
                        case 1:
                            Players.DisableJoining();
                            Instantiate(Kart2);
                            Instantiate(Kart3);
                            Instantiate(Kart4);
                            Instantiate(Kart0);
                            allPlayersSpawned = true;
                            break;
                    }
                    break;

                case 2:
                    switch (joinedPlayers)
                    {
                        case 1:
                            Players.playerPrefab = Kart2;
                            break;

                        case 2:
                            Players.DisableJoining();
                            Instantiate(Kart3);
                            Instantiate(Kart4);
                            Instantiate(Kart0);
                            allPlayersSpawned = true;
                            break;
                    }
                    break;

                case 3:
                    switch (joinedPlayers)
                    {
                        case 1:
                            Players.playerPrefab = Kart2;
                            break;

                        case 2:
                            Players.playerPrefab = Kart3;
                            break;

                        case 3:
                            Players.DisableJoining();
                            Instantiate(Kart4);
                            Instantiate(Kart0);
                            allPlayersSpawned = true;
                            break;
                    }
                    break;

                case 4:
                    switch (joinedPlayers)
                    {
                        case 1:
                            Players.playerPrefab = Kart2;
                            break;

                        case 2:
                            Players.playerPrefab = Kart3;
                            break;

                        case 3:
                            Players.playerPrefab = Kart4;
                            break;

                        case 4:
                            Players.DisableJoining();
                            Instantiate(Kart0);
                            allPlayersSpawned = true;
                            break;
                    }
                    break;
            }
        }

        // Change stats based on Character (12 max points)
        for (int i = 0; i < character.Length; i++)
        {
            if (character[i] == "Elizabeth")    // Purple
            {
                // Fast Kart
                maxSpeed[i] = 5;
                acceleration[i] = 2;
                traction[i] = 4;
                endurance[i] = 1;
            }

            if (character[i] == "Kim")          // Pink
            {
                // Standard Kart
                maxSpeed[i] = 3;
                acceleration[i] = 3;
                traction[i] = 3;
                endurance[i] = 3;
            }

            if (character[i] == "Ben")          // Cyan
            {
                // Slippery Kart
                maxSpeed[i] = 3;
                acceleration[i] = 5;
                traction[i] = 1;
                endurance[i] = 3;
            }

            if (character[i] == "Kelley")       // Orange
            {
                // Strong Kart
                maxSpeed[i] = 2.75f;
                acceleration[i] = 2;
                traction[i] = 3;
                endurance[i] = 5;
            }

            if (character[i] == "Kirsten")      // Green
            {
                // Power Kart
                maxSpeed[i] = 4;
                acceleration[i] = 2;
                traction[i] = 2;
                endurance[i] = 4;
            }

            if (character[i] == "Justin")       // Blue
            {
                // Technical Kart
                maxSpeed[i] = 3;
                acceleration[i] = 3;
                traction[i] = 5;
                endurance[i] = 1;
            }

            if (character[i] == "none")         // Default
            {
                // Default Kart
                maxSpeed[i] = 3;
                acceleration[i] = 3;
                traction[i] = 3;
                endurance[i] = 3;
            }
        }

        // Destroy Extra Players
        for (int i = 0; i < 5; i++)
        {
            if (GameObject.FindGameObjectsWithTag("Kart" + i).Length > 1)
            {
                GameObject[] destroyMe = GameObject.FindGameObjectsWithTag("Kart" + i);

                for (int j = 1; j < destroyMe.Length; j++)
                {
                    Destroy(destroyMe[j].transform.parent.gameObject);
                }
            }
        }
    }
}