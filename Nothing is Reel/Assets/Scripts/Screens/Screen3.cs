using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class Screen3: MonoBehaviour
{
    public Screen screen;
    public MeshRenderer screen3;

    private void Update()
    {
        if (screen.screen3 == true)
        {
            screen3.enabled = true;
        } 
        else
        {
            screen3.enabled = false;
        }
    }
}
