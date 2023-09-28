using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class TestTimer_PrintHelloWorld : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TimerManager.Instance.AddTimer(PrintName, 3.0f, 2);
        TimerManager.Instance.AddTimer(PrintHelloWorld, 1.0f, 10);
    }

    void PrintHelloWorld()
    {
        Debug.Log("Hello World!");
    }

    void PrintName()
    {
        Debug.Log("Andy");
    }
}
