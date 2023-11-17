using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private bool enable = true;
    [SerializeField] private float fadeInDelay = 4;
    [SerializeField] private float fadeInSpeed = 0.1f;

    private Image fade;
    private float fadeTimer;

    private void Start()
    {
        fade = GetComponent<Image>();

        if (!enable)
        {
            GameManager.player.start = true;
            Destroy(gameObject);
        }
        else
        {
            GameManager.player.start = false;
            GameManager.audio.Play("Nuclear Warning");
        }
    }

    private void Update()
    {
        // Fade In
        if (fade.color.a <= 0)
        {
            Destroy(gameObject);
        }
        else if (fadeTimer >= fadeInDelay)
        {
            fade.color = new Vector4(fade.color.r, fade.color.g, fade.color.b, 1 - (fadeTimer - fadeInDelay) * fadeInSpeed);
            GameManager.player.start = true;
        }

        // Fade Timer
        fadeTimer += Time.deltaTime;
    }
}
