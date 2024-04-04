
// Game Manager Template Script
// by Kyle Furey

using UnityEngine;

// A component that allows easy access to other components through the singleton design pattern.
public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance;

    [HideInInspector]
    public static PlayerMovement Player
    {
        get
        {
            if (!player)
            {
                player = FindObjectOfType<PlayerMovement>();
            }

            return player;
        }
        set
        {
            Player = value;
        }
    }

    private static PlayerMovement player;

    private void Awake()
    {
        Instance = this;
    }
}
