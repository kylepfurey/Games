using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    // Main menu objects
    public PlayerInput input;
    public AudioManager Music;
    public GameObject camera;
    public GameObject title;
    public GameObject jump;
    public GameObject credits;
    public GameObject target;
    public RawImage fadeIn;
    public RawImage ice;
    public TextMeshProUGUI go;
    public TextMeshProUGUI goShadow;
    private float cameraHeight;
    private Vector3 titleScale;
    private float timer;
    private float timer2;
    private float timer3;
    private bool start = false;

    private void Awake()
    {
        cameraHeight = camera.transform.position.y;
        titleScale = title.transform.localScale;
    }

    private void Update()
    {
        // Fading in the game
        fadeIn.color = new Vector4(fadeIn.color.r, fadeIn.color.g, fadeIn.color.b, fadeIn.color.a - Time.deltaTime);

        // Camera levitating
        if (!start)
        {
            camera.transform.position = new Vector3(camera.transform.position.x, cameraHeight + Mathf.Sin(timer * 2) / 4, camera.transform.position.z);
        }

        // Title size
        title.transform.localScale = new Vector3(titleScale.x + Mathf.Sin(timer) / 8, titleScale.y + Mathf.Sin(timer) / 8, titleScale.z + Mathf.Sin(timer) / 8);

        // Randomizing seed
        if (timer >= timer2)
        {
            GameManager.Modules.SpawnAll();
            timer2 = timer + 0.5f;

            if (!start)
            {
                jump.active = !jump.active;
            }
        }

        // Timer
        timer += Time.deltaTime;

        // Start game
        if (!start && input.actions.FindAction("Jump").ReadValue<float>() > 0)
        {
            start = true;
            timer3 = timer + 5;
            jump.active = false;
            credits.active = false;
            Music.Play("Start");
        }

        if (start && timer > timer3)
        {
            Music.Stop("Cold");
            Music.Play("Climb");
            SceneManager.LoadScene("Game");
        }
        else if (start)
        {
            // Camera transition
            camera.transform.position = Vector3.Lerp(camera.transform.position, target.transform.position, Time.deltaTime / 1.5f);
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, target.transform.rotation, Time.deltaTime / 1.5f);

            ice.color = new Vector4(ice.color.r, ice.color.g, ice.color.b, ice.color.a - Time.deltaTime / 12.5f);

            if (-(timer - timer3) < 1)
            {
                go.text = "1";
                go.color = Color.red;
            }
            else if (-(timer - timer3) < 2)
            {
                go.text = "2";
                go.color = Color.yellow;
            }
            else if (-(timer - timer3) < 3)
            {
                go.text = "3";
                go.color = Color.green;
            }
            else
            {
                go.text = "Get Ready!";
            }

            goShadow.text = go.text;
        }


        // Exiting game
        if (input.actions.FindAction("Exit").ReadValue<float>() > 0)
        {
            Application.Quit();
        }
    }
}
