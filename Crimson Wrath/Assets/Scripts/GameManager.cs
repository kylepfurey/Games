using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static Player Player { get { if (!player) { player = FindObjectOfType<Player>(); } return player; } set { Player = value; } }
    private static Player player;

    public static AudioManager Audio { get { if (!audio) { audio = FindObjectOfType<AudioManager>(); } return audio; } set { Audio = value; } }
    private static AudioManager audio;

    public static DialogueManager Dialogue { get { if (!dialogue) { dialogue = FindObjectOfType<DialogueManager>(); } return dialogue; } set { Dialogue = value; } }
    private static DialogueManager dialogue;

    private void Awake()
    {
        Instance = this;
    }
}
