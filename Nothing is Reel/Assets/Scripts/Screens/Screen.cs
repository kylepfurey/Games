using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen : MonoBehaviour
{
    public bool screen1;
    public bool screen2;
    public bool screen3;
    public bool screen4;

    public int screen = 1;
    public int screenTimer = -200;

    // Length of each video.
    public int timer1 = -200;
    public int timer2 = -200;
    public int timer3 = -200;
    public int timer4 = -200;

    void Awake()
    {
        screen = 1;
        screenTimer = timer1;
        screen1 = true;
        screen2 = false;
        screen3 = false;
        screen4 = false;
    }

    // Timer and screen swap.
    void FixedUpdate()
    {
        if (screenTimer < 0)
        {
            screenTimer = screenTimer + 1;
        }

        if (screenTimer >= 0)
        {
            screen++;

            if (screen == 5)
            {
                screen = 1;
                screen1 = true;
                screen4 = false;
                screen3 = false;
                screen2 = false;
            }

            if (screen == 1)
            {
                screen1 = true;
                screenTimer = timer1;
                screen4 = false;
                screen3 = false;
                screen2 = false;
            }

            if (screen == 2)
            {
                screen2 = true;
                screenTimer = timer2;
                screen4 = false;
                screen3 = false;
                screen1 = false;
            }

            if (screen == 3)
            {
                screen3 = true;
                screenTimer = timer3;
                screen4 = false;
                screen2 = false;
                screen1 = false;
            }

            if (screen == 4)
            {
                screen4 = true;
                screenTimer = timer4;
                screen3 = false;
                screen2 = false;
                screen1 = false;
            }
        }
    }
}