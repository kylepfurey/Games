using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossRoomTriggerLol : MonoBehaviour
{
    public GameObject bossRoomShield;
    public GameObject gateCloseSoundFX;
    public BossWait theBossWaitLol;

    public bool fightStarted;
    public bool fightStartedStopper;

    public float fightDelay = 3f;
    public float fightTimer;


    public void ActivateTheBossStuff()
    {
        bossRoomShield.SetActive(true);
        GameObject.Instantiate(gateCloseSoundFX, transform.position, Quaternion.identity);

        fightStarted = true;
        fightTimer = Time.time + fightDelay;
    }

    private void Update()
    {
        if (fightStarted)
        {
            if (Time.time > fightTimer && fightStartedStopper == false)
            {
                fightStartedStopper = true;
                theBossWaitLol.StartTheFight();
            }
        }
    }
}
