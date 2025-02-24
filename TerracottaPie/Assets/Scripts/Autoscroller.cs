// .cs
// 2D Autoscroller Component
// by Kyle Furey

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Automatically scrolls select UI game objects.
/// </summary>
public sealed class Autoscroller : MonoBehaviour
{
    [Header("Automatically scrolls select UI game objects.\n")]

    [Header("The background game object to scroll:")]
    [SerializeField] private RectTransform background = null;

    [Header("The speed to rotate auto scrolling game objects:")]
    [SerializeField] private float rotateSpeed = 4;

    [Header("Game objects to rotate with the scrolling background:")]
    [SerializeField] private List<GameObject> rotatingObjects = new List<GameObject>();

    [Header("The time it takes to scroll the entire background:")]
    [SerializeField] private float scrollTime = 60;

    [Header("Whether the screen will start scrolling on start:")]
    public bool scrolling = false;

    [Header("The starting X value of the background (X) and the ending X value of the background (Y):")]
    [SerializeField] private Vector2 screenClamps = new Vector2(3840, -9600);

    [Header("Game objects to display the final score:")]
    [SerializeField] private Vector2 scoreSpeed = new Vector2(1, 1);
    [SerializeField] private GameObject scoreText = null;
    [SerializeField] private GameObject scoreTarget = null;
    [SerializeField] private Vector2 cameraSpeed = new Vector2(1, 1);
    [SerializeField] private GameObject cameraTarget = null;
    [SerializeField] private float restartDelay = 5;

#if UNITY_EDITOR
    [Header("DEBUG - The current progress of the autoscroll:")]
    [SerializeField] private float scrollProgress = 0;
#endif

    // PROPERTIES

    /// <summary>
    /// The current progress of the scroll (0 is the beginning, 1 is the end).
    /// </summary>
    public float Progress
    {
        get
        {
            return Mathf.InverseLerp(screenClamps.x, screenClamps.y, background.localPosition.x);
        }

        set
        {
            Vector3 pos = background.localPosition;

            pos.x = Mathf.Lerp(screenClamps.x, screenClamps.y, value);

            background.localPosition = pos;

#if UNITY_EDITOR
            scrollProgress = value;
#endif
        }
    }


    // UNITY EVENTS

    /// <summary>
    /// Start() is called before the first frame update.
    /// </summary>
    private void Start()
    {
        StartCoroutine(CollectionManager.Instance.StartCountdown(3));

        if (background == null)
        {
            Debug.LogError("ERROR: Autoscroller \"" + gameObject.name + "\"'s background variable was not set!");

            Destroy(this);
        }

        transform.parent.parent.position = new Vector3(0, 100, 0);

        transform.parent.parent.eulerAngles = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// Update() is called once per frame.
    /// </summary>
    private void FixedUpdate()
    {
        if (scrolling)
        {
            Progress += Time.fixedDeltaTime * (1 / scrollTime);

            float progress = Progress * 360;

            foreach (var obj in rotatingObjects)
            {
                Vector3 rot = obj.transform.eulerAngles;

                rot.y = progress * rotateSpeed;

                obj.transform.eulerAngles = rot;
            }
        }

        if (Progress >= 1)
        {
            if (scrolling)
            {
                Restart();
            }

            scrolling = false;

            scoreText.transform.position = Vector3.Lerp(scoreText.transform.position, scoreTarget.transform.position, scoreSpeed.x * Time.fixedDeltaTime);

            scoreText.transform.localScale = Vector3.Lerp(scoreText.transform.localScale, scoreTarget.transform.localScale, scoreSpeed.y * Time.fixedDeltaTime);

            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraTarget.transform.position, cameraSpeed.x * Time.fixedDeltaTime);

            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, cameraTarget.transform.rotation, cameraSpeed.y * Time.fixedDeltaTime);
        }
    }


    // METHODS

    /// <summary>
    /// Restarts the game to the main menu.
    /// </summary>
    private async void Restart()
    {
        FlickerScore();

        await Task.Delay((int)(restartDelay * 1000));

        AudioManager.Instance.StartNewGame();

        SceneManager.LoadScene("StartScreen");
    }

    /// <summary>
    /// Flickers the score every 0.5 seconds.
    /// </summary>
    private async void FlickerScore()
    {
        scoreText.gameObject.SetActive(!scoreText.gameObject.activeSelf);

        await Task.Delay(500);

        if (this != null)
        {
            FlickerScore();
        }
    }
}
