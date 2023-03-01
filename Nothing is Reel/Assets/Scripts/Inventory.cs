using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Monster monster;

    public AudioSource collect;
    public AudioSource collectLast;

     void Start()
    {
        reelFive = false;
        reelAll = false;
        reels = 0;
    }

    public int reels { get; private set; }
    public bool reelFive { get; private set; }
    public bool reelAll { get; private set; }

    public UnityEvent<Inventory> reelCollected;

    // How many Reels you have.
    public void collected()
    {
        reels++;
        reelCollected.Invoke(this);

        if (reels == 5)
        {
            reelAll = true;
            monster.movementStart = true;
            collectLast.Play();
        }

        if (reels == 4)
        {
            reelFive = true;
        }
        
        if (reels < 5)
        {
            collect.Play();
        }
    }
}