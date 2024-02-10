
// Game Manager Template Script
// Made for educational purposes.
// Copyright © 2023 by Kyle Furey

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static p_movement Player { get { if (!player) { player = FindObjectOfType<p_movement>(); } return player; } set { Player = value; } }
    private static p_movement player;

    public static cam_follow Camera { get { if (!camera) { camera = FindObjectOfType<cam_follow>(); } return camera; } set { Camera = value; } }
    private static cam_follow camera;

    public AudioManager Audio
    {
        get
        {
            if (!audio)
            {
                audio = GetComponent<AudioManager>();
            }
            return audio;
        }
        set { audio = value; }
    }
    private static AudioManager audio;

    public static RandomModular Modules { get { if (!modules) { modules = FindObjectOfType<RandomModular>(); } return modules; } set { modules = value; } }
    private static RandomModular modules;

    public static BestTime BestTimer { get { if (!bestTimer) { bestTimer = FindObjectOfType<BestTime>(); } return bestTimer; } set { bestTimer = value; } }
    private static BestTime bestTimer;

    public float timer = 0;
    public bool start = false;
    public bool gameStarted = false;
    public bool win = false;

    // Floor generation
    public GameObject topFloor;
    public GameObject floorPrefab;

    private void Awake()
    {
        Instance = this;
        start = true;
        gameStarted = true;
    }

    private void Update()
    {
        // 10 Second Timer
        if (start)
        {
            if (timer > 10)
            {
                EndGame();
            }

            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }

        // Restart
        if (gameStarted && !start)
        {
            if (Player.Button("Jump"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void StartGame()
    {
        // Starting the game
        gameStarted = true;
    }

    public void EndGame()
    {
        // Ending the game
        start = false;
        Audio.PlayOnce("Lose");
    }

    public void GenerateFloors()
    {
        // Generates the number of floors in the game
        topFloor.transform.position = new Vector3(0, BestTimer.floors * 5, 0);

        for (int i = 0; i < BestTimer.floors; i++)
        {
            GameObject floor = Instantiate(floorPrefab);
            floor.transform.position = new Vector3(0, i * 5, 0);
            Modules.sets.Add(floor.GetComponent<FloorData>().set);
        }

        Modules.SpawnAll();
    }
}
