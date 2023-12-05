using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioCategory[] audioCategories;
    [HideInInspector] public AudioInstance[] audio;

    // Audio Instance Class
    [System.Serializable]
    public class AudioInstance
    {
        public string Name;
        public AudioSource Source;
    }

    // Audio Category Class
    [System.Serializable]
    public class AudioCategory
    {
        public string name;
        public AudioInstance[] audio;
    }

    private void Start()
    {
        // Cumulate Audio Instances in One Array
        List<AudioInstance> list = new List<AudioInstance>();

        foreach (AudioCategory category in audioCategories)
        {
            foreach (AudioInstance sound in category.audio)
            {
                list.Add(sound);
            }
        }

        audio = list.ToArray();

        list.Clear();
    }

    // Start playing a sound
    public void Play(string sound)
    {
        for (int i = 0; i < audio.Length; i++)
        {
            if (sound.ToLower() == audio[i].Name.ToLower() && audio[i] != null && audio[i].Source.clip != null)
            {
                audio[i].Source.Stop();
                audio[i].Source.time = 0;
                audio[i].Source.Play();
                break;
            }
        }
    }

    public void Play(string sound, float startTime)
    {
        for (int i = 0; i < audio.Length; i++)
        {
            if (sound.ToLower() == audio[i].Name.ToLower() && audio[i] != null && audio[i].Source.clip != null)
            {
                audio[i].Source.Stop();
                audio[i].Source.time = startTime;
                audio[i].Source.Play();
                break;
            }
        }
    }

    // Stop playing a designated sound
    public void Stop(string sound)
    {
        for (int i = 0; i < audio.Length; i++)
        {
            if (sound.ToLower() == audio[i].Name.ToLower() && audio[i] != null && audio[i].Source.clip != null)
            {
                if (audio[i].Source.isPlaying)
                {
                    audio[i].Source.Stop();
                    audio[i].Source.time = 0;
                }
                break;
            }
        }
    }

    // Play a sound once
    public void PlayOnce(string sound)
    {
        for (int i = 0; i < audio.Length; i++)
        {
            if (sound.ToLower() == audio[i].Name.ToLower() && audio[i] != null && audio[i].Source.clip != null)
            {
                audio[i].Source.PlayOneShot(audio[i].Source.clip);
                break;
            }
        }
    }

    public void PlayOnce(string sound, float volume)
    {
        for (int i = 0; i < audio.Length; i++)
        {
            if (sound.ToLower() == audio[i].Name.ToLower() && audio[i] != null && audio[i].Source.clip != null)
            {
                audio[i].Source.PlayOneShot(audio[i].Source.clip, volume);
                break;
            }
        }
    }


    // Play a sound after another sound ends
    public void PlayMusic(string opening, string music)
    {
        for (int i = 0; i < audio.Length; i++)
        {
            if (opening.ToLower() == audio[i].Name.ToLower() && audio[i] != null && audio[i].Source.clip != null)
            {
                audio[i].Source.Stop();
                audio[i].Source.time = 0;
                audio[i].Source.Play();

                for (int j = 0; j < audio.Length; j++)
                {
                    if (music.ToLower() == audio[j].Name.ToLower() && audio[j] != null && audio[j].Source.clip != null)
                    {
                        audio[j].Source.Stop();
                        audio[j].Source.time = 0;
                        audio[j].Source.PlayDelayed(audio[i].Source.clip.length);
                        break;
                    }
                }
                break;
            }
        }
    }

    public void PlayMusic(string opening, float openingStartTime, string music)
    {
        for (int i = 0; i < audio.Length; i++)
        {
            if (opening.ToLower() == audio[i].Name.ToLower() && audio[i] != null && audio[i].Source.clip != null)
            {
                audio[i].Source.Stop();
                audio[i].Source.time = openingStartTime;
                audio[i].Source.Play();

                for (int j = 0; j < audio.Length; j++)
                {
                    if (music.ToLower() == audio[j].Name.ToLower() && audio[j] != null && audio[j].Source.clip != null)
                    {
                        audio[j].Source.Stop();
                        audio[j].Source.time = 0;
                        audio[j].Source.PlayDelayed(audio[i].Source.clip.length - openingStartTime);
                        break;
                    }
                }
                break;
            }
        }
    }

    public void PlayMusic(string opening, string music, float musicStartTime)
    {
        for (int i = 0; i < audio.Length; i++)
        {
            if (opening.ToLower() == audio[i].Name.ToLower() && audio[i] != null && audio[i].Source.clip != null)
            {
                audio[i].Source.Stop();
                audio[i].Source.time = 0;
                audio[i].Source.Play();

                for (int j = 0; j < audio.Length; j++)
                {
                    if (music.ToLower() == audio[j].Name.ToLower() && audio[j] != null && audio[j].Source.clip != null)
                    {
                        audio[j].Source.Stop();
                        audio[j].Source.time = musicStartTime;
                        audio[j].Source.PlayDelayed(audio[i].Source.clip.length);
                        break;
                    }
                }
                break;
            }
        }
    }

    public void PlayMusic(string opening, float openingStartTime, string music, float musicStartTime)
    {
        for (int i = 0; i < audio.Length; i++)
        {
            if (opening.ToLower() == audio[i].Name.ToLower() && audio[i] != null && audio[i].Source.clip != null)
            {
                audio[i].Source.Stop();
                audio[i].Source.time = openingStartTime;
                audio[i].Source.Play();

                for (int j = 0; j < audio.Length; j++)
                {
                    if (music.ToLower() == audio[j].Name.ToLower() && audio[j] != null && audio[j].Source.clip != null)
                    {
                        audio[j].Source.Stop();
                        audio[j].Source.time = musicStartTime;
                        audio[j].Source.PlayDelayed(audio[i].Source.clip.length - openingStartTime);
                        break;
                    }
                }
                break;
            }
        }
    }
}