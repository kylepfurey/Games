using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Lazy Loading
    private static Player PlayerReference;

    public static Player Player
    {
        get
        {
            if (PlayerReference == null)
            {
                PlayerReference = FindFirstObjectByType<Player>();
            }

            return PlayerReference;
        }

        set
        {
            PlayerReference = value;
        }
    }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}