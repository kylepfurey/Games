
// Game Manager Template Script
// by Kyle Furey

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static Mouse Player { get { if (!player) { player = FindObjectOfType<Mouse>(); } return player; } set { Player = value; } }
    private static Mouse player;

    public static Timer Timer { get { if (!timer) { timer = FindObjectOfType<Timer>(); } return timer; } set { Timer = value; } }
    private static Timer timer;

    public static AudioManager Audio { get { if (!audio) { audio = FindObjectOfType<AudioManager>(); } return audio; } set { Audio = value; } }
    private static AudioManager audio;

    private void Awake()
    {
        Instance = this;
    }
}
