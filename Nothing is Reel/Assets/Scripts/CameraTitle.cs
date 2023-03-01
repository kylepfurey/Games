using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraTitle : MonoBehaviour
{
    bool start = false;

    void Start()
    {
        gameObject.transform.localPosition = new Vector3(-6.64f, 0.478f, 35.685f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (start == false)
            {
                gameObject.transform.localPosition = new Vector3(0f, 0.478f, 0f);
                start = true;
            }
        }
    }
}