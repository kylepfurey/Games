using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

// The teleporters that navigate the player through the game.
public class Teleporter : MonoBehaviour
{
    [Header("The teleporters that navigate the player through the game.")]

    [Header("TELEPORTER SETTINGS")]

    [Header("The time it takes to spawn this teleporter:")]
    [SerializeField] private float timeToSpawn = 3;

    [Header("The total time it takes to complete a teleport:")]
    [SerializeField] private float timeToTeleport = 2;

    [Header("The tag to check for teleporting:")]
    [SerializeField] private string tagToTeleport = "Fabio";

    [Header("The name of the scene this teleporter loads:")]
    [SerializeField] private string sceneToLoad = "Scene Name";

    [Header("Bounds of the teleporter's position (x is minimum, y is maximum):")]
    [SerializeField] private bool useBoundsX = false;
    [SerializeField] private Vector2 boundsX = new Vector2(-10, 10);
    [SerializeField] private bool useBoundsY = false;
    [SerializeField] private Vector2 boundsY = new Vector2(-10, 10);
    [SerializeField] private bool useBoundsZ = false;
    [SerializeField] private Vector2 boundsZ = new Vector2(-10, 10);

    // The current timer value
    private float timer = 0;

    // Whether the teleporter has spawned
    private bool spawned = false;

    // Whether the player is currently touching the teleporter
    private bool touching = false;

    [Header("TELEPORTER UI")]

    [Header("The time it takes for this teleporter to fade its graphics:")]
    [SerializeField] private float fadeSpeed = 10;

    [Header("The 2D graphic that represents the teleporter:")]
    [SerializeField] private Image image = null;

    [Header("The slider that represents the teleporting progress:")]
    [SerializeField] private Slider slider = null;

    [Header("The images associated with the slider:")]
    [SerializeField] private List<Image> sliderImages = null;

    [Header("The value to apply over time as a continuous rotation:")]
    [SerializeField] private Vector3 rotation = new Vector3(0, 0, -100);

    private void Start()
    {
        image.color = new Vector4(image.color.r, image.color.g, image.color.b, 0);

        for (int i = 0; i < sliderImages.Count; i++)
        {
            sliderImages[i].color = new Vector4(sliderImages[i].color.r, sliderImages[i].color.g, sliderImages[i].color.b, 0);
        }
    }

    private void Update()
    {
        transform.Rotate(rotation * Time.deltaTime);

        if (!spawned)
        {
            // Increment timer
            timer += Time.deltaTime;

            // Spawn the teleporter
            if (timer >= timeToSpawn)
            {
                spawned = true;

                timer = 0;
            }
        }
        else
        {
            // Fade the teleporter in
            image.color = new Vector4(image.color.r, image.color.g, image.color.b, Mathf.Min(image.color.a + fadeSpeed * Time.deltaTime, 1));

            if (touching)
            {
                for (int i = 0; i < sliderImages.Count; i++)
                {
                    sliderImages[i].color = new Vector4(sliderImages[i].color.r, sliderImages[i].color.g, sliderImages[i].color.b, Mathf.Min(sliderImages[i].color.a + fadeSpeed * Time.deltaTime, 1));
                }

                timer += Time.deltaTime;

                slider.value = timer / timeToTeleport;

                if (timer >= timeToTeleport)
                {
                    GameManager.Timer.timerGo = true;

                    //GameManager.Audio.Stop("Title");

                    GameManager.Audio.Stop("Sleighbells");

                    GameManager.Audio.Stop("Shake");

                    GameManager.Audio.Stop("Sleighbells");

                    GameManager.Audio.Stop("Bubbles");

                    GameManager.Audio.Stop("Teleport");

                    SceneManager.LoadScene(sceneToLoad);
                }
            }
            else
            {
                for (int i = 0; i < sliderImages.Count; i++)
                {
                    sliderImages[i].color = new Vector4(sliderImages[i].color.r, sliderImages[i].color.g, sliderImages[i].color.b, Mathf.Max(sliderImages[i].color.a - fadeSpeed * Time.deltaTime, 0));
                }

                timer -= Time.deltaTime;

                timer = Mathf.Max(timer, 0);

                slider.value = timer / timeToTeleport;
            }
        }

        if (useBoundsX)
        {
            if (transform.parent.transform.position.x > boundsX.y)
            {
                transform.parent.transform.position = new Vector3(boundsX.y, transform.parent.transform.position.y, transform.parent.transform.position.z);
            }
            else if (transform.parent.transform.position.x < boundsX.x)
            {
                transform.parent.transform.position = new Vector3(boundsX.x, transform.parent.transform.position.y, transform.parent.transform.position.z);
            }
        }

        if (useBoundsY)
        {
            if (transform.parent.transform.position.y > boundsY.y)
            {
                transform.parent.transform.position = new Vector3(transform.parent.transform.position.x, boundsY.y, transform.parent.transform.position.z);
            }
            else if (transform.parent.transform.position.y < boundsY.x)
            {
                transform.parent.transform.position = new Vector3(transform.parent.transform.position.x, boundsX.x, transform.parent.transform.position.z);
            }
        }

        if (useBoundsZ)
        {
            if (transform.parent.transform.position.z > boundsZ.y)
            {
                transform.parent.transform.position = new Vector3(transform.parent.transform.position.x, transform.parent.transform.position.y, boundsZ.y);
            }
            else if (transform.parent.transform.position.z < boundsZ.x)
            {
                transform.parent.transform.position = new Vector3(transform.parent.transform.position.x, transform.parent.transform.position.y, boundsZ.y);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Start loading next scene
        if (spawned && other.tag == tagToTeleport)
        {
            touching = true;

            GameManager.Audio.Play("Teleport", slider.value / 1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Stop loading
        if (spawned && other.tag == tagToTeleport)
        {
            touching = false;

            GameManager.Audio.Stop("Teleport");
        }
    }
}
