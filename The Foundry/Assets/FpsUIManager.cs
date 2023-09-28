using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FpsUIManager : MonoBehaviour
{
    public PlayerInput Input;
    public bool SELECT;
    public bool SELECT_UP;

    public static FpsUIManager instance;
    public GameObject hudUI;
    public GameObject deadUI;
    public GameObject victoryUI;

    public GameObject Loading;

    public UIState currentState = UIState.playing;
    public enum UIState
    {
        playing,
        dead,
        victory
    }

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        hudUI.SetActive(currentState == UIState.playing);
        deadUI.SetActive(currentState == UIState.dead);
        victoryUI.SetActive(currentState == UIState.victory);


        // Controller Logic
        if (SELECT)
        {
            SELECT_UP = false;
        }
        else
        {
            SELECT_UP = true;
        }

        SELECT = Button(Input.actions.FindAction("Menu Select").ReadValue<float>());

        if (currentState == UIState.dead || currentState == UIState.victory)
        {
            if (SELECT && SELECT_UP)
            {
                Retry();
            }
        }
    }

    public void Retry()
    {
        Loading.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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