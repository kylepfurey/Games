using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    public GameObject creditsPanel;
    public GameObject startFireButton;
    public GameObject creditsFireButton;
    public GameObject backFireButton;
    public GameObject quitFireButton;
    public GameObject howFireButton;
    public GameObject back2FireButton;
    public TextMeshProUGUI highScoreValue;
    private void Start()
    {
        creditsPanel.SetActive(false);
        highScoreValue.text = CollectionManager.highestScore.ToString();
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    [SerializeField] private GameObject titleText = null;
    [SerializeField] private Vector2 rotateSpeeds = new Vector2(1, 1);
    private float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;
        Vector3 rot = titleText.transform.eulerAngles;
        rot.z = Mathf.Sin(timer * rotateSpeeds.x) * rotateSpeeds.y;
        titleText.transform.eulerAngles = rot;
    }


    public void switchStartImage()
    {
        if(startFireButton.gameObject.activeSelf)
        {
            startFireButton.SetActive(false);
        }
        else
        {
            startFireButton.SetActive(true);
        }
    }

    public void switchCreditsImage()
    {
        if (creditsFireButton.gameObject.activeSelf)
        {
            creditsFireButton.SetActive(false);
        }
        else
        {
            creditsFireButton.SetActive(true);
        }
    }

    public void switchBackImage()
    {
        if (backFireButton.gameObject.activeSelf)
        {
            backFireButton.SetActive(false);
        }
        else
        {
            backFireButton.SetActive(true);
        }
    }

    public void switchQuitImage()
    {
        if (quitFireButton.gameObject.activeSelf)
        {
            quitFireButton.SetActive(false);
        }
        else
        {
            quitFireButton.SetActive(true);
        }
    }

    public void switchHowImage()
    {
        if (howFireButton.gameObject.activeSelf)
        {
            howFireButton.SetActive(false);
        }
        else
        {
            howFireButton.SetActive(true);
        }
    }

    public void switchBack2Image()
    {
        if (back2FireButton.gameObject.activeSelf)
        {
            back2FireButton.SetActive(false);
        }
        else
        {
            back2FireButton.SetActive(true);
        }
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    
}
