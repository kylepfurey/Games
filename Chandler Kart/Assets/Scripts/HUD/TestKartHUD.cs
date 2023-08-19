using TMPro;
using UnityEngine;

public class TestKartHUD : MonoBehaviour
{
    // Kart Reference
    public TestKart Kart;

    // Assets
    public TextMeshPro Speed;
    public TextMeshPro SpeedShadow;

    public TextMeshPro Laps;
    public TextMeshPro LapsShadow;

    public TextMeshPro Timer;
    public TextMeshPro TimerShadow;

    public TextMeshPro Placement;
    public TextMeshPro PlacementShadow;

    public GameObject CurrentItem;
    public GameObject Boost;
    public GameObject Banana;
    public GameObject Shell;
    public GameObject Bouncer;

    public GameObject ShellAim;
    public Renderer ShellAimRenderer;

    public GameObject Left;
    public GameObject Right;
    public GameObject LeftBorder;
    public GameObject RightBorder;

    public Renderer Finish;
    public Renderer FinishShadow;

    void Start()
    {
        GameObject Object0 = GameObject.FindWithTag("Kart0");
        Kart = Object0.GetComponent<TestKart>();
    }

    void FixedUpdate()
    {
        // Text and Shadows
        if ((int)Kart.speedPercent >= 95 && (int)Kart.speedPercent <= 112 && Kart.drift == false)
        {
            Speed.text = "100 %";
            SpeedShadow.text = Speed.text;
        }
        else if (Kart.drift == false)
        {
            Speed.text = (int)Kart.speedPercent + " %";
            SpeedShadow.text = Speed.text;
        }
        else
        {
            Speed.text = "75 %";
            SpeedShadow.text = Speed.text;
        }

        if (Kart.speedPercent > 112 && Kart.drift == false)
        {
            Speed.color = new Vector4(0, 226f / 255f, 229f / 255f, 1);
        }
        else if (Kart.drift == true)
        {
            Speed.color = new Vector4(113f / 255f, 133f / 255f, 133f / 255f, 1);
        }
        else
        {
            Speed.color = Color.white;
        }


        if (Kart.win == false)
        {
            if (Kart.KartSettings.map == 3)
            {
                Laps.text = (Kart.checkpoints + 1).ToString();
                LapsShadow.text = "Lap " + Laps.text;
            }
            else
            {
                Laps.text = (Kart.completedLaps + 1).ToString();
                LapsShadow.text = "Lap " + Laps.text;
            }
        }

        switch (Kart.KartSettings.map)
        {
            case 1:
                if (Kart.win == false)
                {
                    Laps.text += " / 3";
                    LapsShadow.text += " / 3";
                }

                if (Kart.completedLaps >= 2)
                {
                    Laps.color = Placement.color;
                }
                break;

            case 2:
                if (Kart.win == false)
                {
                    Laps.text += " / 5";
                    LapsShadow.text += " / 5";
                }

                if (Kart.completedLaps >= 4)
                {
                    Laps.color = Placement.color;
                }
                break;

            case 3:
                if (Kart.win == false)
                {
                    Laps.text += " / 3";
                    LapsShadow.text += " / 3";
                }

                if (Kart.checkpoints >= 2)
                {
                    Laps.color = Placement.color;
                }
                break;
        }


        if (Kart.elapsedSeconds < 10)
        {
            Timer.text = Kart.elapsedMinutes.ToString() + ":0";
        }
        else
        {
            Timer.text = Kart.elapsedMinutes.ToString() + ":";
        }

        // Check whether to add extra 0s for empty decimals
        float singleDecimalCheck = (int)((Kart.elapsedFrames - (int)Kart.elapsedFrames) * 100);
        singleDecimalCheck /= 10;

        if (singleDecimalCheck == (int)singleDecimalCheck && Kart.elapsedSeconds != (int)Kart.elapsedSeconds)
        {
            Timer.text += Kart.elapsedSeconds.ToString();
            Timer.text += "0";
        }
        else if (Kart.elapsedSeconds != (int)Kart.elapsedSeconds)
        {
            Timer.text += Kart.elapsedSeconds.ToString();
        }
        else
        {
            Timer.text += Kart.elapsedSeconds.ToString();
            Timer.text += ".00";
        }

        if (Kart.win)
        {
            Timer.color = Placement.color;
        }
        else
        {
            Timer.color = Color.white;
        }

        TimerShadow.text = "Elapsed Time  " + Timer.text;


        switch (Kart.placement)
        {
            case 1:
                Placement.text = "1st";
                Placement.color = new Vector4(201f / 255f, 172f / 255f, 44f / 255f, 1);
                break;

            case 2:
                Placement.text = "2nd";
                Placement.color = new Vector4(193f / 255f, 193f / 255f, 193f / 255f, 1);
                break;

            case 3:
                Placement.text = "3rd";
                Placement.color = new Vector4(181f / 255f, 108f / 255f, 23f / 255f, 1);
                break;

            case 4:
                Placement.text = "4th";
                Placement.color = new Vector4(79f / 255f, 24f / 255f, 181f / 255f, 1);
                break;
        }

        if (Kart.KartSettings.maxPlayers == 1)
        {
            Placement.text = "";
        }

        PlacementShadow.text = Placement.text;


        // Items
        switch (Kart.item)
        {
            case 0:
                if (CurrentItem != null)
                {
                    Destroy(CurrentItem);
                    CurrentItem = null;
                }
                break;

            case 1:
                if (CurrentItem == null)
                {
                    CurrentItem = Instantiate(Boost);

                    CurrentItem.transform.parent = transform;
                    CurrentItem.transform.localEulerAngles = new Vector3(90, 0, 0);
                    CurrentItem.transform.localScale = new Vector3(0.0436f, 0.0974f, 0.0436f);

                    if (Kart.KartSettings.maxPlayers == 2)
                    {
                        CurrentItem.transform.localPosition = new Vector3(0.355f, -0.094f, 0.11f);
                    }
                    else
                    {
                        CurrentItem.transform.localPosition = new Vector3(0.355f, -0.094f, 0.267f);
                    }
                }
                break;

            case 2:
                if (CurrentItem == null)
                {
                    CurrentItem = Instantiate(Banana);

                    CurrentItem.transform.parent = transform;
                    CurrentItem.transform.localEulerAngles = new Vector3(0, 45, 0);
                    CurrentItem.transform.localScale = new Vector3(0.0218f, 0.0487f, 0.0218f);

                    if (Kart.KartSettings.maxPlayers == 2)
                    {
                        CurrentItem.transform.localPosition = new Vector3(0.34f, -0.09f, 0.11f);
                    }
                    else
                    {
                        CurrentItem.transform.localPosition = new Vector3(0.34f, -0.09f, 0.26f);
                    }
                }
                break;

            case 3:
                if (CurrentItem == null)
                {
                    CurrentItem = Instantiate(Shell);

                    CurrentItem.transform.parent = transform;
                    CurrentItem.transform.localEulerAngles = new Vector3(0, 45, 0);
                    CurrentItem.transform.localScale = new Vector3(0.0557f, 0.0113f, 0.0557f);

                    if (Kart.KartSettings.maxPlayers == 2)
                    {
                        CurrentItem.transform.localPosition = new Vector3(0.3405f, -0.103f, 0.11f);
                    }
                    else
                    {
                        CurrentItem.transform.localPosition = new Vector3(0.3405f, -0.103f, 0.258f);
                    }
                }
                break;

            case 4:
                if (CurrentItem == null)
                {
                    CurrentItem = Instantiate(Bouncer);

                    CurrentItem.transform.parent = transform;
                    CurrentItem.transform.localEulerAngles = new Vector3(-47.294f, 306.367f, 28.467f);
                    CurrentItem.transform.localScale = new Vector3(0.0108f, 0.0108f, 0.0108f);

                    if (Kart.KartSettings.maxPlayers == 2)
                    {
                        CurrentItem.transform.localPosition = new Vector3(0.351f, -0.1f, 0.11f);
                    }
                    else
                    {
                        CurrentItem.transform.localPosition = new Vector3(0.351f, -0.1f, 0.266f);
                    }
                }
                break;
        }

        if (CurrentItem != null)
        {
            CurrentItem.transform.eulerAngles += new Vector3(0, 2.5f, 0);
        }


        // Shell Aim
        if (Kart.item == 3)
        {
            ShellAimRenderer.enabled = true;
        }
        else
        {
            ShellAimRenderer.enabled = false;
        }


        // Layers
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Kart0");

            foreach (Transform child2 in child.transform)
            {
                child2.gameObject.layer = LayerMask.NameToLayer("Kart0");

                foreach (Transform child3 in child2.transform)
                {
                    child3.gameObject.layer = LayerMask.NameToLayer("Kart0");

                    foreach (Transform child4 in child3.transform)
                    {
                        child4.gameObject.layer = LayerMask.NameToLayer("Kart0");
                    }
                }
            }
        }

        ShellAim.layer = LayerMask.NameToLayer("Kart0");


        // Two Player Orientation
        if (Kart.KartSettings.maxPlayers == 2)
        {
            Left.transform.localPosition = new Vector3(5, 0, -0.14f);
            Right.transform.localPosition = new Vector3(5, 0, 0.14f);

            LeftBorder.transform.localPosition = new Vector3(0.318f, 0, 0.162f);
            RightBorder.transform.localPosition = new Vector3(0.311f, 0, -0.1592f);
        }
        else
        {
            Left.transform.localPosition = new Vector3(5, 0, 0);
            Right.transform.localPosition = new Vector3(5, 0, 0);

            LeftBorder.transform.localPosition = new Vector3(0.314f, 0, 0.322f);
            RightBorder.transform.localPosition = new Vector3(0.314f, 0, -0.322f);
        }


        // Finished
        if (Kart.win)
        {
            Finish.enabled = true;
            FinishShadow.enabled = true;
        }
        else
        {
            Finish.enabled = false;
            FinishShadow.enabled = false;
        }
    }
}