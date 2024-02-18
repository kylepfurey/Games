using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Title screen letter behavior 
    [SerializeField] private Rigidbody[] letters;
    [SerializeField] private GameObject platform;
    private float timer = 0;
    [SerializeField] private float freezeLetterTime = 2.2f;
    [SerializeField] private Vector3 platformNewPosition = new Vector3(0, -4, 0);
    private bool frozen = false;
    [SerializeField] private float helpTextTime = 10;
    [SerializeField] private float helpTextFadeSpeed = 10;
    [SerializeField] private TextMeshPro helpText = null;
    [SerializeField] private GameObject bestTimeObject = null;
    private TextMeshPro bestTime = null;
    [SerializeField] private float dropTime = 1.3f;
    private bool dropSound = false;
    [SerializeField] private float crashTime = 2;
    private bool crashSound = false;

    private void Start()
    {
        helpText.color = new Vector4(helpText.color.r, helpText.color.g, helpText.color.b, 0);

        bestTime = bestTimeObject.GetComponentInChildren<TextMeshPro>();

        GameManager.Timer.timerGo = false;

        if (GameManager.Timer.timer == 0)
        {
            Destroy(bestTimeObject);
        }
        else if (GameManager.Timer.timer < GameManager.Timer.bestTime)
        {
            GameManager.Timer.bestTime = GameManager.Timer.timer;

            GameManager.Timer.displayedBestTime = "Best Time: " + Timer.ConvertTimer(GameManager.Timer.bestTime);

            bestTime.text = GameManager.Timer.displayedBestTime;

            Invoke("Cat", 3);
        }
        else
        {
            GameManager.Timer.displayedBestTime = "Best Time: " + Timer.ConvertTimer(GameManager.Timer.bestTime);

            bestTime.text = GameManager.Timer.displayedBestTime;
        }

        GameManager.Timer.timer = 0;

        GameManager.Audio.Stop("Title");

        GameManager.Audio.Play("Drumroll");
    }

    private void Update()
    {
        // Move platform and freeze letters after a set number of seconds
        if (!frozen)
        {
            if (!dropSound && timer >= dropTime)
            {
                AudioSource source = GameManager.Audio.Get("Crash");

                source.volume = 1;

                source.PlayOneShot(source.clip);

                Invoke("Crash", 0.6f);

                dropSound = true;
            }

            if (!crashSound && timer >= crashTime)
            {
                GameManager.Audio.Get("Drumroll").time = 4.25f;

                crashSound = true;

                GameManager.Audio.Play("Title");
            }

            if (timer >= freezeLetterTime)
            {
                frozen = true;

                platform.transform.position = platformNewPosition;

                for (int i = 0; i < letters.Length; i++)
                {
                    letters[i].velocity = Vector3.zero;

                    letters[i].angularVelocity = Vector3.zero;
                }
            }
        }
        else
        {
            if (timer >= helpTextTime)
            {
                helpText.color = new Vector4(helpText.color.r, helpText.color.g, helpText.color.b, helpText.color.a + helpTextFadeSpeed * Time.deltaTime);

                if (helpText.color.a >= 1)
                {
                    Destroy(this);
                }
            }
        }

        timer += Time.deltaTime;
    }

    private void Crash()
    {
        AudioSource source = GameManager.Audio.Get("Crash");

        source.volume = 0.75f;

        source.PlayOneShot(source.clip);
    }

    private void Cat()
    {
        GameManager.Audio.PlayOnce("Cat");
    }
}
