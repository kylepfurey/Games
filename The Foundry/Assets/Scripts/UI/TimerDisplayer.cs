using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerDisplayer : MonoBehaviour
{
    [SerializeField]
    private Timer timer;

    [SerializeField]
    private TMP_Text text;

    private void OnValidate()
    {
        if (!timer)
        {
            timer = FindObjectOfType<Timer>(true);
        }

        if (!text)
        {
            text = GetComponent<TMP_Text>();
        }
    }

    private void Start()
    {
        if (!timer)
        {
            timer = FindObjectOfType<Timer>(true);
        }
    }

    private void Update()
    {
        int minutes = (int)(timer.currentTimeInSeconds / 60f);
        int seconds = (int)(timer.currentTimeInSeconds % 60f);

        text.text = $"Total Time: {minutes}:{seconds:00}";

        if (!timer.shouldIncreaseTime)
        {
            text.color = Color.green;
        }
        else
        {
            text.color = Color.red;
        }
    }
}
