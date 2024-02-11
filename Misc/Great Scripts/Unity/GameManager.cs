
// Game Manager Template Script
// by Kyle Furey

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static Player Player { get { if (!player) { player = FindObjectOfType<Player>(); } return player; } set { Player = value; } }
    private static Player player;

    private void Awake()
    {
        Instance = this;
    }
}
