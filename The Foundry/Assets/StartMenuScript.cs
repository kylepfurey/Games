using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuScript : MonoBehaviour
{
    public PlayerInput Input;

    public bool isMouse;
    public int selection;

    public float MOVE;
    public float MOVE_DELAY_START;
    public float MOVE_DELAY_RESET;
    public float MOVE_DELAY_TIME;
    public bool SELECT;
    public bool SELECT_UP;
    public bool BACK;
    public bool BACK_UP;
    public bool PAUSE;
    public bool PAUSE_UP;

    public Button StartButton;
    public Button LegacyStartButton;
    public Button QuitButton;
    public Button ResumeButton;
    public Button MainMenuButton;

    public TextMeshProUGUI StartText;
    public TextMeshProUGUI LegacyStartText;
    public TextMeshProUGUI QuitText;
    public TextMeshProUGUI ResumeText;
    public TextMeshProUGUI MainMenuText;
    public GameObject Loading;
    public GameObject Credits;

    public Vector4 White = new Vector4(255, 255, 255, 255);
    public Vector4 Orange = new Vector4(253, 147, 121, 255);

    public bool Paused;

    public GameObject SelectSound;
    public GameObject StartSound;

    private void Start()
    {
        Credits.SetActive(!Paused);

        StartButton.enabled = !Paused;
        LegacyStartButton.enabled = !Paused;
        QuitButton.enabled = !Paused;
        ResumeButton.enabled = Paused;
        MainMenuButton.enabled = Paused;

        StartText.enabled = !Paused;
        LegacyStartText.enabled = !Paused;
        QuitText.enabled = !Paused;
        ResumeText.enabled = Paused;
        MainMenuText.enabled = Paused;

        MOVE_DELAY_TIME = MOVE_DELAY_START;
    }

    void Update()
    {
        // Controller Input
        if (isMouse == false)
        {
            MOVE = Input.actions.FindAction("Menu Move").ReadValue<Vector2>().y;

            if (MOVE_DELAY_TIME > 0)
            {
                MOVE_DELAY_TIME -= Time.unscaledDeltaTime;

                MOVE = 0;
            }
            else
            {
                MOVE_DELAY_TIME = 0;
            }

            if (Mathf.Abs(MOVE) < 0.25f && MOVE_DELAY_TIME > MOVE_DELAY_RESET && MOVE_DELAY_RESET > 0)
            {
                MOVE_DELAY_TIME = MOVE_DELAY_RESET;
            }

            SELECT = Button(Input.actions.FindAction("Menu Select").ReadValue<float>());

            if (SELECT == false)
            {
                SELECT_UP = true;
            }

            BACK = Button(Input.actions.FindAction("Menu Back").ReadValue<float>());

            if (BACK == false)
            {
                BACK_UP = true;
            }

            PAUSE = Button(Input.actions.FindAction("Pause").ReadValue<float>());

            if (PAUSE == false)
            {
                PAUSE_UP = true;
            }
        }

        // Freeze Time on Pause
        if (Paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        // Pause Menu Logic
        StartButton.enabled = !Paused;
        LegacyStartButton.enabled = !Paused;
        QuitButton.enabled = !Paused;
        ResumeButton.enabled = Paused;
        MainMenuButton.enabled = Paused;

        StartText.enabled = !Paused;
        LegacyStartText.enabled = !Paused;
        QuitText.enabled = !Paused;
        ResumeText.enabled = Paused;
        MainMenuText.enabled = Paused;

        // Button Color Logic
        if (isMouse == false)
        {
            StartButton.enabled = false;
            LegacyStartButton.enabled = false;
            QuitButton.enabled = false;
            ResumeButton.enabled = false;
            MainMenuButton.enabled = false;

            if (selection == 0)
            {
                StartText.color = Orange / 255;
                ResumeText.color = Orange / 255;
                LegacyStartText.color = White / 255;
                QuitText.color = White / 255;
                MainMenuText.color = White / 255;
            }

            if (selection == 1)
            {
                StartText.color = White / 255;
                ResumeText.color = White / 255;
                LegacyStartText.color = Orange / 255;
                QuitText.color = White / 255;
                MainMenuText.color = Orange / 255;
            }

            if (selection == 2)
            {
                StartText.color = White / 255;
                ResumeText.color = White / 255;
                LegacyStartText.color = White / 255;
                QuitText.color = Orange / 255;
                MainMenuText.color = White / 255;
            }
        }
        else
        {
            StartButton.enabled = true;
            LegacyStartButton.enabled = true;
            QuitButton.enabled = true;
            ResumeButton.enabled = true;
            MainMenuButton.enabled = true;

            StartText.color = White / 255;
            ResumeText.color = White / 255;
            LegacyStartText.color = White / 255;
            QuitText.color = White / 255;
            MainMenuText.color = White / 255;
        }

        // Mouse or Controller Check
        if (Mathf.Abs(UnityEngine.Input.GetAxisRaw("Mouse X")) + Mathf.Abs(UnityEngine.Input.GetAxisRaw("Mouse Y")) > 0 || UnityEngine.Input.GetMouseButtonDown(0))
        {
            if (isMouse == false)
            {
                MOVE_DELAY_TIME = 0;
            }

            isMouse = true;
        }
        else if (Mathf.Abs(Input.actions.FindAction("Menu Move").ReadValue<Vector2>().magnitude) > 0 || Button(Input.actions.FindAction("Menu Select").ReadValue<float>()) || Button(Input.actions.FindAction("Menu Back").ReadValue<float>()))
        {
            if (isMouse)
            {
                MOVE_DELAY_TIME = MOVE_DELAY_START;
            }

            isMouse = false;
        }

        // Move Selection Logic
        if (MOVE > 0 && MOVE_DELAY_TIME <= 0)
        {
            Instantiate(SelectSound, transform.position, transform.rotation, null);
            selection--;
            MOVE_DELAY_TIME = MOVE_DELAY_START;
        }
        else if (MOVE < 0 && MOVE_DELAY_TIME <= 0)
        {
            Instantiate(SelectSound, transform.position, transform.rotation, null);
            selection++;
            MOVE_DELAY_TIME = MOVE_DELAY_START;
        }

        if (selection < 0)
        {
            if (Paused == false)
            {
                selection = 2;
            }
            else
            {
                selection = 1;
            }
        }
        else if (selection > 2 && Paused == false)
        {
            selection = 0;
        }
        else if (selection > 1 && Paused)
        {
            selection = 0;
        }

        // Selecting Logic
        if (SELECT && SELECT_UP && isMouse == false)
        {
            if (selection == 0 && Paused == false)
            {
                StartGame();
            }
            else if (selection == 0)
            {
                Resume();
            }

            if (selection == 1 && Paused == false)
            {
                StartLegacyGame();
            }
            else if (selection == 1)
            {
                MainMenu();
            }

            if (selection == 2 && Paused == false)
            {
                Quit();
            }
        }

        if (((PAUSE && PAUSE_UP) || (BACK && BACK_UP)) && Paused)
        {
            Resume();
        }

        // Button Reset
        if (SELECT)
        {
            SELECT_UP = false;
        }

        if (BACK)
        {
            BACK_UP = false;
        }

        if (PAUSE)
        {
            PAUSE_UP = false;
        }
    }

    public void StartGame()
    {
        Instantiate(StartSound, transform.position, transform.rotation, null);
        Loading.SetActive(true);
        SceneManager.LoadScene("New Map Level 1");
    }

    public void StartLegacyGame()
    {
        Instantiate(StartSound, transform.position, transform.rotation, null);
        Loading.SetActive(true);
        SceneManager.LoadScene("TheMainGameScene");
    }

    public void Quit()
    {
        Instantiate(StartSound, transform.position, transform.rotation, null);
        Application.Quit();
    }

    public void Resume()
    {
        Instantiate(StartSound, transform.position, transform.rotation, null);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Destroy(gameObject);
    }

    public void MainMenu()
    {
        Instantiate(StartSound, transform.position, transform.rotation, null);
        Time.timeScale = 1;
        Loading.SetActive(true);
        SceneManager.LoadScene("StartMenu");
    }

    public void OnPointerEnter()
    {
        Instantiate(SelectSound, transform.position, transform.rotation, null);
    }

    public bool Button(float input)
    {
        if (Mathf.Abs(input) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}