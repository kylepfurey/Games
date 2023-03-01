using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// https://www.youtube.com/watch?v=EfUCEwKmcjc SOURCE
public class UI : MonoBehaviour
{
    public TextMeshProUGUI text;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateReelsUI(Inventory inventory)
    {
        text.text = inventory.reels.ToString();
    }
}
