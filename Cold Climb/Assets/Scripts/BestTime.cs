using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestTime : MonoBehaviour
{
    public AudioManager Music;
    public int floors = 0;
    public string bestTime = "";

    // Track the best time
    private void Awake()
    {
        Music = GetComponent<AudioManager>();
        floors = 0;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        // Play music
        Music.Play("Cold");
    }
}
