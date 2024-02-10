using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    UI UI;
    private void Awake()
    {
        UI = FindObjectOfType<UI>();
    }

    // A trigger used to inform the game the player has won
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<p_movement>())
        {
            GameManager.Instance.win = true;
            GameManager.Instance.start = false;
            GameManager.Instance.Audio.PlayOnce("Win");
            GameManager.BestTimer.floors++;
        }
    }
}
