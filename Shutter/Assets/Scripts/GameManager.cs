
// Game Manager Template Script
// by Kyle Furey

using UnityEngine;
using UnityEngine.SceneManagement;

// A component that allows easy access to other components through the singleton design pattern.
public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance;

    [HideInInspector]
    public static PlayerMovement Player
    {
        get
        {
            if (!player)
            {
                player = FindObjectOfType<PlayerMovement>();
            }

            return player;
        }
        set
        {
            Player = value;
        }
    }

    private static PlayerMovement player;

    public static int lightSwitches = 8;

    public static SwitchedLight emergencyLight;

    public static GameObject winBox;

    public FadeImages fade;

    public float time = 2;

    public Jumpscare jumpscare;

    public GameObject monsterPrefab;

    public GameObject spawnPoint1;

    public GameObject spawnPoint2;

    public GameObject spawnPoint3;

    public static bool hardmode = false;

    private void Awake()
    {
        Instance = this;
        lightSwitches = FindObjectsOfType<LightSwitch>().Length;

        if (GameObject.Find("Emergency Light") != null)
        {
            emergencyLight = GameObject.Find("Emergency Light").GetComponent<SwitchedLight>();
            emergencyLight.TurnOff();
        }

        winBox = GameObject.Find("Win Box");

        fade.transform.parent = null;

        DontDestroyOnLoad(fade);

        if (hardmode)
        {
            GameObject monster = Instantiate(monsterPrefab);

            monster.GetComponent<MonsterAI>().number = 1;

            monster.transform.position = spawnPoint1.transform.position;
        }
    }

    public void check()
    {
        if (hardmode)
        {
            if (lightSwitches == 12)
            {
                GameObject monster = Instantiate(monsterPrefab);

                monster.GetComponent<MonsterAI>().number = 2;

                monster.transform.position = spawnPoint2.transform.position;
            }
            else if (lightSwitches == 0)
            {
                GameObject monster = Instantiate(monsterPrefab);

                monster.GetComponent<MonsterAI>().number = 3;

                monster.transform.position = spawnPoint3.transform.position;

                end();
            }
        }
        else
        {
            if (lightSwitches == 12)
            {
                GameObject monster = Instantiate(monsterPrefab);

                monster.GetComponent<MonsterAI>().number = 1;

                monster.transform.position = spawnPoint1.transform.position;
            }
            else if (lightSwitches == 0)
            {
                GameObject monster = Instantiate(monsterPrefab);

                monster.GetComponent<MonsterAI>().number = 2;

                monster.transform.position = spawnPoint3.transform.position;

                end();
            }
        }
    }

    public static void end()
    {
        AudioManager.Instance.Play("Escape");

        emergencyLight.TurnOn();

        winBox.SetActive(true);
    }

    public void death()
    {
        jumpscare.gameObject.active = true;

        Player.active = false;

        fade.Fade();

        Invoke("death2", time);

        AudioManager.Instance.Stop("Ambience");

        AudioManager.Instance.Stop("Shutter");

        AudioManager.Instance.Play("Jumpscare", 0.15f);

        AudioManager.Instance.Play("Death");
    }

    private void death2()
    {
        fade.isFading = FadeImages.FadingState.NotFading;

        Invoke("StartGame", 1.5f);
    }

    private void StartGame()
    {
        fade.isFading = FadeImages.FadingState.FadingOut;

        fade.Invoke("Destroy", time);

        AudioManager.Instance.Play("Ambience");

        AudioManager.Instance.Stop("Jumpscare");

        AudioManager.Instance.Stop("Death");

        SceneManager.LoadScene("Game");
    }
}
