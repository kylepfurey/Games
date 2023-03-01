using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HideUntilStart : MonoBehaviour
{
    [SerializeField]
    public CanvasGroup text;
    public Title title;

    void Start()
    {
        text.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (title.start == true)
        {
            text.alpha = 0.5f;
        }
    }
}
