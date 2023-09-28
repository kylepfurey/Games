using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonStart : MonoBehaviour
{
    [SerializeField]
    private Timer timer;

    [SerializeField]
    private TMP_InputField timeInputField;

    private void OnValidate()
    {
        if (!timer)
        {
            timer = FindObjectOfType<Timer>(true);
        }
    }
}
