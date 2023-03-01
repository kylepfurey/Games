using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public PlayerMovement player;

    [SerializeField]
    public CanvasGroup note;
    public Title title;
    public float timer;

    public AudioSource filmStart;

    void Start()
    {
        timer = 0;
        note.alpha = 0;
        player.movement = false;
    }

    void Update()
    {
        if (title.start == true)
        {
            timer = timer + Time.deltaTime;
            if (timer > 0)
            {
                note.alpha = 1;
            }
        }
        else
        {
            note.alpha = 0;
            player.movement = false;
        }

        if (timer > 13)
        {
            note.alpha -= 1;
            player.movement = true;
        }

        if (timer > 12 && timer < 13)
        {
            filmStart.Play();
        }
    }
}
