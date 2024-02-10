using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private BestTime bestTime;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI timerShadow;
    public GameObject lose;
    public GameObject win;
    public TextMeshProUGUI bestTimer;
    public TextMeshProUGUI bestTimerShadow;
    public RawImage freeze;

    private void Awake()
    {
        while (!bestTime)
        {
            bestTime = GameManager.BestTimer;
            bestTimer.text = bestTime.bestTime;
            bestTimerShadow.text = bestTimer.text;
        }
    }

    private void Update()
    {
        // Freeze Time
        if (GameManager.Instance.gameStarted && !GameManager.Instance.start && !GameManager.Instance.win)
        {
            lose.active = true;
            Time.timeScale = 0;
        }
        else if (GameManager.Instance.gameStarted && !GameManager.Instance.start && GameManager.Instance.win)
        {
            win.active = true;
            Time.timeScale = 0;

            // Update best floor count
            bestTime.bestTime = "FLOOR " + (bestTime.floors + 1);
        }
        else
        {
            Time.timeScale = 1;
        }

        // Countdown
        if (GameManager.Instance.start)
        {
            // Change timer float to a readable time value
            float countdown = 10 - GameManager.Instance.timer;
            string time = (countdown - ((int)countdown)).ToString();

            if (time.Length > 2)
            {
                time = time.Remove(0, 2);
            }

            while (time.Length > 2)
            {
                time = time.Remove(time.Length - 1, 1);
            }

            if (time.Length == 1)
            {
                time += "0";
            }

            time = ((int)countdown).ToString() + "." + time;

            timer.text = "TIME: " + time;

            // Update timer colors
            if (GameManager.Instance.timer < 4)
            {
                timer.color = Color.green;
            }
            else if (GameManager.Instance.timer < 7)
            {
                timer.color = Color.yellow;
            }
            else
            {
                timer.color = Color.red;
            }

            // Freezing camera
            freeze.color = new Vector4(255, 255, 255, GameManager.Instance.timer * 17.5f) / 255;
        }
        else if (!GameManager.Instance.win)
        {
            timer.text = "TIME: 0.00";
        }

        timerShadow.text = timer.text;
    }
}
