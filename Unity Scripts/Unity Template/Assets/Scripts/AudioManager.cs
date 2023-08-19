using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public string[] Name;
    public AudioSource[] Audio;

    // Play a sound once
    public void Play(string sound)
    {
        for (int i = 0; i < Name.Length; i++)
        {
            if (sound == Name[i] && Audio[i] != null && Audio[i].clip != null)
            {
                Audio[i].PlayOneShot(Audio[i].clip);
                break;
            }
        }
    }

    // Play a sound after another sound ends
    public void PlayMusic(string opening, float openingStartTime, string music, float musicStartTime)
    {
        for (int i = 0; i < Name.Length; i++)
        {
            if (opening == Name[i] && Audio[i] != null && Audio[i].clip != null)
            {
                Audio[i].Stop();
                Audio[i].time = openingStartTime;
                Audio[i].Play();

                for (int j = 0; j < Name.Length; j++)
                {
                    if (music == Name[j] && Audio[j] != null && Audio[j].clip != null)
                    {
                        Audio[j].Stop();
                        Audio[j].time = musicStartTime;
                        Audio[j].PlayDelayed(Audio[i].clip.length - openingStartTime);
                        break;
                    }
                }
                break;
            }
        }
    }

    // Start playing a sound
    public void StartSound(string sound, float startTime)
    {
        for (int i = 0; i < Name.Length; i++)
        {
            if (sound == Name[i] && Audio[i] != null && Audio[i].clip != null)
            {
                Audio[i].Stop();
                Audio[i].time = startTime;
                Audio[i].Play();
                break;
            }
        }
    }

    // Stop playing a designated sound
    public void StopSound(string sound)
    {
        for (int i = 0; i < Name.Length; i++)
        {
            if (sound == Name[i] && Audio[i] != null && Audio[i].clip != null)
            {
                if (Audio[i].isPlaying)
                {
                    Audio[i].Stop();
                    Audio[i].time = 0;
                }
                break;
            }
        }
    }

    void Start()
    {
        enabled = true;
    }
}