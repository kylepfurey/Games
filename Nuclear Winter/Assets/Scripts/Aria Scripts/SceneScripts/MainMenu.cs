using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    [SerializeField] private PlayerInput Input;

    [SerializeField] private TextMeshProUGUI startText;
    [SerializeField] private TextMeshProUGUI howToPlayText;
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField] private TextMeshProUGUI closeText;
    [SerializeField] private TextMeshProUGUI closeText2;

    [SerializeField] private GameObject instructionsTxt;
    [SerializeField] private GameObject creditsTxt;

    [SerializeField] private Image fade;
    [SerializeField] private Image fadeInLogo;
    [SerializeField] private float fadeInDelay;
    [SerializeField] private float fadeInSpeed;
    [SerializeField] private float fadeOutDelay;
    [SerializeField] private float fadeOutSpeed;

    private bool isController;
    private Color selected;
    private Color unSelected;
    private int selection;
    private bool mainMenu = true;
    private bool moveUp;
    private bool selectUp;

    private bool start;
    private bool music;
    private bool fadeOut;
    private float fadeTimer;

    private void Start()
    {
        mainMenu = true;
        isController = false;

        selected = startText.color;
        unSelected = creditsText.color;
        startText.color = unSelected;

        fade.gameObject.SetActive(true);

        GameManager.audio.PlayOnce("Opening");
    }

    private void Update()
    {
        // Fade In
        if (fade.color.a <= 0 && !start)
        {
            Destroy(fadeInLogo.gameObject);

            fade.gameObject.SetActive(false);

            start = true;
        }
        else if (fadeTimer >= fadeInDelay && !start)
        {
            fade.color = new Vector4(fade.color.r, fade.color.g, fade.color.b, 1 - (fadeTimer - fadeInDelay) * fadeInSpeed);
            fadeInLogo.color = new Vector4(fadeInLogo.color.r, fadeInLogo.color.g, fadeInLogo.color.b, fade.color.a);

            if (!music)
            {
                music = true;

                GameManager.audio.Play("Main Menu");
            }
        }

        // Fade Out
        if (fadeTimer >= fadeOutDelay && fadeOut)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else if (fadeTimer < fadeOutDelay && fadeOut)
        {
            fade.color = new Vector4(fade.color.r, fade.color.g, fade.color.b, fadeTimer * fadeOutSpeed);
        }


        if (start && !fadeOut)
        {
            // Controller Check
            if (Input.actions.FindAction("Look Mouse").ReadValue<Vector2>().magnitude > 0  && isController)
            {
                isController = false;

                UpdateSelection();
            }


            // Input
            float move = Input.actions.FindAction("Menu Move").ReadValue<Vector2>().x;
            bool select = Player.Button(Input.actions.FindAction("Menu Select").ReadValue<float>());

            if (Mathf.Abs(move) < 0.25f)
            {
                moveUp = true;
            }

            if (!select)
            {
                selectUp = true;
            }


            // Moving Selection
            if (move > 0 && moveUp && isController)
            {
                moveUp = false;

                selection++;

                if (selection > 2)
                {
                    selection = 0;
                }

                UpdateSelection();
            }
            else if (move > 0 && moveUp)
            {
                moveUp = false;

                isController = true;

                UpdateSelection();
            }

            if (move < 0 && moveUp && isController)
            {
                moveUp = false;

                selection--;

                if (selection < 0)
                {
                    selection = 2;
                }

                UpdateSelection();
            }
            else if (move < 0 && moveUp)
            {
                moveUp = false;

                isController = true;

                UpdateSelection();
            }


            // Selection
            if (select && selectUp && mainMenu && isController)
            {
                selectUp = false;

                switch (selection)
                {
                    case 0:

                        StartGame();

                        break;

                    case 1:

                        HowToPlay();

                        break;

                    case 2:

                        Credits();

                        break;
                }

                UpdateSelection();
            }
            else if (select && selectUp && !mainMenu && isController)
            {
                selectUp = false;

                Close();

                UpdateSelection();
            }
            else if (select && selectUp)
            {
                selectUp = false;

                isController = true;

                UpdateSelection();
            }


            // Quit
            if (Input.actions.FindAction("Exit").ReadValue<float>() > 0)
            {
                Application.Quit();
            }
        }
        else
        {
            // Fade Timer
            fadeTimer += Time.deltaTime;
        }
    }

    private void UpdateSelection()
    {
        if (isController)
        {
            if (mainMenu)
            {
                switch (selection)
                {
                    case 0:

                        startText.color = selected;
                        howToPlayText.color = unSelected;
                        creditsText.color = unSelected;
                        closeText.color = unSelected;
                        closeText2.color = unSelected;

                        break;

                    case 1:

                        startText.color = unSelected;
                        howToPlayText.color = selected;
                        creditsText.color = unSelected;
                        closeText.color = unSelected;
                        closeText2.color = unSelected;

                        break;

                    case 2:

                        startText.color = unSelected;
                        howToPlayText.color = unSelected;
                        creditsText.color = selected;
                        closeText.color = unSelected;
                        closeText2.color = unSelected;

                        break;
                }
            }
            else
            {
                startText.color = unSelected;
                howToPlayText.color = unSelected;
                creditsText.color = unSelected;
                closeText.color = selected;
                closeText2.color = selected;
            }
        }
        else
        {
            startText.color = unSelected;
            howToPlayText.color = unSelected;
            creditsText.color = unSelected;
            closeText.color = unSelected;
            closeText2.color = unSelected;
        }
    }

    public void StartGame()
    {
        if (mainMenu)
        {
            fadeOut = true;

            fade.gameObject.SetActive(true);

            fadeTimer = 0;

            GameManager.audio.Stop("Main Menu");
            GameManager.audio.PlayOnce("Start Up");
        }
    }
    public void HowToPlay()
    {
        if (mainMenu)
        {
            instructionsTxt.SetActive(true);

            mainMenu = false;
        }
    }
    public void Credits()
    {
        if (mainMenu)
        {
            creditsTxt.SetActive(true);

            mainMenu = false;
        }
    }
    public void Close()
    {
        if (!mainMenu)
        {
            creditsTxt.SetActive(false);
            instructionsTxt.SetActive(false);

            mainMenu = true;
        }
    }
}
