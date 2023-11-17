using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private float timerStart;

    [SerializeField] private TextMeshProUGUI timerText;
    private TextMeshProUGUI timerShadowText;

    [SerializeField] private TextMeshProUGUI oxygenText;
    [SerializeField] private TextMeshProUGUI helmetText;
    [SerializeField] private TextMeshProUGUI suitText;

    private float timer;
    private float delayTimer;
    private string time;
    private Color obtained;
    private bool end;

    void Start()
    {
        timer = timerStart;

        timerShadowText = timerText.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        obtained = oxygenText.color;
        oxygenText.color = Color.white;
    }

    void Update()
    {
        if (delayTimer > delay)
        {
            if (timer > 0)
            {
                int minutes = (int)(timer / 60);
                float seconds = (int)(timer - minutes * 60);

                if (seconds > 9)
                {
                    time = minutes + ":" + seconds;
                }
                else
                {
                    time = minutes + ":0" + seconds;
                }

                timer -= Time.unscaledDeltaTime;

                timerShadowText.text = time;
            }
            else
            {
                timerShadowText.text = "0:00";

                if (!end)
                {
                    end = true;
                    GameManager.audio.PlayOnce("Shatter");
                    Invoke("RestoreTime", 1);
                }
            }

            timerText.text = timerShadowText.text;


            if (GameManager.player.oxygen)
            {
                oxygenText.color = obtained;
            }
            else
            {
                oxygenText.color = Color.white;
            }

            if (GameManager.player.helmet)
            {
                helmetText.color = obtained;
            }
            else
            {
                helmetText.color = Color.white;
            }

            if (GameManager.player.suit)
            {
                suitText.color = obtained;
            }
            else
            {
                suitText.color = Color.white;
            }
        }
        else
        {
            delayTimer += Time.deltaTime;
        }
    }

    private void RestoreTime()
    {
        GameManager.timeScale = 1;
        GameManager.audio.PlayOnce("Rumble");
    }
}
