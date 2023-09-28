using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
//INFO TERMINAL
//This script is intended for the "InfoTerminalScreen" prefab. It should allow us to have interactable screens for expository purposes
//WRITTEN BY LIAM SHELTON
public class InfoTerminal : MonoBehaviour
{
    //Set in the inspector
    public GameObject screenPivot;
    public TextMeshPro textMeshPro;
    public GameObject soundFXPrefabTurnOn;
    public GameObject soundFXPrefabTurnOff;

    //text crawl
    private string originalString;
    private float crawlTimer;
    public float crawlDelay = .02f;

    //screen lerp
    public float screenLerpTime = .2f;
    private bool turningOn;



    // Start is called before the first frame update
    void Start()
    {
        originalString = textMeshPro.text;
        screenPivot.transform.localScale = new Vector3(1, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (turningOn)
        {
            TurningOnUpdate();
        }
        else
        {
            TurningOffUpdate();
        }
    }

    //Call this to turn the screen on
    public void TurnOn()
    {
        turningOn = true;
        textMeshPro.text = "";
        lerpStart = Time.time;
        scaleVal = 0f;
        GameObject.Instantiate(soundFXPrefabTurnOn, transform.position, transform.rotation);
    }

    //Call this to turn the screen off
    public void TurnOff()
    {
        turningOn = false;
        lerpStart = Time.time + screenLerpTime;
        scaleVal = screenLerpTime;
        //GameObject.Instantiate(soundFXPrefabTurnOff, transform.position, transform.rotation);
    }
    private void TurningOnUpdate()
    {
        TextCrawl();
        TurnOnLerp();
    }

    private void TextCrawl()
    {
        if (textMeshPro.text.Length <= originalString.Length)
        {
            if (Time.time > crawlTimer)
            {
                textMeshPro.text = textMeshPro.text.TrimEnd('_');
                crawlTimer = Time.time + crawlDelay;
                textMeshPro.text = textMeshPro.text + originalString[textMeshPro.text.Length] + "_";
            }
        }
    }

    private float lerpStart;
    private float scaleVal;
    private void TurnOnLerp()
    {
        if (scaleVal < 1)
        {
            scaleVal = (Time.time - lerpStart) / screenLerpTime;
            Vector3 theScale = new Vector3(1, 0, 1);
            theScale.y = scaleVal;
            screenPivot.transform.localScale = theScale;
        }
    }

    private void TurningOffUpdate()
    {
        if (scaleVal > 0)
        {
            scaleVal = (lerpStart - Time.time) / screenLerpTime;
            Vector3 theScale = new Vector3(1, 0, 1);
            theScale.y = scaleVal;
            screenPivot.transform.localScale = theScale;
        }
        else
        {
            screenPivot.transform.localScale = new Vector3(1, 0, 1);
        }
    }
}
