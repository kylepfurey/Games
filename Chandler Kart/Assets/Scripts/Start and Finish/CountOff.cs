using UnityEngine;

public class CountOff : MonoBehaviour
{
    public GameObject This;

    // Kart Data
    public Kart1 Kart1;
    public Kart2 Kart2;
    public Kart3 Kart3;
    public Kart4 Kart4;

    // Timer
    public float timer;
    public float flashTimer;
    public bool flash = false;

    // 3 2 1 Go Assets
    public Vector3 originalPosition;
    public GameObject Three;
    public GameObject Two;
    public GameObject One;
    public GameObject Go;

    public Renderer[] GoPieces;     // 13 Total
    public Material Blue;
    public Material White;

    // Sound Bool
    public bool playSounds;
    public float musicStart = 28.5f;

    void Start()
    {
        timer = 180;                // 6 second timer
        flashTimer = 10;

        originalPosition = transform.position;

        GameObject Object0 = GameObject.FindWithTag("Kart1");
        if (Object0 != null)
        {
            Kart1 = Object0.GetComponent<Kart1>();
        }

        GameObject Object1 = GameObject.FindWithTag("Kart2");
        if (Object0 != null)
        {
            Kart2 = Object1.GetComponent<Kart2>();
        }

        GameObject Object2 = GameObject.FindWithTag("Kart3");
        if (Object0 != null)
        {
            Kart3 = Object2.GetComponent<Kart3>();
        }

        GameObject Object3 = GameObject.FindWithTag("Kart4");
        if (Object0 != null)
        {
            Kart4 = Object3.GetComponent<Kart4>();
        }
    }

    void FixedUpdate()
    {
        timer -= 0.5f;
        flashTimer += 1;

        if (timer > 120)
        {
            Three.transform.position = new Vector3(0, -100, 0);
            Two.transform.position = new Vector3(0, -100, 0);
            One.transform.position = new Vector3(0, -100, 0);
            Go.transform.position = new Vector3(0, -100, 0);
        }
        else if (timer > 90)
        {
            Three.transform.position = new Vector3(originalPosition.x, Mathf.Sin(Mathf.Abs(timer) / 10) / 5 + originalPosition.y, originalPosition.z);
            Two.transform.position = new Vector3(0, -100, 0);
            One.transform.position = new Vector3(0, -100, 0);
            Go.transform.position = new Vector3(0, -100, 0);
        }
        else if (timer > 60)
        {
            Three.transform.position = new Vector3(0, -100, 0);
            Two.transform.position = new Vector3(originalPosition.x, Mathf.Sin(Mathf.Abs(timer) / 10) / 5 + originalPosition.y, originalPosition.z);
            One.transform.position = new Vector3(0, -100, 0);
            Go.transform.position = new Vector3(0, -100, 0);
        }
        else if (timer > 30)
        {
            Three.transform.position = new Vector3(0, -100, 0);
            Two.transform.position = new Vector3(0, -100, 0);
            One.transform.position = new Vector3(originalPosition.x, Mathf.Sin(Mathf.Abs(timer) / 10) / 5 + originalPosition.y, originalPosition.z);
            Go.transform.position = new Vector3(0, -100, 0);
        }
        else if (timer > -30)
        {
            Three.transform.position = new Vector3(0, -100, 0);
            Two.transform.position = new Vector3(0, -100, 0);
            One.transform.position = new Vector3(0, -100, 0);
            Go.transform.position = new Vector3(originalPosition.x, Mathf.Sin(Mathf.Abs(timer) / 10) / 5 + originalPosition.y, originalPosition.z);

            Kart1.go = true;
            Kart2.go = true;
            Kart3.go = true;
            Kart4.go = true;

            if (playSounds && timer == musicStart)
            {
                switch (Kart1.KartSettings.map)
                {
                    case 1:
                        if (Kart1.KartSettings.night)
                        {
                            Kart1.Audio.PlayMusic("Chandler Music Start Night", 0, "Chandler Music Night", 0);
                        }
                        else
                        {
                            Kart1.Audio.PlayMusic("Chandler Music Start Day", 0, "Chandler Music Day", 0);
                        }
                        break;

                    case 2:
                        if (Kart1.KartSettings.night)
                        {
                            Kart1.Audio.PlayMusic("Belknap Music Start Night", 0, "Belknap Music Night", 0);
                        }
                        else
                        {
                            Kart1.Audio.PlayMusic("Belknap Music Start Day", 0, "Belknap Music Day", 0);
                        }
                        break;

                    case 3:
                        if (Kart1.KartSettings.night)
                        {
                            Kart1.Audio.PlayMusic("Furey Music Start Night", 0, "Furey Music Night", 0);
                        }
                        else
                        {
                            Kart1.Audio.PlayMusic("Furey Music Start Day", 0, "Furey Music Day", 0);
                        }
                        break;
                }

                playSounds = false;
            }
        }
        else if (timer == -30)
        {
            Destroy(This);
        }


        if (flashTimer > 5)
        {
            flashTimer = 0;

            if (flash)
            {
                flash = false;
            }
            else
            {
                flash = true;
            }
        }

        if (flash)
        {
            for (int i = 0; i < GoPieces.Length; i++)
            {
                GoPieces[i].material = White;
            }
        }
        else
        {
            for (int i = 0; i < GoPieces.Length; i++)
            {
                GoPieces[i].material = Blue;
            }
        }
    }
}