using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//MUSIC UNIT
//This script should be attached to a child of the Music System gameobject (see Assets/MusicSystemDev/MusicSystemExample.prefab for example)
//The goal is to load the soundtracks preemptively to prevent stutter when music starts.
public class MusicUnit : MonoBehaviour
{
    public AudioSource mySource;
    public float targetVolume = 1f;

    public float timeElapsed = 0;
    public float lerpDuration = 4f;
    bool shouldPlay;

    private void Start()
    {
        mySource = GetComponent<AudioSource>();
        mySource.enabled = false;
    }
    //Call this to start the fadeout
    public void Stop()
    {
        shouldPlay = false;
        timeElapsed = 0f;
    }
    //Call this to start the fadein
    public void Play()
    {
        shouldPlay = true;
        timeElapsed = 0f;
        mySource.enabled = true;
        mySource.Play();
        mySource.volume = 0f;
    }

    public void Update()
    {
        if (shouldPlay == true)
        {
            StartingUpdate();

        }
        else
        {
            StoppingUpdate();
        }
    }

    private void StoppingUpdate()
    {
        mySource.volume = Mathf.Lerp(targetVolume, 0, timeElapsed / lerpDuration);
        timeElapsed += Time.deltaTime * 2;
    }
    private void StartingUpdate()
    {
        mySource.volume = Mathf.Lerp(0, targetVolume, timeElapsed / lerpDuration);
        timeElapsed += Time.deltaTime;
    }
}
