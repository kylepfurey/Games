using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private static Player playerReference;
    private static AudioManager audioReference;
    private static TimeManager timeReference;

    public static Player player
    {
        get
        {
            if (playerReference == null)
            {
                playerReference = FindObjectOfType<Player>();
            }

            return playerReference;
        }

        set
        {
            playerReference = value;
        }
    }

    public static AudioManager audio
    {
        get
        {
            if (audioReference == null)
            {
                audioReference = FindObjectOfType<AudioManager>();
            }

            return audioReference;
        }

        set
        {
            audioReference = value;
        }
    }
    public static TimeManager time
    {
        get
        {
            if (timeReference == null)
            {
                timeReference = FindObjectOfType<TimeManager>();
            }

            return timeReference;
        }

        set
        {
            timeReference = value;
        }
    }

    public static float timeScale
    {
        get
        {
            return instance._timeScale;
        }
        set
        {
            foreach (var item in FindObjectsOfType<Rigidbody>())
            {
                if (item.gameObject.tag == "Player")
                    continue;

                item.mass *= (instance._timeScale / value);
                item.velocity *= (value / instance._timeScale);
            }
            instance._timeScale = value;
            foreach (var item in FindObjectsOfType<VisualEffect>())
                try
                {
                    item.SetFloat("TimeScale", value);
                }
                catch
                {
                    Debug.Log(item.gameObject.name + " does not have a TimeScale");
                }

        }
    }
    private float _timeScale = 1.0f;
    public static float deltaTime => Time.deltaTime * timeScale;

    [SerializeField] private float gravity;
    public float Gravity => gravity / 100;
    public GameObject shockwave
    {
        get
        {
            if (_shockwave == null)
                _shockwave = FindObjectOfType<ShockwaveScript>().gameObject;

            return _shockwave;
        }
    }
    private GameObject _shockwave;
    public GameObject heatwave
    {
        get
        {
            if (_heatwave == null)
                _heatwave = FindObjectOfType<HeatwaveScript>().gameObject;

            return _heatwave;
        }
    }
    private GameObject _heatwave; 
    public GameObject deathwave
    {
        get
        {
            if (_deathwave == null)
                _deathwave = FindObjectOfType<DeathwaveScript>().gameObject;

            return _deathwave;
        }
    }
    private GameObject _deathwave;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

}
