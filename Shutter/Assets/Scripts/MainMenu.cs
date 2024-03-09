using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public FadeImages fade = null;
    private bool start = false;
    public float time = 2;
    public float blinkTimeA = 4;
    public float blinkTimeB = 7;
    public float blinkLength = 0.1f;
    public GameObject monster = null;
    private float breatheTime = 0;
    public GameObject hardmode = null;

    void Awake()
    {
        hardmode.active = GameManager.hardmode;

        fade.isFading = FadeImages.FadingState.FadingOut;

        monster.active = true;

        Invoke("Blink", Random.Range(blinkTimeA, blinkTimeB));

        DontDestroyOnLoad(fade);
    }

    private void Start()
    {
        AudioManager.Instance.Play("Title");

        AudioManager.Instance.Play("Static");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && fade.isFading == FadeImages.FadingState.NotFading && !start)
        {
            start = true;

            fade.Fade();

            Invoke("StartGame", time);

            fade.Invoke("Destroy", time * 2);

            AudioManager.Instance.Stop("Title");

            AudioManager.Instance.Play("Start", 0.2f);
        }

        breatheTime += Time.deltaTime;

        monster.transform.position += new Vector3(0, Mathf.Sin(breatheTime * 1) * 0.05f * Time.deltaTime, 0);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void StartGame()
    {
        AudioManager.Instance.Play("Ambience");

        SceneManager.LoadScene("Game");
    }

    private void Blink()
    {
        monster.active = false;
        Invoke("EndBlink", blinkLength);
    }

    private void EndBlink()
    {
        monster.active = true;
        Invoke("Blink", Random.Range(blinkTimeA, blinkTimeB));
    }
}
