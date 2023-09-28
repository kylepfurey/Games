using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    private static Timer instance = null;
    public static Timer Instance => instance;

    public float currentTimeInSeconds = 0.0f;

    public bool shouldIncreaseTime = true;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        currentTimeInSeconds = 0.0f;
    }

    private void Update()
    {
        if (shouldIncreaseTime)
        {
            currentTimeInSeconds += Time.deltaTime;
        }
    }
}
