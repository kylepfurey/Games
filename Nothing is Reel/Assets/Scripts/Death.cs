using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

public class Death : MonoBehaviour
{
    public Hitbox hitbox;
    public VideoPlayer videoPlayer;
    public GameObject videoObject;

    void Start()
    {
        videoObject = GameObject.Find("Death Clip");
        videoObject.GetComponent<VideoPlayer>().targetCameraAlpha = 0;
    }

    // When you die, Death video fades in.
    void FixedUpdate()
    {
        if (hitbox.death == true)
        {
            if (videoObject.GetComponent<VideoPlayer>().targetCameraAlpha < 1.5)
            {
                videoObject.GetComponent<VideoPlayer>().targetCameraAlpha = videoObject.GetComponent<VideoPlayer>().targetCameraAlpha + 0.3f * Time.fixedDeltaTime;
            }

            if (videoObject.GetComponent<VideoPlayer>().targetCameraAlpha >= 1.5)
            {
                Debug.Log("Game closes.");
                Application.Quit();
            }
        }
    }
}