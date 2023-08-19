using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuAssets : MonoBehaviour
{
    // Menu Control and Kart Data
    public MenuControl Menu;
    public KartSettings KartSettings;
    public Kart1 Kart1;
    public Kart2 Kart2;
    public Kart3 Kart3;
    public Kart4 Kart4;

    // Colors
    public Material Red;
    public Material Yellow;
    public Material Blue;
    public Material Point;
    public Material White;
    public Material Black;

    public bool CharacterColor;
    public bool ignoreCharacterColor;
    public Material[] PlayerKart;
    public Material ElizabethKart;
    public Material KimKart;
    public Material BenKart;
    public Material KelleyKart;
    public Material KirstenKart;
    public Material JustinKart;

    // Menu Assets
    public float timer = 0;

    public GameObject TitleChandler;
    public GameObject TitleKart;
    public Renderer TitleStart;
    public Renderer TitleStartShadow;
    public Renderer TitleCredit;
    public Renderer TitleCreditShadow;

    public GameObject PlayersHowMany;
    public GameObject PlayersLeftArrow;
    public Renderer PlayersLeftArrow1;
    public Renderer PlayersLeftArrow2;
    public Renderer PlayersLeftArrow3;
    public GameObject PlayersRightArrow;
    public Renderer PlayersRightArrow1;
    public Renderer PlayersRightArrow2;
    public Renderer PlayersRightArrow3;
    public GameObject Players1;
    public GameObject Players2;
    public GameObject Players3;
    public GameObject Players4;
    public GameObject PlayersPracticeMode;

    public GameObject CharactersChoose;
    public GameObject[] CharactersPlayer;
    public GameObject[] CharactersStats;
    public TextMeshPro[] CharactersName;
    public Renderer[] CharactersSpeed1;
    public Renderer[] CharactersAcceleration1;
    public Renderer[] CharactersTraction1;
    public Renderer[] CharactersEndurance1;
    public Renderer[] CharactersSpeed2;
    public Renderer[] CharactersAcceleration2;
    public Renderer[] CharactersTraction2;
    public Renderer[] CharactersEndurance2;
    public Renderer[] CharactersSpeed3;
    public Renderer[] CharactersAcceleration3;
    public Renderer[] CharactersTraction3;
    public Renderer[] CharactersEndurance3;
    public Renderer[] CharactersSpeed4;
    public Renderer[] CharactersAcceleration4;
    public Renderer[] CharactersTraction4;
    public Renderer[] CharactersEndurance4;
    public Renderer[] CharactersPlate;
    public GameObject CharactersJoin;

    public GameObject MapChoose;
    public Renderer MapBelknap;
    public Renderer MapBelknapIcon;
    public Renderer MapChandler;
    public Renderer MapChandlerIcon;
    public Renderer MapFurey;
    public Renderer MapFureyIcon;
    public Renderer MapDayNight;
    public Renderer MapDayNightIcon;
    public TextMeshPro MapDayNightText;
    public Material MapBelknapIconDay;
    public Material MapBelknapIconNight;
    public Material MapChandlerIconDay;
    public Material MapChandlerIconNight;
    public Material MapFureyIconDay;
    public Material MapFureyIconNight;

    public TextMeshPro[] ReadyPlayer;
    public TextMeshPro[] ReadyPlayerShadow;
    public GameObject[] ReadyKart;
    public GameObject[] ReadyCharacter;
    public Renderer ReadyMap;
    public Material[] ReadyMapIcon;

    public GameObject Elizabeth;
    public GameObject Kim;
    public GameObject Ben;
    public GameObject Kelley;
    public GameObject Kirsten;
    public GameObject Justin;
    public GameObject Empty;

    public GameObject ThreePlayerCameraObject;
    public Camera ThreePlayerCamera;
    public PlayerInput ThreePlayerCameraPlayerInput;

    // Menu Movement
    public bool menuMove;
    public float menuMoveSpeed;
    public bool menuMoving;

    void Start()
    {
        GameObject Object0 = GameObject.FindWithTag("Settings");
        if (Object0 != null)
        {
            KartSettings = Object0.GetComponent<KartSettings>();
        }

        Menu = GetComponent<MenuControl>();
    }

    void FixedUpdate()
    {
        // Initialize Classes
        GameObject Object1 = GameObject.FindWithTag("Kart1");
        if (Object1 != null)
        {
            Kart1 = Object1.GetComponent<Kart1>();
        }

        GameObject Object2 = GameObject.FindWithTag("Kart2");
        if (Object2 != null)
        {
            Kart2 = Object2.GetComponent<Kart2>();
        }

        GameObject Object3 = GameObject.FindWithTag("Kart3");
        if (Object3 != null)
        {
            Kart3 = Object3.GetComponent<Kart3>();
        }

        GameObject Object4 = GameObject.FindWithTag("Kart4");
        if (Object4 != null)
        {
            Kart4 = Object4.GetComponent<Kart4>();
        }


        // Timer
        timer++;

        if (timer > 120)
        {
            timer = timer - 120;
        }


        // Menu Assets

        // Movement
        TitleChandler.transform.position = new Vector3(7, Mathf.Sin(timer / 10) / 10 + 108.5f, -1);
        TitleKart.transform.eulerAngles = new Vector3(90, 0, Mathf.Sin(timer / 10) * 10 + 105);

        PlayersHowMany.transform.localScale = new Vector3((Mathf.Sin(timer / 10) / 25 + 1) / 2, (Mathf.Sin(timer / 10) / 25 + 1) / 2, 1);
        PlayersLeftArrow.transform.position = new Vector3(88.44f, Mathf.Sin(timer / 10) / 10 + 95.7f, 16.2f);
        PlayersRightArrow.transform.position = new Vector3(111.85f, Mathf.Sin(timer / 10) / 10 + 95.7f, 16.2f);

        CharactersChoose.transform.localScale = new Vector3((Mathf.Sin(timer / 10) / 25 + 1) / 2.5f, (Mathf.Sin(timer / 10) / 25 + 1) / 2.5f, 1);

        MapChoose.transform.localScale = new Vector3(Mathf.Sin(timer / 10) / 25 + 1, Mathf.Sin(timer / 10) / 25 + 1, 1);

        // Rotate Cars
        for (int i = 0; i < Menu.selection.Length; i++)
        {
            if (Menu.menu == 4)
            {
                if (Menu.READY[i])
                {
                    if (i % 2 == 1)
                    {
                        if (ReadyKart[i].transform.eulerAngles.y < 180 + 155.8f)
                        {
                            ReadyKart[i].transform.eulerAngles = Vector3.MoveTowards(ReadyKart[i].transform.eulerAngles, new Vector3(ReadyKart[i].transform.eulerAngles.x, 155.8f, ReadyKart[i].transform.eulerAngles.z), 24);
                        }
                        else
                        {
                            ReadyKart[i].transform.eulerAngles = Vector3.MoveTowards(ReadyKart[i].transform.eulerAngles, new Vector3(ReadyKart[i].transform.eulerAngles.x, 155.8f, ReadyKart[i].transform.eulerAngles.z), -24);
                        }
                    }
                    else
                    {
                        if (ReadyKart[i].transform.eulerAngles.y < 180 + 24.5f)
                        {
                            ReadyKart[i].transform.eulerAngles = Vector3.MoveTowards(ReadyKart[i].transform.eulerAngles, new Vector3(ReadyKart[i].transform.eulerAngles.x, 24.5f, ReadyKart[i].transform.eulerAngles.z), 24);
                        }
                        else
                        {
                            ReadyKart[i].transform.eulerAngles = Vector3.MoveTowards(ReadyKart[i].transform.eulerAngles, new Vector3(ReadyKart[i].transform.eulerAngles.x, 24.5f, ReadyKart[i].transform.eulerAngles.z), -24);

                        }
                    }
                }
                else
                {
                    if (i % 2 == 1)
                    {
                        ReadyKart[i].transform.localEulerAngles += new Vector3(0, 3, 0);
                    }
                    else
                    {
                        ReadyKart[i].transform.localEulerAngles += new Vector3(0, -3, 0);
                    }
                }
            }
            else
            {
                if (i % 2 == 1)
                {
                    ReadyKart[i].transform.eulerAngles = new Vector3(ReadyKart[i].transform.eulerAngles.x, 155.8f, ReadyKart[i].transform.eulerAngles.z);
                }
                else
                {
                    ReadyKart[i].transform.eulerAngles = new Vector3(ReadyKart[i].transform.eulerAngles.x, 24.5f, ReadyKart[i].transform.eulerAngles.z);
                }
            }
        }


        // Timer Events
        if (timer > 0 && timer < 15)
        {
            TitleStart.enabled = true;
            TitleStartShadow.enabled = true;
        }

        if (timer > 15 && timer < 30)
        {
            TitleStart.enabled = true;
            TitleStartShadow.enabled = true;
        }

        if (timer > 30 && timer < 45)
        {
            TitleStart.enabled = false;
            TitleStartShadow.enabled = false;
        }

        if (timer > 45 && timer < 60)
        {
            TitleStart.enabled = false;
            TitleStartShadow.enabled = false;
        }

        if (timer > 60 && timer < 75)
        {
            TitleStart.enabled = true;
            TitleStartShadow.enabled = true;
        }

        if (timer > 75 && timer < 90)
        {
            TitleStart.enabled = true;
            TitleStartShadow.enabled = true;
        }

        if (timer > 90 && timer < 105)
        {
            TitleStart.enabled = false;
            TitleStartShadow.enabled = false;
        }

        if (timer > 105 && timer < 120)
        {
            TitleStart.enabled = false;
            TitleStartShadow.enabled = false;
        }


        // Selections
        if (Menu.MOVE[0].x > 0)
        {
            PlayersRightArrow1.material = Red;
            PlayersRightArrow2.material = Red;
            PlayersRightArrow3.material = Red;
        }
        else if (Menu.MOVECOOLDOWN[0] <= 0)
        {
            PlayersRightArrow1.material = White;
            PlayersRightArrow2.material = White;
            PlayersRightArrow3.material = White;
        }

        if (Menu.MOVE[0].x < 0)
        {
            PlayersLeftArrow1.material = Red;
            PlayersLeftArrow2.material = Red;
            PlayersLeftArrow3.material = Red;
        }
        else if (Menu.MOVECOOLDOWN[0] <= 0)
        {
            PlayersLeftArrow1.material = White;
            PlayersLeftArrow2.material = White;
            PlayersLeftArrow3.material = White;
        }

        if (Menu.selection[0].x == 0 && Menu.selection[0].y == 0)
        {
            Players1.transform.position = new Vector3(100.19f, 96.68f, 16.58f);
            Players2.transform.position = new Vector3(100.19f, 196.68f, 16.58f);
            Players3.transform.position = new Vector3(100.19f, 196.68f, 16.58f);
            Players4.transform.position = new Vector3(100.19f, 196.68f, 16.58f);

            PlayersPracticeMode.transform.localPosition = new Vector3(-0.59f, -2.34f, -3.54f);
        }

        if (Menu.selection[0].x == 1 && Menu.selection[0].y == 0)
        {
            Players1.transform.position = new Vector3(100.19f, 196.68f, 16.58f);
            Players2.transform.position = new Vector3(100.19f, 96.68f, 16.58f);
            Players3.transform.position = new Vector3(100.19f, 196.68f, 16.58f);
            Players4.transform.position = new Vector3(100.19f, 196.68f, 16.58f);

            PlayersPracticeMode.transform.localPosition = new Vector3(9.59f, -2.34f, -3.54f);
        }

        if (Menu.selection[0].x == 2 && Menu.selection[0].y == 0)
        {
            Players1.transform.position = new Vector3(100.19f, 196.68f, 16.58f);
            Players2.transform.position = new Vector3(100.19f, 196.68f, 16.58f);
            Players3.transform.position = new Vector3(100.19f, 96.68f, 16.58f);
            Players4.transform.position = new Vector3(100.19f, 196.68f, 16.58f);

            PlayersPracticeMode.transform.localPosition = new Vector3(9.59f, -2.34f, -3.54f);
        }

        if (Menu.selection[0].x == 3 && Menu.selection[0].y == 0)
        {
            Players1.transform.position = new Vector3(100.19f, 196.68f, 16.58f);
            Players2.transform.position = new Vector3(100.19f, 196.68f, 16.58f);
            Players3.transform.position = new Vector3(100.19f, 196.68f, 16.58f);
            Players4.transform.position = new Vector3(100.19f, 96.68f, 16.58f);

            PlayersPracticeMode.transform.localPosition = new Vector3(9.59f, -2.34f, -3.54f);
        }

        for (int i = 0; i < Menu.selection.Length; i++)
        {
            if (Menu.selection[i].x == 0 && Menu.selection[i].y == 0)
            {
                CharactersPlayer[i].transform.position = new Vector3(194.02f, 100.87f, 16.87f);

                if (i <= KartSettings.joinedPlayers - 1)
                {
                    CharactersName[i].text = "Elizabeth";
                }
                else
                {
                    CharactersName[i].text = "Not Joined";
                }

                if (i == 0)
                {
                    MapBelknap.material = Red;
                    if (KartSettings.night)
                    {
                        MapChandler.material = White;
                        MapFurey.material = White;
                        MapDayNight.material = White;
                    }
                    else
                    {
                        MapChandler.material = Black;
                        MapFurey.material = Black;
                        MapDayNight.material = Black;
                    }
                }

                // Speed Points
                for (int j = 0; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersSpeed1[j].material = Point;
                            break;
                        case 1:
                            CharactersSpeed2[j].material = Point;
                            break;
                        case 2:
                            CharactersSpeed3[j].material = Point;
                            break;
                        case 3:
                            CharactersSpeed4[j].material = Point;
                            break;
                    }
                }
                // Empty Speed Points
                for (int j = 5; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersSpeed1[j].material = Black;
                            break;
                        case 1:
                            CharactersSpeed2[j].material = Black;
                            break;
                        case 2:
                            CharactersSpeed3[j].material = Black;
                            break;
                        case 3:
                            CharactersSpeed4[j].material = Black;
                            break;
                    }
                }

                // Acceleration Points
                for (int j = 0; j < 2; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersAcceleration1[j].material = Point;
                            break;
                        case 1:
                            CharactersAcceleration2[j].material = Point;
                            break;
                        case 2:
                            CharactersAcceleration3[j].material = Point;
                            break;
                        case 3:
                            CharactersAcceleration4[j].material = Point;
                            break;
                    }
                }
                // Empty Acceleration Points
                for (int j = 2; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersAcceleration1[j].material = Black;
                            break;
                        case 1:
                            CharactersAcceleration2[j].material = Black;
                            break;
                        case 2:
                            CharactersAcceleration3[j].material = Black;
                            break;
                        case 3:
                            CharactersAcceleration4[j].material = Black;
                            break;
                    }
                }

                // Traction Points
                for (int j = 0; j < 4; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersTraction1[j].material = Point;
                            break;
                        case 1:
                            CharactersTraction2[j].material = Point;
                            break;
                        case 2:
                            CharactersTraction3[j].material = Point;
                            break;
                        case 3:
                            CharactersTraction4[j].material = Point;
                            break;
                    }
                }
                // Empty Traction Points
                for (int j = 4; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersTraction1[j].material = Black;
                            break;
                        case 1:
                            CharactersTraction2[j].material = Black;
                            break;
                        case 2:
                            CharactersTraction3[j].material = Black;
                            break;
                        case 3:
                            CharactersTraction4[j].material = Black;
                            break;
                    }
                }

                // Endurance Points
                for (int j = 0; j < 1; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersEndurance1[j].material = Point;
                            break;
                        case 1:
                            CharactersEndurance2[j].material = Point;
                            break;
                        case 2:
                            CharactersEndurance3[j].material = Point;
                            break;
                        case 3:
                            CharactersEndurance4[j].material = Point;
                            break;
                    }
                }
                // Empty Endurance Points 
                for (int j = 1; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersEndurance1[j].material = Black;
                            break;
                        case 1:
                            CharactersEndurance2[j].material = Black;
                            break;
                        case 2:
                            CharactersEndurance3[j].material = Black;
                            break;
                        case 3:
                            CharactersEndurance4[j].material = Black;
                            break;
                    }
                }
            }

            if (Menu.selection[i].x == 1 && Menu.selection[i].y == 0)
            {
                CharactersPlayer[i].transform.position = new Vector3(200f, 100.87f, 16.87f);

                if (i <= KartSettings.joinedPlayers - 1)
                {
                    CharactersName[i].text = "Kim";
                }
                else
                {
                    CharactersName[i].text = "Not Joined";
                }

                if (i == 0)
                {
                    MapChandler.material = Red;
                    if (KartSettings.night)
                    {
                        MapBelknap.material = White;
                        MapFurey.material = White;
                        MapDayNight.material = White;
                    }
                    else
                    {
                        MapBelknap.material = Black;
                        MapFurey.material = Black;
                        MapDayNight.material = Black;
                    }
                }

                // Speed Points
                for (int j = 0; j < 3; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersSpeed1[j].material = Point;
                            break;
                        case 1:
                            CharactersSpeed2[j].material = Point;
                            break;
                        case 2:
                            CharactersSpeed3[j].material = Point;
                            break;
                        case 3:
                            CharactersSpeed4[j].material = Point;
                            break;
                    }
                }
                // Empty Speed Points
                for (int j = 3; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersSpeed1[j].material = Black;
                            break;
                        case 1:
                            CharactersSpeed2[j].material = Black;
                            break;
                        case 2:
                            CharactersSpeed3[j].material = Black;
                            break;
                        case 3:
                            CharactersSpeed4[j].material = Black;
                            break;
                    }
                }

                // Acceleration Points
                for (int j = 0; j < 3; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersAcceleration1[j].material = Point;
                            break;
                        case 1:
                            CharactersAcceleration2[j].material = Point;
                            break;
                        case 2:
                            CharactersAcceleration3[j].material = Point;
                            break;
                        case 3:
                            CharactersAcceleration4[j].material = Point;
                            break;
                    }
                }
                // Empty Acceleration Points
                for (int j = 3; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersAcceleration1[j].material = Black;
                            break;
                        case 1:
                            CharactersAcceleration2[j].material = Black;
                            break;
                        case 2:
                            CharactersAcceleration3[j].material = Black;
                            break;
                        case 3:
                            CharactersAcceleration4[j].material = Black;
                            break;
                    }
                }

                // Traction Points
                for (int j = 0; j < 3; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersTraction1[j].material = Point;
                            break;
                        case 1:
                            CharactersTraction2[j].material = Point;
                            break;
                        case 2:
                            CharactersTraction3[j].material = Point;
                            break;
                        case 3:
                            CharactersTraction4[j].material = Point;
                            break;
                    }
                }
                // Empty Traction Points
                for (int j = 3; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersTraction1[j].material = Black;
                            break;
                        case 1:
                            CharactersTraction2[j].material = Black;
                            break;
                        case 2:
                            CharactersTraction3[j].material = Black;
                            break;
                        case 3:
                            CharactersTraction4[j].material = Black;
                            break;
                    }
                }

                // Endurance Points
                for (int j = 0; j < 3; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersEndurance1[j].material = Point;
                            break;
                        case 1:
                            CharactersEndurance2[j].material = Point;
                            break;
                        case 2:
                            CharactersEndurance3[j].material = Point;
                            break;
                        case 3:
                            CharactersEndurance4[j].material = Point;
                            break;
                    }
                }
                // Empty Endurance Points
                for (int j = 3; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersEndurance1[j].material = Black;
                            break;
                        case 1:
                            CharactersEndurance2[j].material = Black;
                            break;
                        case 2:
                            CharactersEndurance3[j].material = Black;
                            break;
                        case 3:
                            CharactersEndurance4[j].material = Black;
                            break;
                    }
                }
            }

            if (Menu.selection[i].x == 2 && Menu.selection[i].y == 0)
            {
                CharactersPlayer[i].transform.position = new Vector3(205.864f, 100.87f, 16.87f);

                if (i <= KartSettings.joinedPlayers - 1)
                {
                    CharactersName[i].text = "Ben";
                }
                else
                {
                    CharactersName[i].text = "Not Joined";
                }

                if (i == 0)
                {
                    MapFurey.material = Red;
                    if (KartSettings.night)
                    {
                        MapBelknap.material = White;
                        MapChandler.material = White;
                        MapDayNight.material = White;
                    }
                    else
                    {
                        MapBelknap.material = Black;
                        MapChandler.material = Black;
                        MapDayNight.material = Black;
                    }
                }

                // Speed Points
                for (int j = 0; j < 3; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersSpeed1[j].material = Point;
                            break;
                        case 1:
                            CharactersSpeed2[j].material = Point;
                            break;
                        case 2:
                            CharactersSpeed3[j].material = Point;
                            break;
                        case 3:
                            CharactersSpeed4[j].material = Point;
                            break;
                    }
                }
                // Empty Speed Points
                for (int j = 3; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersSpeed1[j].material = Black;
                            break;
                        case 1:
                            CharactersSpeed2[j].material = Black;
                            break;
                        case 2:
                            CharactersSpeed3[j].material = Black;
                            break;
                        case 3:
                            CharactersSpeed4[j].material = Black;
                            break;
                    }
                }

                // Acceleration Points
                for (int j = 0; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersAcceleration1[j].material = Point;
                            break;
                        case 1:
                            CharactersAcceleration2[j].material = Point;
                            break;
                        case 2:
                            CharactersAcceleration3[j].material = Point;
                            break;
                        case 3:
                            CharactersAcceleration4[j].material = Point;
                            break;
                    }
                }
                // Empty Acceleration Points
                for (int j = 5; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersAcceleration1[j].material = Black;
                            break;
                        case 1:
                            CharactersAcceleration2[j].material = Black;
                            break;
                        case 2:
                            CharactersAcceleration3[j].material = Black;
                            break;
                        case 3:
                            CharactersAcceleration4[j].material = Black;
                            break;
                    }
                }

                // Traction Points
                for (int j = 0; j < 1; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersTraction1[j].material = Point;
                            break;
                        case 1:
                            CharactersTraction2[j].material = Point;
                            break;
                        case 2:
                            CharactersTraction3[j].material = Point;
                            break;
                        case 3:
                            CharactersTraction4[j].material = Point;
                            break;
                    }
                }
                // Empty Traction Points
                for (int j = 1; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersTraction1[j].material = Black;
                            break;
                        case 1:
                            CharactersTraction2[j].material = Black;
                            break;
                        case 2:
                            CharactersTraction3[j].material = Black;
                            break;
                        case 3:
                            CharactersTraction4[j].material = Black;
                            break;
                    }
                }

                // Endurance Points
                for (int j = 0; j < 3; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersEndurance1[j].material = Point;
                            break;
                        case 1:
                            CharactersEndurance2[j].material = Point;
                            break;
                        case 2:
                            CharactersEndurance3[j].material = Point;
                            break;
                        case 3:
                            CharactersEndurance4[j].material = Point;
                            break;
                    }
                }
                // Empty Endurance Points
                for (int j = 3; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersEndurance1[j].material = Black;
                            break;
                        case 1:
                            CharactersEndurance2[j].material = Black;
                            break;
                        case 2:
                            CharactersEndurance3[j].material = Black;
                            break;
                        case 3:
                            CharactersEndurance4[j].material = Black;
                            break;
                    }
                }
            }

            if (Menu.selection[i].x == 0 && Menu.selection[i].y == 1)
            {
                CharactersPlayer[i].transform.position = new Vector3(194.02f, 94.87f, 16.87f);

                if (i <= KartSettings.joinedPlayers - 1)
                {
                    CharactersName[i].text = "Kelley";
                }
                else
                {
                    CharactersName[i].text = "Not Joined";
                }

                // Speed Points
                for (int j = 0; j < 2; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersSpeed1[j].material = Point;
                            break;
                        case 1:
                            CharactersSpeed2[j].material = Point;
                            break;
                        case 2:
                            CharactersSpeed3[j].material = Point;
                            break;
                        case 3:
                            CharactersSpeed4[j].material = Point;
                            break;
                    }
                }
                // Empty Speed Points
                for (int j = 2; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersSpeed1[j].material = Black;
                            break;
                        case 1:
                            CharactersSpeed2[j].material = Black;
                            break;
                        case 2:
                            CharactersSpeed3[j].material = Black;
                            break;
                        case 3:
                            CharactersSpeed4[j].material = Black;
                            break;
                    }
                }

                // Acceleration Points
                for (int j = 0; j < 2; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersAcceleration1[j].material = Point;
                            break;
                        case 1:
                            CharactersAcceleration2[j].material = Point;
                            break;
                        case 2:
                            CharactersAcceleration3[j].material = Point;
                            break;
                        case 3:
                            CharactersAcceleration4[j].material = Point;
                            break;
                    }
                }
                // Empty Acceleration Points
                for (int j = 2; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersAcceleration1[j].material = Black;
                            break;
                        case 1:
                            CharactersAcceleration2[j].material = Black;
                            break;
                        case 2:
                            CharactersAcceleration3[j].material = Black;
                            break;
                        case 3:
                            CharactersAcceleration4[j].material = Black;
                            break;
                    }
                }

                // Traction Points
                for (int j = 0; j < 3; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersTraction1[j].material = Point;
                            break;
                        case 1:
                            CharactersTraction2[j].material = Point;
                            break;
                        case 2:
                            CharactersTraction3[j].material = Point;
                            break;
                        case 3:
                            CharactersTraction4[j].material = Point;
                            break;
                    }
                }
                // Empty Traction Points
                for (int j = 3; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersTraction1[j].material = Black;
                            break;
                        case 1:
                            CharactersTraction2[j].material = Black;
                            break;
                        case 2:
                            CharactersTraction3[j].material = Black;
                            break;
                        case 3:
                            CharactersTraction4[j].material = Black;
                            break;
                    }
                }

                // Endurance Points
                for (int j = 0; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersEndurance1[j].material = Point;
                            break;
                        case 1:
                            CharactersEndurance2[j].material = Point;
                            break;
                        case 2:
                            CharactersEndurance3[j].material = Point;
                            break;
                        case 3:
                            CharactersEndurance4[j].material = Point;
                            break;
                    }
                }
                // Empty Endurance Points
                for (int j = 5; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersEndurance1[j].material = Black;
                            break;
                        case 1:
                            CharactersEndurance2[j].material = Black;
                            break;
                        case 2:
                            CharactersEndurance3[j].material = Black;
                            break;
                        case 3:
                            CharactersEndurance4[j].material = Black;
                            break;
                    }
                }
            }

            if (Menu.selection[i].x == 1 && Menu.selection[i].y == 1)
            {
                CharactersPlayer[i].transform.position = new Vector3(200f, 94.87f, 16.87f);

                if (i <= KartSettings.joinedPlayers - 1)
                {
                    CharactersName[i].text = "Kirsten";
                }
                else
                {
                    CharactersName[i].text = "Not Joined";
                }

                if (i == 0)
                {
                    MapDayNight.material = Red;
                    if (KartSettings.night)
                    {
                        MapBelknap.material = White;
                        MapChandler.material = White;
                        MapFurey.material = White;
                    }
                    else
                    {
                        MapBelknap.material = Black;
                        MapChandler.material = Black;
                        MapFurey.material = Black;
                    }
                }

                // Speed Points
                for (int j = 0; j < 4; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersSpeed1[j].material = Point;
                            break;
                        case 1:
                            CharactersSpeed2[j].material = Point;
                            break;
                        case 2:
                            CharactersSpeed3[j].material = Point;
                            break;
                        case 3:
                            CharactersSpeed4[j].material = Point;
                            break;
                    }
                }
                // Empty Speed Points
                for (int j = 4; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersSpeed1[j].material = Black;
                            break;
                        case 1:
                            CharactersSpeed2[j].material = Black;
                            break;
                        case 2:
                            CharactersSpeed3[j].material = Black;
                            break;
                        case 3:
                            CharactersSpeed4[j].material = Black;
                            break;
                    }
                }

                // Acceleration Points
                for (int j = 0; j < 2; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersAcceleration1[j].material = Point;
                            break;
                        case 1:
                            CharactersAcceleration2[j].material = Point;
                            break;
                        case 2:
                            CharactersAcceleration3[j].material = Point;
                            break;
                        case 3:
                            CharactersAcceleration4[j].material = Point;
                            break;
                    }
                }
                // Empty Acceleration Points
                for (int j = 2; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersAcceleration1[j].material = Black;
                            break;
                        case 1:
                            CharactersAcceleration2[j].material = Black;
                            break;
                        case 2:
                            CharactersAcceleration3[j].material = Black;
                            break;
                        case 3:
                            CharactersAcceleration4[j].material = Black;
                            break;
                    }
                }

                // Traction Points
                for (int j = 0; j < 2; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersTraction1[j].material = Point;
                            break;
                        case 1:
                            CharactersTraction2[j].material = Point;
                            break;
                        case 2:
                            CharactersTraction3[j].material = Point;
                            break;
                        case 3:
                            CharactersTraction4[j].material = Point;
                            break;
                    }
                }
                // Empty Traction Points
                for (int j = 2; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersTraction1[j].material = Black;
                            break;
                        case 1:
                            CharactersTraction2[j].material = Black;
                            break;
                        case 2:
                            CharactersTraction3[j].material = Black;
                            break;
                        case 3:
                            CharactersTraction4[j].material = Black;
                            break;
                    }
                }

                // Endurance Points
                for (int j = 0; j < 4; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersEndurance1[j].material = Point;
                            break;
                        case 1:
                            CharactersEndurance2[j].material = Point;
                            break;
                        case 2:
                            CharactersEndurance3[j].material = Point;
                            break;
                        case 3:
                            CharactersEndurance4[j].material = Point;
                            break;
                    }
                }
                // Empty Endurance Points
                for (int j = 4; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersEndurance1[j].material = Black;
                            break;
                        case 1:
                            CharactersEndurance2[j].material = Black;
                            break;
                        case 2:
                            CharactersEndurance3[j].material = Black;
                            break;
                        case 3:
                            CharactersEndurance4[j].material = Black;
                            break;
                    }
                }
            }

            if (Menu.selection[i].x == 2 && Menu.selection[i].y == 1)
            {
                CharactersPlayer[i].transform.position = new Vector3(205.86f, 94.87f, 16.87f);

                if (i <= KartSettings.joinedPlayers - 1)
                {
                    CharactersName[i].text = "Justin";
                }
                else
                {
                    CharactersName[i].text = "Not Joined";
                }

                // Speed Points
                for (int j = 0; j < 3; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersSpeed1[j].material = Point;
                            break;
                        case 1:
                            CharactersSpeed2[j].material = Point;
                            break;
                        case 2:
                            CharactersSpeed3[j].material = Point;
                            break;
                        case 3:
                            CharactersSpeed4[j].material = Point;
                            break;
                    }
                }
                // Empty Speed Points
                for (int j = 3; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersSpeed1[j].material = Black;
                            break;
                        case 1:
                            CharactersSpeed2[j].material = Black;
                            break;
                        case 2:
                            CharactersSpeed3[j].material = Black;
                            break;
                        case 3:
                            CharactersSpeed4[j].material = Black;
                            break;
                    }
                }

                // Acceleration Points
                for (int j = 0; j < 3; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersAcceleration1[j].material = Point;
                            break;
                        case 1:
                            CharactersAcceleration2[j].material = Point;
                            break;
                        case 2:
                            CharactersAcceleration3[j].material = Point;
                            break;
                        case 3:
                            CharactersAcceleration4[j].material = Point;
                            break;
                    }
                }
                // Empty Acceleration Points
                for (int j = 3; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersAcceleration1[j].material = Black;
                            break;
                        case 1:
                            CharactersAcceleration2[j].material = Black;
                            break;
                        case 2:
                            CharactersAcceleration3[j].material = Black;
                            break;
                        case 3:
                            CharactersAcceleration4[j].material = Black;
                            break;
                    }
                }

                // Traction Points
                for (int j = 0; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersTraction1[j].material = Point;
                            break;
                        case 1:
                            CharactersTraction2[j].material = Point;
                            break;
                        case 2:
                            CharactersTraction3[j].material = Point;
                            break;
                        case 3:
                            CharactersTraction4[j].material = Point;
                            break;
                    }
                }
                // Empty Traction Points
                for (int j = 5; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersTraction1[j].material = Black;
                            break;
                        case 1:
                            CharactersTraction2[j].material = Black;
                            break;
                        case 2:
                            CharactersTraction3[j].material = Black;
                            break;
                        case 3:
                            CharactersTraction4[j].material = Black;
                            break;
                    }
                }

                // Endurance Points
                for (int j = 0; j < 1; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersEndurance1[j].material = Point;
                            break;
                        case 1:
                            CharactersEndurance2[j].material = Point;
                            break;
                        case 2:
                            CharactersEndurance3[j].material = Point;
                            break;
                        case 3:
                            CharactersEndurance4[j].material = Point;
                            break;
                    }
                }
                // Empty Endurance Points
                for (int j = 1; j < 5; j++)
                {
                    switch (i)
                    {
                        case 0:
                            CharactersEndurance1[j].material = Black;
                            break;
                        case 1:
                            CharactersEndurance2[j].material = Black;
                            break;
                        case 2:
                            CharactersEndurance3[j].material = Black;
                            break;
                        case 3:
                            CharactersEndurance4[j].material = Black;
                            break;
                    }
                }
            }

            if (KartSettings.character[i + 1] == "none")
            {
                CharactersPlate[i].material = Black;
            }
            else
            {
                CharactersPlate[i].material = White;
            }

            if (KartSettings.maxPlayers - 1 < i && Menu.menu == 2)
            {
                CharactersPlayer[i].transform.position = new Vector3(200f, 200f, 16.87f);
                CharactersStats[i].transform.position = new Vector3(200f, 200f, 17.19f);
            }
        }

        // Day or Night Button
        if (KartSettings.night)
        {
            MapDayNightIcon.material = Blue;
            MapDayNightText.text = "NIGHT";
            MapDayNightText.color = Color.white;

            MapBelknapIcon.material = MapBelknapIconNight;
            MapChandlerIcon.material = MapChandlerIconNight;
            MapFureyIcon.material = MapFureyIconNight;
        }
        else
        {
            MapDayNightIcon.material = Yellow;
            MapDayNightText.text = "DAY";
            MapDayNightText.color = Color.black;

            MapBelknapIcon.material = MapBelknapIconDay;
            MapChandlerIcon.material = MapChandlerIconDay;
            MapFureyIcon.material = MapFureyIconDay;
        }

        // Ready Text
        for (int i = 0; i < Menu.selection.Length; i++)
        {
            if (Menu.READY[i] && KartSettings.maxPlayers - 1 >= i)
            {
                ReadyPlayer[i].text = "Ready!";
                ReadyPlayerShadow[i].text = "Ready!";

                ReadyPlayer[i].color = Color.white;
                ReadyPlayerShadow[i].color = Color.black;
            }
            else if (Menu.READY[i] == false && KartSettings.maxPlayers - 1 >= i)
            {
                ReadyPlayer[i].text = "Ready?";
                ReadyPlayerShadow[i].text = "Ready?";

                switch (i)
                {
                    case 0:
                        ReadyPlayer[i].color = Color.red;
                        break;
                    case 1:
                        ReadyPlayer[i].color = Color.blue;
                        break;
                    case 2:
                        ReadyPlayer[i].color = Color.green;
                        break;
                    case 3:
                        ReadyPlayer[i].color = Color.yellow;
                        break;
                }
                ReadyPlayerShadow[i].color = Color.black;
            }
            else
            {
                ReadyPlayer[i].text = "";
                ReadyPlayerShadow[i].text = "";

                ReadyPlayer[i].color = Color.white;
                ReadyPlayerShadow[i].color = Color.black;

                if (Menu.menu == 4)
                {
                    ReadyKart[i].transform.position = new Vector3(400, 200, 0);
                }
            }

            // Same Character Check
            if (Menu.menu > 2)
            {
                for (int j = 0; j < Menu.selection.Length; j++)
                {
                    if (KartSettings.character[i + 1] == "none")
                    {
                        break;
                    }

                    if (KartSettings.character[i + 1] == KartSettings.character[j + 1] && KartSettings.character[j + 1] != "none" && i != j)
                    {
                        CharacterColor = false;
                    }
                }
            }
            else
            {
                CharacterColor = true;
            }

            // Ready Characters
            if (Menu.menu != 4)
            {
                if (ReadyCharacter[i] != Empty)
                {
                    Destroy(ReadyCharacter[i]);
                }

                ReadyCharacter[i] = Empty;
            }
            else if (KartSettings.maxPlayers - 1 >= i)
            {
                bool currentCharacterColor = CharacterColor;

                if (ignoreCharacterColor)
                {
                    currentCharacterColor = !CharacterColor;
                }

                switch (KartSettings.character[i + 1])
                {
                    case "Elizabeth":
                        if (ReadyCharacter[i] == Empty)
                        {
                            ReadyCharacter[i] = Instantiate(Elizabeth);

                            if (currentCharacterColor)
                            {
                                ReadyKart[i].GetComponent<Renderer>().material = ElizabethKart;
                            }
                            else
                            {
                                ReadyKart[i].GetComponent<Renderer>().material = PlayerKart[i];
                            }
                        }
                        break;

                    case "Kim":
                        if (ReadyCharacter[i] == Empty)
                        {
                            ReadyCharacter[i] = Instantiate(Kim);

                            if (currentCharacterColor)
                            {
                                ReadyKart[i].GetComponent<Renderer>().material = KimKart;
                            }
                            else
                            {
                                ReadyKart[i].GetComponent<Renderer>().material = PlayerKart[i];
                            }
                        }
                        break;

                    case "Ben":
                        if (ReadyCharacter[i] == Empty)
                        {
                            ReadyCharacter[i] = Instantiate(Ben);

                            if (currentCharacterColor)
                            {
                                ReadyKart[i].GetComponent<Renderer>().material = BenKart;
                            }
                            else
                            {
                                ReadyKart[i].GetComponent<Renderer>().material = PlayerKart[i];
                            }
                        }
                        break;

                    case "Kelley":
                        if (ReadyCharacter[i] == Empty)
                        {
                            ReadyCharacter[i] = Instantiate(Kelley);

                            if (currentCharacterColor)
                            {
                                ReadyKart[i].GetComponent<Renderer>().material = KelleyKart;
                            }
                            else
                            {
                                ReadyKart[i].GetComponent<Renderer>().material = PlayerKart[i];
                            }
                        }
                        break;

                    case "Kirsten":
                        if (ReadyCharacter[i] == Empty)
                        {
                            ReadyCharacter[i] = Instantiate(Kirsten);

                            if (currentCharacterColor)
                            {
                                ReadyKart[i].GetComponent<Renderer>().material = KirstenKart;
                            }
                            else
                            {
                                ReadyKart[i].GetComponent<Renderer>().material = PlayerKart[i];
                            }
                        }
                        break;

                    case "Justin":
                        if (ReadyCharacter[i] == Empty)
                        {
                            ReadyCharacter[i] = Instantiate(Justin);

                            if (currentCharacterColor)
                            {
                                ReadyKart[i].GetComponent<Renderer>().material = JustinKart;
                            }
                            else
                            {
                                ReadyKart[i].GetComponent<Renderer>().material = PlayerKart[i];
                            }
                        }
                        break;
                }

                ReadyCharacter[i].transform.parent = ReadyKart[i].transform;
                ReadyCharacter[i].transform.position = ReadyKart[i].transform.position;
                ReadyCharacter[i].transform.localPosition = new Vector3(-0.33f, 1.83f, 0);
                ReadyCharacter[i].transform.localEulerAngles = Vector3.zero;
                ReadyCharacter[i].transform.localScale = new Vector3(1.26f, 5.18f, 1.7f);
            }
        }

        // All Players Joined
        if (KartSettings.joinedPlayers < KartSettings.maxPlayers)
        {
            CharactersJoin.transform.localPosition = new Vector3(7.38f, -5.828f, -13.491f);
        }
        else
        {
            CharactersJoin.transform.localPosition = new Vector3(7.38f, -15.828f, -13.491f);
        }


        // Map on Ready Screen
        if (Menu.menu >= 4)
        {
            ReadyMap.material = ReadyMapIcon[KartSettings.map - 1];
        }


        // Set Characters
        if (Menu.menu == 5)
        {
            for (int i = 0; i < Menu.selection.Length; i++)
            {
                GameObject Character = null;
                switch (KartSettings.character[i + 1])
                {
                    case "Elizabeth":
                        Character = Elizabeth;
                        break;
                    case "Kim":
                        Character = Kim;
                        break;
                    case "Ben":
                        Character = Ben;
                        break;
                    case "Kelley":
                        Character = Kelley;
                        break;
                    case "Kirsten":
                        Character = Kirsten;
                        break;
                    case "Justin":
                        Character = Justin;
                        break;
                    case "none":
                        Character = Empty;
                        break;
                }

                switch (i)
                {
                    case 0:
                        Kart1.Character = Instantiate(Character);
                        Kart1.transform.GetChild(0).GetComponent<Renderer>().material = ReadyKart[i].GetComponent<Renderer>().material;
                        break;

                    case 1:
                        Kart2.Character = Instantiate(Character);
                        Kart2.transform.GetChild(0).GetComponent<Renderer>().material = ReadyKart[i].GetComponent<Renderer>().material;
                        break;

                    case 2:
                        Kart3.Character = Instantiate(Character);
                        Kart3.transform.GetChild(0).GetComponent<Renderer>().material = ReadyKart[i].GetComponent<Renderer>().material;
                        break;

                    case 3:
                        Kart4.Character = Instantiate(Character);
                        Kart4.transform.GetChild(0).GetComponent<Renderer>().material = ReadyKart[i].GetComponent<Renderer>().material;
                        break;
                }
            }
        }


        // Three Player Camera
        if (KartSettings.maxPlayers == 3 && Menu.menu == 5)
        {
            ThreePlayerCamera.enabled = true;
            ThreePlayerCameraPlayerInput.enabled = true;
        }
        else if (Menu.menu == 5)
        {
            Destroy(ThreePlayerCameraObject);
        }


        // Menu Camera Movement
        if (menuMove)
        {
            if (Mathf.Abs(Vector3.Distance(transform.position, new Vector3(Menu.menu * 100, 100, 0))) > menuMoveSpeed)
            {
                menuMoving = true;

                transform.position = Vector3.MoveTowards(transform.position, new Vector3(Menu.menu * 100, 100, 0), menuMoveSpeed);
            }
            else
            {
                menuMoving = false;

                transform.position = new Vector3(Menu.menu * 100, 100, 0);
            }
        }
        else
        {
            menuMoving = false;
            transform.position = new Vector3(Menu.menu * 100, 100, 0);
        }

        Menu.menuMoving = menuMoving;
    }
}