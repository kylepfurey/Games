using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class Screen4: MonoBehaviour
{
    public Screen screen;
    public MeshRenderer screen4;

    private void Update()
    {
        if (screen.screen4 == true)
        {
            screen4.enabled = true;
        } 
        else
        {
            screen4.enabled = false;
        }
    }
}
