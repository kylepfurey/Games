using UnityEngine.SceneManagement;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [HideInInspector] public float timer = 0;
    [HideInInspector] public float bestTime = 0;
    [HideInInspector] public string displayedTime = "";
    [HideInInspector] public string displayedBestTime = "";
    [HideInInspector] public bool timerGo = false;

    // Dirtily built timer

    private void Awake()
    {
        if (GameManager.Timer != this)
        {
            Destroy(gameObject);
        }

        bestTime = Mathf.Infinity;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // Exiting game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            GameManager.Audio.Stop("Sleighbells");

            GameManager.Audio.Stop("Shake");

            GameManager.Audio.Stop("Sleighbells");

            GameManager.Audio.Stop("Bubbles");

            GameManager.Audio.Stop("Teleport");

            SceneManager.LoadScene("Title");
         
            timer = 0;
        }
        else if (timerGo)
        {
            timer += Time.deltaTime;

            displayedTime = ConvertTimer(timer);
        }
    }

    // Converts a timer float into a readable string 
    public static string ConvertTimer(float timer)
    {
        // Change timer float to a readable time value
        float seconds = timer;

        string milliseconds = (seconds - ((int)seconds)).ToString();

        if (milliseconds.Length > 2)
        {
            milliseconds = milliseconds.Remove(0, 2);
        }

        while (milliseconds.Length > 2)
        {
            milliseconds = milliseconds.Remove(milliseconds.Length - 1, 1);
        }

        if (milliseconds.Length == 1)
        {
            milliseconds += "0";
        }

        int minutes = 0;

        while (seconds > 60)
        {
            minutes += 1;

            seconds -= 60;
        }

        if (seconds < 10 && minutes < 10)
        {
            return "0" + minutes.ToString() + ".0" + ((int)seconds).ToString() + "." + milliseconds;
        }
        else if (seconds < 10)
        {
            return minutes.ToString() + ".0" + ((int)seconds).ToString() + "." + milliseconds;
        }
        else if (minutes < 10)
        {
            return "0" + minutes.ToString() + "." + ((int)seconds).ToString() + "." + milliseconds;
        }
        else
        {
            return minutes.ToString() + "." + ((int)seconds).ToString() + "." + milliseconds;
        }
    }
}
