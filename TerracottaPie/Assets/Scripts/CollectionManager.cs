using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CollectionManager : MonoBehaviour
{

    private static CollectionManager _instance;

    public static CollectionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<CollectionManager>();
            }
            return _instance;
        }
    }
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public int collectionScore;
    public int goodScoreAddition;
    public int badScoreSubtraction;
    public Autoscroller autoscrollerScript;
    [SerializeField] public GameObject countdownPanel;
    float currCountdownValue;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI scoreNumber;
    public static int highestScore;

    public IEnumerator StartCountdown(float countdownValue)
    {
        AudioManager.Instance.StartGame();
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            Debug.Log("Countdown: " + currCountdownValue);
            timerText.text = currCountdownValue.ToString();
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        autoscrollerScript.scrolling = true;
        countdownPanel.SetActive(false);
    }

    private void Update()
    {
        if (collectionScore < 0)
        {
            collectionScore = 0;
        }

        scoreNumber.text = collectionScore.ToString();
        
        if (autoscrollerScript.Progress >= 1 && collectionScore > highestScore)
        {
            highestScore = collectionScore;
        }
    }
}
