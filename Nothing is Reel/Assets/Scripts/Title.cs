using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    public PlayerMovement player;
    public bool start;

    void Start()
    {
        start = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (start == false)
            {
                start = true;
            }
        }
    }
}