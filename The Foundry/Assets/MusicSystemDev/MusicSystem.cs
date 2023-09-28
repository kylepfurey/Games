using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//This is a script to control the active soundtrack. It has functionality to fade them in and out.

public class MusicSystem : MonoBehaviour
{
    private List<MusicUnit> sources = new List<MusicUnit>();

    // Start is called before the first frame update
    void Start()
    {
        sources = GetComponentsInChildren<MusicUnit>().ToList();
    }

    //Call this function via a Unity Event with the desired index to play the music
    public void PlayMusic(int index)
    {
        StopAll();
        sources[index].Play();
    }

    public void StopAll()
    {
        foreach (MusicUnit source in sources)
        {
            source.Stop();
        }
    }

    public void StopAllInstantly()
    {
        foreach (MusicUnit source in sources)
        {
            source.Stop();
            source.timeElapsed = source.lerpDuration;
        }
    }
}
