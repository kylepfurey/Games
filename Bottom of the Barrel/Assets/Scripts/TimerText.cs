using TMPro;
using UnityEngine;

public class TimerText : MonoBehaviour
{
    public TextMeshPro timer;

    private void Update()
    {
        if (GameManager.Timer != null)
        {
            timer.text = GameManager.Timer.displayedTime;
        }
    }
}
