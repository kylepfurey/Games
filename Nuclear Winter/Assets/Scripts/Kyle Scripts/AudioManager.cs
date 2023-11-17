using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioInstance[] Audio;

    // Audio Instance Class
    [System.Serializable]
    private class AudioInstance
    {
        public string Name;
        public AudioSource Source;
    }

    // Start playing a sound
    public void Play(string sound)
    {
        for (int i = 0; i < Audio.Length; i++)
        {
            if (sound == Audio[i].Name && Audio[i] != null && Audio[i].Source.clip != null)
            {
                Audio[i].Source.Stop();
                Audio[i].Source.time = 0;
                Audio[i].Source.Play();
                break;
            }
        }
    }

    public void Play(string sound, float startTime)
    {
        for (int i = 0; i < Audio.Length; i++)
        {
            if (sound == Audio[i].Name && Audio[i] != null && Audio[i].Source.clip != null)
            {
                Audio[i].Source.Stop();
                Audio[i].Source.time = startTime;
                Audio[i].Source.Play();
                break;
            }
        }
    }

    // Stop playing a designated sound
    public void Stop(string sound)
    {
        for (int i = 0; i < Audio.Length; i++)
        {
            if (sound == Audio[i].Name && Audio[i] != null && Audio[i].Source.clip != null)
            {
                if (Audio[i].Source.isPlaying)
                {
                    Audio[i].Source.Stop();
                    Audio[i].Source.time = 0;
                }
                break;
            }
        }
    }

    // Play a sound once
    public void PlayOnce(string sound)
    {
        for (int i = 0; i < Audio.Length; i++)
        {
            if (sound == Audio[i].Name && Audio[i] != null && Audio[i].Source.clip != null)
            {
                Audio[i].Source.PlayOneShot(Audio[i].Source.clip);
                break;
            }
        }
    }

    public void PlayOnce(string sound, float volume)
    {
        for (int i = 0; i < Audio.Length; i++)
        {
            if (sound == Audio[i].Name && Audio[i] != null && Audio[i].Source.clip != null)
            {
                Audio[i].Source.PlayOneShot(Audio[i].Source.clip, volume);
                break;
            }
        }
    }


    // Play a sound after another sound ends
    public void PlayMusic(string opening, string music)
    {
        for (int i = 0; i < Audio.Length; i++)
        {
            if (opening == Audio[i].Name && Audio[i] != null && Audio[i].Source.clip != null)
            {
                Audio[i].Source.Stop();
                Audio[i].Source.time = 0;
                Audio[i].Source.Play();

                for (int j = 0; j < Audio.Length; j++)
                {
                    if (music == Audio[j].Name && Audio[j] != null && Audio[j].Source.clip != null)
                    {
                        Audio[j].Source.Stop();
                        Audio[j].Source.time = 0;
                        Audio[j].Source.PlayDelayed(Audio[i].Source.clip.length);
                        break;
                    }
                }
                break;
            }
        }
    }

    public void PlayMusic(string opening, float openingStartTime, string music)
    {
        for (int i = 0; i < Audio.Length; i++)
        {
            if (opening == Audio[i].Name && Audio[i] != null && Audio[i].Source.clip != null)
            {
                Audio[i].Source.Stop();
                Audio[i].Source.time = openingStartTime;
                Audio[i].Source.Play();

                for (int j = 0; j < Audio.Length; j++)
                {
                    if (music == Audio[j].Name && Audio[j] != null && Audio[j].Source.clip != null)
                    {
                        Audio[j].Source.Stop();
                        Audio[j].Source.time = 0;
                        Audio[j].Source.PlayDelayed(Audio[i].Source.clip.length - openingStartTime);
                        break;
                    }
                }
                break;
            }
        }
    }

    public void PlayMusic(string opening, string music, float musicStartTime)
    {
        for (int i = 0; i < Audio.Length; i++)
        {
            if (opening == Audio[i].Name && Audio[i] != null && Audio[i].Source.clip != null)
            {
                Audio[i].Source.Stop();
                Audio[i].Source.time = 0;
                Audio[i].Source.Play();

                for (int j = 0; j < Audio.Length; j++)
                {
                    if (music == Audio[j].Name && Audio[j] != null && Audio[j].Source.clip != null)
                    {
                        Audio[j].Source.Stop();
                        Audio[j].Source.time = musicStartTime;
                        Audio[j].Source.PlayDelayed(Audio[i].Source.clip.length);
                        break;
                    }
                }
                break;
            }
        }
    }

    public void PlayMusic(string opening, float openingStartTime, string music, float musicStartTime)
    {
        for (int i = 0; i < Audio.Length; i++)
        {
            if (opening == Audio[i].Name && Audio[i] != null && Audio[i].Source.clip != null)
            {
                Audio[i].Source.Stop();
                Audio[i].Source.time = openingStartTime;
                Audio[i].Source.Play();

                for (int j = 0; j < Audio.Length; j++)
                {
                    if (music == Audio[j].Name && Audio[j] != null && Audio[j].Source.clip != null)
                    {
                        Audio[j].Source.Stop();
                        Audio[j].Source.time = musicStartTime;
                        Audio[j].Source.PlayDelayed(Audio[i].Source.clip.length - openingStartTime);
                        break;
                    }
                }
                break;
            }
        }
    }

    private void Start()
    {
        enabled = true;
    }
}
