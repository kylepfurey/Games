using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowUntilStart : MonoBehaviour
{
    [SerializeField]
    public CanvasGroup text;
    public Title title;

    void Start()
    {
        text.alpha = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (title.start == true)
        {
            text.alpha = 0;
        }
    }
}
