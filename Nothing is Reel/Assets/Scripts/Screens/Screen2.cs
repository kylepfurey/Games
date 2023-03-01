using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class Screen2 : MonoBehaviour
{
    public Screen screen;
    public MeshRenderer screen2;

    private void Update()
    {
        if (screen.screen2 == true)
        {
            screen2.enabled = true;
        } 
        else
        {
            screen2.enabled = false;
        }
    }
}
