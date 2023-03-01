using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class Screen1 : MonoBehaviour
{
    public Screen screen;
    public MeshRenderer screen1;

    private void Update()
    {
        if (screen.screen1 == true)
        {
            screen1.enabled = true;
        } 
        else
        {
            screen1.enabled = false;
        }
    }
}
