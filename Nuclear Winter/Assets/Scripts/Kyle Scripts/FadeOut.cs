using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    [SerializeField] private float fadeOutDelay = 4;
    [SerializeField] private float fadeOutSpeed = 0.1f;
    [SerializeField] private bool playerWin = false;
    [SerializeField] private float winTimer = 4;

    private Image fade;
    private float fadeTimer;

    private void Start()
    {
        fade = GetComponent<Image>();
    }

    private void Update()
    {
        // Fade In
        if (fadeTimer >= fadeOutDelay)
        {
            if (playerWin && fadeTimer >= winTimer + fadeOutDelay)
            {
                Destroy(GameManager.instance.gameObject);
                SceneManager.LoadScene("MainMenu");
            }
            else if (!playerWin)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else if (fadeTimer < fadeOutDelay)
        {
            fade.color = new Vector4(fade.color.r, fade.color.g, fade.color.b, fadeTimer * fadeOutSpeed);
        }

        // Fade Timer
        if (GameManager.player.lose || playerWin)
        {
            fadeTimer += Time.deltaTime;
        }
    }
}
