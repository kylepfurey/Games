using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<AudioManager>();
            }
            return _instance;
        }
    }
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        DontDestroyOnLoad(gameObject);
    }


    [Header("------Audio Source------")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource SFXSource;

    [Header("------Audio Clip------")]
    public AudioClip background;
    public AudioClip footstep;
    public AudioClip death;
    public AudioClip pickUpGood;
    public AudioClip pickUpBad;
    public AudioClip fire;
    public AudioClip jump;
    public AudioClip pop;
    public AudioClip button;
    public AudioClip footsteps;

    private void Start()
    {
        musicSource.clip = fire;
        musicSource.Play();

    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void StartGame()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void StartNewGame()
    {
        musicSource.clip = fire;
        musicSource.Play();
    }

    public void ButtonClicked()
    {
        PlaySFX(button);
    }

}
